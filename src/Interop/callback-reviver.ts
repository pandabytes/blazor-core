// We import DotNet namespace like this just to get the type declaration
// but we don't want to bundle it in our output .js file. Because the
// namespace DotNet is already available in the browser when Blazor
// loads the app
// Idea taken from here https://stackoverflow.com/questions/74723484/how-to-get-vite-to-not-import-bundle-an-external-dependency/74723564
import type { DotNet as NpmDotNet } from '@microsoft/dotnet-js-interop';

declare global {
  const DotNet: typeof NpmDotNet;
}

type CallbackInterop = {
  assemblyName: string,
  isAsync: boolean,
  isCallBackInterop: boolean,
  dotNetRef: NpmDotNet.DotNetObject,
}

function isCallbackInterop(obj: any): obj is CallbackInterop {
  const isObjecType = (obj && typeof obj === 'object');
  if (!isObjecType) {
    return false;
  }

  // These properties are defined in C# BaseCallbackInterop class and
  // they're serialized to camel case by Blazor JSON serializer options
  const mustHaveProps = [
    'isAsync',
    'isCallbackInterop',
    'assemblyName',
    'dotNetRef'
  ]

  const haveAllProps = mustHaveProps.every(propName => obj.hasOwnProperty(propName));
  return haveAllProps && obj['assemblyName'] === 'Blazor.Interop';
}

class CallbackReviver {
  /**
   * We only want to register this reviver only once
   * for the whole lifetime of the app.
   */
  private registered: boolean;

  constructor() {
    this.registered = false;
  }

  public registerReviver() {
    if (this.registered) {
      return;
    }

    /**
     * Taken from:
     *  - https://remibou.github.io/How-to-send-callback-to-JS-Interop-in-Blazor/
     *  - https://remibou.github.io/How-to-keep-js-object-reference-in-Blazor/
     * Essentially this converts any C# callback (Func, Action, & EventCallback) to
     * a JS function that can be invoked by JS code.
     *
     * According to the author in that article, DotNet can have multiple revivers so
     * if a reviver cannot handle a value it will be passed to the next reviver.
     * If this reviver can handle a value and potentially "transform" it (like what we do here),
     * then the "transformed" value will be passed to the next reviver until
     * there's no more revivers in the chain.
     *
     * Quote from the author:
     * This reviver will be called for every serialized object send to JS via JSInterop
     * (even deep in the object graph, so you can send arrays or complex objects with
     * JsRuntimeObjectRef properties).
     * 
     */
    DotNet.attachReviver((_, value) => {
      if (isCallbackInterop(value)) {
        const dotNetRef = value.dotNetRef;

        if (value.isAsync) {
          // This callback will return a Promise
          return function() {
            return dotNetRef.invokeMethodAsync('Invoke', ...arguments);
          }
        }

        return function() {
          return dotNetRef.invokeMethod('Invoke', ...arguments);
        }
      }

      return value;
    });

    this.registered = true;
  }
}

export const CallbackReviverObj = new CallbackReviver();
