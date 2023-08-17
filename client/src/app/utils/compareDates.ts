export default function compareDates(
  date01: Date | string | null,
  date02: Date | string | null
): number {
  if (!date01) {
    return -1;
  }

  if (!date02) {
    return 1;
  }

  if (typeof date01 === 'string') {
    date01 = new Date(Date.parse(date01));
  }

  if (typeof date02 === 'string') {
    date02 = new Date(Date.parse(date02));
  }

  return date02.getTime() - date01.getTime();
}