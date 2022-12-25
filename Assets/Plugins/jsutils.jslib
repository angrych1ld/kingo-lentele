mergeInto(LibraryManager.library, {

  WriteCurrentGame: function (str) {
    localStorage.setItem('currentGame', UTF8ToString(str));
  },

  ReadCurrentGame: function () {
    var returnStr = localStorage.getItem('currentGame');

    if (!returnStr) {
        returnStr = '';
    }

    var bufferSize = lengthBytesUTF8(returnStr);
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

  WriteHistoricalData: function (str) {
    localStorage.setItem('historicalData', UTF8ToString(str));
  },

  ReadHistoricalData: function () {
    var returnStr = localStorage.getItem('historicalData');

    if (!returnStr) {
        returnStr = '';
    }

    var bufferSize = lengthBytesUTF8(returnStr);
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  }
});