
export function logCallback(array: number[], callback: (n: number) => void) {
  array.forEach(callback);
}

export function formatCurrentDateTime(formatter: (dateTime: Date) => string) {
  return formatter(new Date());
}
