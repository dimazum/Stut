export function getErrorMessage(err: unknown): string {
  if (!err) return 'Unknown error';       // null или undefined
  if (err instanceof Error) return err.message; // стандартная ошибка
  if (typeof err === 'string') return err;      // строка
  if (typeof err === 'object' && 'message' in err && typeof (err as any).message === 'string') {
    return (err as any).message;                 // объект с message
  }
  return 'Unknown error';                        // fallback
}