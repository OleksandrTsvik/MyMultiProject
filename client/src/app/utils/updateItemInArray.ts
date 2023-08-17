/**
 * Returns 1 if the element has been updated and -1 otherwise.
 * @param array the array in which the element needs to be updated.
 * @param item new value.
 * @param predicate find calls predicate once for each element of the array, in ascending
 * order, until it finds one where predicate returns true. If such an element is found,
 * findIndex immediately returns that element index. Otherwise, findIndex returns -1.
 */
export default function updateItemInArray<T>(
  array: T[],
  item: Partial<T>,
  predicate: (value: T, index: number, obj: T[]) => unknown
): number {
  let updateIndex = array.findIndex(predicate);

  if (updateIndex < 0) {
    return -1;
  }

  array[updateIndex] = {
    ...array[updateIndex],
    ...item
  };

  return 1;
}