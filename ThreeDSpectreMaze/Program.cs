using ThreeDSpectreMaze;
using ThreeDSpectreMaze.Algorithms;

var map = MapFactory.Create(RecursiveBackTracking.Algorithm);
var game = new Game(map);
game.Run();
