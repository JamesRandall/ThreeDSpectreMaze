using Spectre.Console;

namespace ThreeDSpectreMaze;

public class RetroFont
{
    #region Character models
    private static byte[][] Layouts = {
        new byte[]
        {
            0b01110,
            0b10001,
            0b10011,
            0b10101,
            0b11001,
            0b10001,
            0b01110
        },
        new byte[]
        {
            0b00100,
            0b01100,
            0b00100,
            0b00100,
            0b00100,
            0b00100,
            0b01110
        },
        new byte[]
        {
            0b01110,
            0b10001,
            0b00001,
            0b00110,
            0b01000,
            0b10000,
            0b11111
        },
        new byte[]
        {
            0b01110,
            0b10001,
            0b00001,
            0b00110,
            0b00001,
            0b10001,
            0b01110
        },
        new byte[]
        {
            0b00011,
            0b00101,
            0b01001,
            0b10001,
            0b11111,
            0b00001,
            0b00001
        },
        new byte[]
        {
            0b11111,
            0b10000,
            0b11110,
            0b00001,
            0b00001,
            0b10001,
            0b01110
        },
        new byte[]
        {
            0b01110,
            0b10001,
            0b10000,
            0b11110,
            0b10001,
            0b10001,
            0b01110
        },
        new byte[]
        {
            0b11111,
            0b00001,
            0b00010,
            0b00100,
            0b01000,
            0b01000,
            0b01000
        },
        new byte[]
        {
            0b01110,
            0b10001,
            0b10001,
            0b01110,
            0b10001,
            0b10001,
            0b01110
        },
        new byte[] // 9
        {
            0b01110,
            0b10001,
            0b10001,
            0b01111,
            0b00001,
            0b10001,
            0b01110
        },
        new byte[] // :
        {
            0b00100,
            0b00000,
            0b00000,
            0b00000,
            0b00000,
            0b00100,
            0b00000
        },
        new byte[] // ;
        {
            0b00100,
            0b00000,
            0b00000,
            0b00000,
            0b00000,
            0b00100,
            0b01000
        },
        new byte[] // <
        {
            0b00000,
            0b00100,
            0b01000,
            0b10000,
            0b01000,
            0b00100,
            0b00000
        },
        new byte[] // =
        {
            0b00000,
            0b00000,
            0b01110,
            0b00000,
            0b01110,
            0b00000,
            0b00000
        },
        new byte[] // >
        {
            0b00000,
            0b00100,
            0b00010,
            0b00001,
            0b00010,
            0b00100,
            0b00000
        },
        new byte[] // ?
        {
            0b01110,
            0b10001,
            0b00001,
            0b00010,
            0b00100,
            0b00000,
            0b00100
        },
        new byte[] // @
        {
            0b01110,
            0b10001,
            0b10011,
            0b10101,
            0b10011,
            0b10000,
            0b01111
        },
        new byte[] // A
        {
            0b01111,
            0b10001,
            0b10001,
            0b11111,
            0b10001,
            0b10001,
            0b10001,
        },
        new byte[] // B
        {
            0b11110,
            0b10001,
            0b10001,
            0b11110,
            0b10001,
            0b10001,
            0b11110
        },
        new byte[] // C
        {
            0b01110,
            0b10001,
            0b10000,
            0b10000,
            0b10000,
            0b10001,
            0b01110
        },
        new byte[] // D
        {
            0b11110,
            0b10001,
            0b10001,
            0b10001,
            0b10001,
            0b10001,
            0b11110
        },
        new byte[] // E
        {
            0b11111,
            0b10000,
            0b10000,
            0b11110,
            0b10000,
            0b10000,
            0b11111
        },
        new byte[] // F
        {
            0b11111,
            0b10000,
            0b10000,
            0b11110,
            0b10000,
            0b10000,
            0b10000
        },
        new byte[] // G
        {
            0b01110,
            0b10001,
            0b10000,
            0b10000,
            0b10011,
            0b10001,
            0b01110
        },
        new byte[] // H
        {
            0b10001,
            0b10001,
            0b10001,
            0b11111,
            0b10001,
            0b10001,
            0b10001
        },
        new byte[] // I
        {
            0b01110,
            0b00100,
            0b00100,
            0b00100,
            0b00100,
            0b00100,
            0b01110
        },
        
        new byte[] // J
        {
            0b00011,
            0b00001,
            0b00001,
            0b00001,
            0b00001,
            0b10001,
            0b01110
        },
        new byte[] // K
        {
            0b10001,
            0b10010,
            0b10100,
            0b11000,
            0b10100,
            0b10010,
            0b10001
        },
        new byte[] // L
        {
            0b10000,
            0b10000,
            0b10000,
            0b10000,
            0b10000,
            0b10000,
            0b11111
        },
        new byte[] // M
        {
            0b10001,
            0b11011,
            0b10101,
            0b10101,
            0b10001,
            0b10001,
            0b10001
        },
        new byte[] // N
        {
            0b10001,
            0b10001,
            0b11001,
            0b10101,
            0b10011,
            0b10001,
            0b10001
        },
        new byte[] // O
        {
            0b01110,
            0b10001,
            0b10001,
            0b10001,
            0b10001,
            0b10001,
            0b01110
        },
        new byte[] // P
        {
            0b11110,
            0b10001,
            0b10001,
            0b11110,
            0b10000,
            0b10000,
            0b10000
        },
        new byte[] // Q
        {
            0b01110,
            0b10001,
            0b10001,
            0b10001,
            0b10101,
            0b10010,
            0b01101
        },
        new byte[] // R
        {
            0b11110,
            0b10001,
            0b10001,
            0b11110,
            0b10001,
            0b10001,
            0b10001,
        },
        new byte[] // S
        {
            0b01110,
            0b10001,
            0b10000,
            0b01110,
            0b00001,
            0b10001,
            0b01110
        },
        new byte[] // T
        {
            0b11111,
            0b00100,
            0b00100,
            0b00100,
            0b00100,
            0b00100,
            0b00100
        },
        new byte[] // U
        {
            0b10001,
            0b10001,
            0b10001,
            0b10001,
            0b10001,
            0b10001,
            0b01110
        },
        new byte[] // V
        {
            0b10001,
            0b10001,
            0b10001,
            0b10001,
            0b10001,
            0b01010,
            0b00100
        },
        new byte[] // W
        {
            0b10001,
            0b10001,
            0b10001,
            0b10001,
            0b10101,
            0b10101,
            0b01010
        },
        new byte[] // X
        {
            0b10001,
            0b10001,
            0b01010,
            0b00100,
            0b01010,
            0b10001,
            0b10001
        },
        new byte[] // Y
        {
            0b10001,
            0b10001,
            0b01010,
            0b00100,
            0b00100,
            0b00100,
            0b00100
        },
        new byte[] // Z
        {
            0b11111,
            0b00001,
            0b00010,
            0b00100,
            0b01000,
            0b10000,
            0b11111
        }
    };
    #endregion

