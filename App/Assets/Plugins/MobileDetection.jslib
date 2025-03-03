mergeInto(LibraryManager.library, {
  IsMobile: function () {
    var mobileRegex =
      /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i;
    return mobileRegex.test(navigator.userAgent);
  },
});
