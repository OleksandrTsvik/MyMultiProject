const defaultlocales: Intl.LocalesArgument = 'en-US';

const defaultOptions: Intl.DateTimeFormatOptions = {
  weekday: 'long',
  year: 'numeric',
  month: 'long',
  day: '2-digit',
  hour: '2-digit',
  minute: '2-digit',
  second: '2-digit',
  hour12: false
};

export default function dateFormat(
  date: Date | string,
  locales: Intl.LocalesArgument = defaultlocales,
  options: Intl.DateTimeFormatOptions = defaultOptions
): string {
  if (typeof date === 'string') {
    date = new Date(Date.parse(date));
  }

  return date.toLocaleDateString(locales, options);
}