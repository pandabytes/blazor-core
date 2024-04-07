/**
 * See https://learn.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/call-dotnet-from-javascript?view=aspnetcore-7.0#create-javascript-object-and-data-references-to-pass-to-net
 */
declare global {
  const DotNet: {
    attachReviver(callBack: (key: string, value: any) => Function | any): void;
  }
}

interface DotNetObjectReference {
  invokeMethod(methodName: string, ...args: any[]): any;
  invokeMethodAsync(methodName: string, ...args: any[]): Promise<any>;
}

type CallbackInterop = {
  assemblyName: string,
  isAsync: boolean,
  isCallBackInterop: boolean,
  dotNetRef: DotNetObjectReference,
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

class DotNetReviverHandler {
  private registered: boolean;

  constructor() {
    this.registered = false;
  }

  public registerAttachReviver() {
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
    DotNet.attachReviver((key, value) => {
      if (isCallbackInterop(value)) {
        const dotNetRef = value.dotNetRef;

        if (value.isAsync) {
          // This callback will return a Promise
          return function() {
            return dotNetRef.invokeMethodAsync('Invoke', ...arguments);
          };
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

export const DotNetReviverHandlerObj = new DotNetReviverHandler();
