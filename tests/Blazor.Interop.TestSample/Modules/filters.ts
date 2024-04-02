
export function filterNumberBy(array: number[], predicate: (n: number) => boolean): number[] {
  return array.filter(predicate);
}

export function logCallback(array: number[], callback: (n: number) => void) {
  array.forEach(callback);
}
