export const fileTypes = {
  Lp: 'LP',
  Tou: 'TOU',
};

export const getFileType = (file) => {
  if (file.name.startsWith(fileTypes.Lp)) {
    return fileTypes.Lp;
  }
  if (file.name.startsWith(fileTypes.Tou)) {
    return fileTypes.Tou;
  }
  return null;
}
