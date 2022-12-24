mergeInto(LibraryManager.library, {

  WriteCurrentGame: function (str) {
    localStorage.setItem('currentGame', str);
    Console.write('writing current game: ' + str);
  },

  ReadCurrentGame: function () {
  	const str = localStorage.getItem('currentGame');
    Console.write('reading current game: ' + str);
    return str;
  },

  WriteHistoricalData: function (str) {
    localStorage.setItem('historicalData', str);
  },

  ReadHistoricalData: function () {
    return localStorage.getItem('historicalData');
  }
});