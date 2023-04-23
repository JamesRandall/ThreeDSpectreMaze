using ThreeDSpectreMaze;
using ThreeDSpectreMaze.Algorithms;

var map =
    args.Length == 1 && args[0].ToLower() == "--observe"
        ? ConsoleObserver.ObserveCreation(RecursiveBackTracking.Algorithm)
        : MapFactory.Create(RecursiveBackTracking.Algorithm);

var game = new Game(map);
game.Run();
