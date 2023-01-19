export const setLocalStorageItem = (key, value) => {
    localStorage.setItem(key, value);
};
export const getLocalStorageItem = (key) => (localStorage.getItem(key) || null);
