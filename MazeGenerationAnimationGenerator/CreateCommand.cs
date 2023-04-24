using System.Collections.Immutable;
using System.CommandLine;
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
            name: "---image-height",
            description: "The height of the images",
            getDefaultValue: () => 400
        );
        var mazeWidthOption = new Option<int>(
            name: "---maze-width",
            description: "The height of the maze",
            getDefaultValue: () => 6
        );
        var mazeHeightOption = new Option<int>(
            name: "---maze-height",
            description: "The height of the maze",
            getDefaultValue: () => 6
        );
        
        var createCommand = new Command("--create", "Creates a maze");
        createCommand.AddOption(algorithmOption);
        createCommand.AddOption(outputOption);
        createCommand.AddOption(imageWidthOption);
        createCommand.AddOption(imageHeightOption);
        createCommand.AddOption(mazeWidthOption);
        createCommand.AddOption(mazeHeightOption);
        createCommand.SetHandler(
            Handler,
            algorithmOption,
            outputOption,
            imageWidthOption,
            imageHeightOption,
            mazeWidthOption,
            mazeHeightOption
        );
        return createCommand;
    }

    private static void Handler(
        string algorithmName,
        string outputFile,
        int imageWidth,
        int imageHeight,
        int mazeWidth,
        int mazeHeight)
    {
        var algorithm = algorithmName.ToLower() switch
        {
            "recursivebacktracker" => (Func<int, int, Action<Direction[,], ImmutableList<MapVector>>?, Direction[,]>)RecursiveBackTracking.Algorithm,
            "huntandkill" => (Func<int, int, Action<Direction[,], ImmutableList<MapVector>>?, Direction[,]>)HuntAndKill.Algorithm,
            _ => throw new NotSupportedException("Algorithm must be recursivebacktracker or huntandkill")
        };
        
        const int frameDelay = 15;
        using var gif = new Image<Rgba32>(imageWidth, imageHeight);
        var metadata = gif.Frames.RootFrame.Metadata.GetGifMetadata();
        metadata.FrameDelay = frameDelay;
        var frameNumber = 0;
        algorithm(
            mazeWidth,
            mazeHeight,
            (map, path) =>
            {
                var frameImage = MazeImageGenerator.GenerateImage(
                    map,
                    path,
                    frameNumber,
                    imageWidth,
                    imageHeight,
                    outputFile
                );
                if (frameNumber == 0)
                {
                    gif.Mutate(x => x.DrawImage(frameImage, 1));
                }
                else
                {
                    metadata = frameImage.Frames.RootFrame.Metadata.GetGifMetadata();
                    metadata.FrameDelay = frameDelay;
                    gif.Frames.AddFrame(frameImage.Frames.RootFrame);
                }
                frameNumber++;
            }
        );
        gif.SaveAsGif(outputFile);
    }
}