    public const int Width = 5;
    public const int Height = 7;
    
    public enum Alignment { Left, Right, Center }

    public static void Render(Canvas canvas, Color color, MapVector position, string value, Alignment alignment=Alignment.Left)
    {
        var currentPosition =
            alignment switch
            {
                Alignment.Left => position,
                Alignment.Right => position with
                {
                    x = position.x -
                        (value.Length-1) - // spaces between numbers
                        value.Length * Width // numbers
                },
                Alignment.Center => position with
                {
                    x = position.x -
                        (value.Length-1) / 2 - // spaces between numbers
                        value.Length * Width / 2 // numbers
                },
                _ => throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null)
            };
        foreach (var character in value.ToUpper())
        {
            if ((character >= '0' && character <= 'Z') || character == ' ')
            {
                if (character != ' ')
                {
                    var index = character - '0';
                    for (var y = 0; y < Layouts[index].Length; y++)
                    {
                        var row = Layouts[index][y];
                        for (var x = 0; x < Width; x++)
                        {
                            if ((row & (1 << x)) != 0)
                            {
                                canvas.SetPixel(currentPosition.x + (Width - 1 - x), currentPosition.y + y, color);
                            }
                        }
                    }
                }
                currentPosition =  currentPosition with {x = currentPosition.x + (Width + 1)};
            }
        }
    }
}