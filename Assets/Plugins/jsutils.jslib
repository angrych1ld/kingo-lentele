mergeInto(LibraryManager.library, {

  WriteCurrentGame: function (str) {
    localStorage.setItem('currentGame', str);
  },

  ReadCurrentGame: function () {
    return localStorage.getItem('currentGame');
  },

  WriteHistoricalData: function (str) {
    localStorage.setItem('historicalData', str);
  },

  ReadHistoricalData: function () {
    return localStorage.getItem('historicalData');
  }
});