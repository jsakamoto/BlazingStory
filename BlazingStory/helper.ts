export const setLocalStorageItem = (key: string, value: string): void => {
    localStorage.setItem(key, value);
};

export const getLocalStorageItem = (key: string): string | null => (localStorage.getItem(key) || null);

