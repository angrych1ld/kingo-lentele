mergeInto(LibraryManager.library, {

  WriteCurrentGame: function (str) {
  console.log('js write: ' + str);
    localStorage.setItem('currentGame', str);
  },

  ReadCurrentGame: function () {
  	const str = localStorage.getItem('currentGame');
    return str;
  },

  WriteHistoricalData: function (str) {
    localStorage.setItem('historicalData', str);
  },

  ReadHistoricalData: function () {
    return localStorage.getItem('historicalData');
  }
});