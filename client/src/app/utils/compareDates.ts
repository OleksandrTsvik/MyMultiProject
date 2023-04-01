export default function compareDates(
    date01: string | null,
    date02: string | null
): number {
    if (!date01) {
        return -1;
    } else if (!date02) {
        return 1;
    }

    return Date.parse(date02) - Date.parse(date01);
}