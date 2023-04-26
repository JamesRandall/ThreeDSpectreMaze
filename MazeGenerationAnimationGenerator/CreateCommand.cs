using System.Collections.Immutable;
using System.CommandLine;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using ThreeDSpectreMaze.Algorithms;

namespace ThreeDSpectreMaze;

public static class CreateCommand
{
    public static Command Create()
    {
        var algorithmOption = new Option<string>(
            name: "--algorithm",
            description: "The algorithm to use to generate the maze: recursivebacktracker or huntandkill",
            getDefaultValue: () => "recursivebacktracker"
        );
        var outputOption = new Option<string>(
            name: "--output",
            description: "The name of the file to output to",
            getDefaultValue: () => "."
        );
        var imageWidthOption = new Option<int>(
            name: "--image-width",
            description: "The width of the images",
            getDefaultValue: () => 400
        );
        var imageHeightOption = new Option<int>(
            name: "--image-height",
            description: "The height of the images",
            getDefaultValue: () => 400
        );
        var mazeWidthOption = new Option<int>(
            name: "--maze-width",
            description: "The height of the maze",
            getDefaultValue: () => 6
        );
        var mazeHeightOption = new Option<int>(
            name: "--maze-height",
            description: "The height of the maze",
            getDefaultValue: () => 6
        );
        var frameTimeOption = new Option<int>(
            name: "--frame-time",
            description: "The time between frames in milliseconds",
            getDefaultValue: () => 15
        );
        
        var createCommand = new Command("--create", "Creates a maze");
        createCommand.AddOption(algorithmOption);
        createCommand.AddOption(outputOption);
        createCommand.AddOption(imageWidthOption);
        createCommand.AddOption(imageHeightOption);
        createCommand.AddOption(mazeWidthOption);
        createCommand.AddOption(mazeHeightOption);
        createCommand.AddOption(frameTimeOption);
        createCommand.SetHandler(
            Handler,
            algorithmOption,
            outputOption,
            imageWidthOption,
            imageHeightOption,
            mazeWidthOption,
            mazeHeightOption,
            frameTimeOption
        );
        return createCommand;
    }

    private static void Handler(
        string algorithmName,
        string outputFile,
        int imageWidth,
        int imageHeight,
        int mazeWidth,
        int mazeHeight,
        int frameTime)
    {
        var algorithm = algorithmName.ToLower() switch
        {
            "recursivebacktracker" => (Func<int, int, Action<Direction[,], ImmutableList<MapVector>>?, Direction[,]>)RecursiveBackTracking.Algorithm,
            "huntandkill" => (Func<int, int, Action<Direction[,], ImmutableList<MapVector>>?, Direction[,]>)HuntAndKill.Algorithm,
            _ => throw new NotSupportedException("Algorithm must be recursivebacktracker or huntandkill")
        };

        var gif = new Image<Rgba32>(imageWidth, imageHeight);
        var metadata = gif.Frames.RootFrame.Metadata.GetGifMetadata();
        metadata.FrameDelay = frameTime;
        var frameNumber = 0;
        algorithm(
            mazeWidth,
            mazeHeight,
            (map, path) =>
            {
                using var frameImage = MazeImageGenerator.GenerateImage(
                    map,
                    path,
                    imageWidth,
                    imageHeight
                );
                if (frameNumber == 0)
                {
                    gif.Mutate(x => x.DrawImage(frameImage, 1));
                }
                else
                {
                    metadata = frameImage.Frames.RootFrame.Metadata.GetGifMetadata();
                    metadata.FrameDelay = frameTime;
                    gif.Frames.AddFrame(frameImage.Frames.RootFrame);
                }
                frameNumber++;
                if (frameNumber % 100 == 0) Console.WriteLine($"Frames completed: {frameNumber}");
            }
        );
        // while you can't avoid some artifacting the below seems to result in the best output
        // on large mazes
        var palette = new ReadOnlyMemory<Color>(MazeImageGenerator.Palette.ToArray());
        var encoder =new GifEncoder()
        {
            Quantizer = new PaletteQuantizer(palette),
        };
        gif.SaveAsGif(outputFile, encoder);
    }
}