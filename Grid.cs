<<<<<<< HEAD
﻿using System.Text;

using Silk.NET.Maths;

namespace RLLib;

public class Grid
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    internal GridCell[,] Cells;

    public Grid(int width, int height)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than zero.");
        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be greater than zero.");

        Width = width;
        Height = height;
        Cells = new GridCell[Width, height];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Cells[x, y] = new GridCell(0, Color.ANSI.White, Color.ANSI.Black, false, false);
            }
        }
    }

    #region Clear
    public void Clear() => Fill(0, Color.ANSI.White, Color.ANSI.Black, false, false);
    #endregion

    #region Fill
    public void Fill(int? displayCharacter = null, Color? foreground = null, Color? background = null, bool? flipHorizontally = null, bool? flipVertically = null) => Fill(0, 0, Width, Height, displayCharacter, foreground, background, flipHorizontally, flipVertically);
    public void Fill(int startX, int startY, int width, int height, int? displayCharacter = null, Color? foreground = null, Color? background = null, bool? flipHorizontally = null, bool? flipVertically = null)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Set(startX + x, startY + y, displayCharacter, foreground, background, flipHorizontally, flipVertically);
            }
        }
    }

    public void Fill(Vector2D<int> character, Color? foreground = null, Color? background = null, bool? flipHorizontally = null, bool? flipVertically = null) => Fill(0, 0, Width, Height, character.X, character.Y, foreground, background, flipHorizontally, flipVertically);
    public void Fill(int displayX, int displayY, Color? foreground = null, Color? background = null, bool? flipHorizontally = null, bool? flipVertically = null) => Fill(0, 0, Width, Height, displayX, displayY, foreground, background, flipHorizontally, flipVertically);
    public void Fill(int startX, int startY, int width, int height, Vector2D<int> character, Color? foreground = null, Color? background = null, bool? flipHorizontally = null, bool? flipVertically = null) => Fill(startX, startY, width, height, character.X, character.Y, foreground, background, flipHorizontally, flipVertically);
    public void Fill(int startX, int startY, int width, int height, int displayX, int displayY, Color? foreground = null, Color? background = null, bool? flipHorizontally = null, bool? flipVertically = null)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Set(startX + x, startY + y, displayX, displayY, foreground, background, flipHorizontally, flipVertically);
            }
        }
    }

    internal void Fill(GridCell[,] cells) => Fill(0, 0, cells);
    internal void Fill(int startX, int startY, GridCell[,] cells)
    {
        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                Set(startX + x, startY + y, cells[x, y]);
            }
        }
    }
    #endregion

    #region Set
    public void Set(int positionX, int positionY, int? displayCharacter = null, Color? foreground = null, Color? background = null, bool? flipHorizontally = null, bool? flipVertically = null)
    {
        if (!IsValidXY(positionX, positionY))
        {
            return;
        }

        if (displayCharacter.HasValue)
        {
            Cells[positionX, positionY].SetCharacter(displayCharacter.Value);
        }
        if (foreground.HasValue)
        {
            Cells[positionX, positionY].SetForegroundColor(foreground.Value);
        }
        if (background.HasValue)
        {
            Cells[positionX, positionY].SetBackgroundColor(background.Value);
        }

        Cells[positionX, positionY].SetFlipped(flipHorizontally, flipVertically);
    }
    public void Set(int positionX, int positionY, Vector2D<int> character, Color? foreground = null, Color? background = null, bool? flipHorizontally = null, bool? flipVertically = null) => Set(positionX, positionY, character.X, character.Y, foreground, background, flipHorizontally, flipVertically);
    public void Set(int positionX, int positionY, int? displayX, int? displayY, Color? foreground = null, Color? background = null, bool? flipHorizontally = null, bool? flipVertically = null)
    {
        if (!IsValidXY(positionX, positionY))
        {
            return;
        }

        if (displayX.HasValue &&
            displayY.HasValue)
        {
            Cells[positionX, positionY].SetXY(displayX.Value, displayY.Value);
        }
        if (foreground.HasValue)
        {
            Cells[positionX, positionY].SetForegroundColor(foreground.Value);
        }
        if (background.HasValue)
        {
            Cells[positionX, positionY].SetBackgroundColor(background.Value);
        }

        Cells[positionX, positionY].SetFlipped(flipHorizontally, flipVertically);
    }
    internal void Set(int positionX, int positionY, GridCell cell)
    {
        if (!IsValidXY(positionX, positionY))
        {
            return;
        }

        Cells[positionX, positionY] = cell;
    }
    #endregion

    #region Print
    public void Print(int positionX, int positionY, string text, Color? foreground = null, Color? background = null)
    {
        for (int i = 0; i < text.Length; i++)
        {
            Set(positionX + i, positionY, text[i], foreground, background, false, false);
        }
    }

    public int Print(int positionX, int positionY, string text, Color? foreground = null, Color? background = null, int wrap = 10, int maxLines = -1)
    {
        if (wrap <= 0)
            throw new ArgumentOutOfRangeException(nameof(wrap), "Wrap must be greater than 0.");
        StringBuilder sb = new StringBuilder(wrap);
        string[] words = text.Split(' ');
        int i = 0;
        int lines = 0;

        while (i < words.Length && (maxLines == -1 || lines <= maxLines))
        {
            while (i < words.Length && sb.Length + words[i].Length + 1 < wrap)
            {
                sb.Append(words[i++] + " ");
            }

            string line = sb.ToString();
            sb.Clear();

            for (int j = 0; j < line.Length; j++)
            {
                Set(positionX + j, positionY + lines, line[j], foreground, background, false, false);
            }
            lines++;
        }
        return lines;
    }

    public void Print(int positionX, int positionY, ColoredString text)
    {
        int i = 0;
        foreach (var gridCell in text.Glyphs)
        {
            Set(positionX + i, positionY, gridCell);
            i++;
        }
    }
    #endregion

    public static void Blit(Grid sourceGrid, Grid destinationGrid, int width, int height) => Blit(sourceGrid, 0, 0, destinationGrid, 0, 0, width, height);
    public static void Blit(Grid sourceGrid, int sourceX, int sourceY, Grid destinationGrid, int width, int height) => Blit(sourceGrid, sourceX, sourceY, destinationGrid, 0, 0, width, height);
    public static void Blit(Grid sourceGrid, Grid destinationGrid, int destinationX, int destinationY, int width, int height) => Blit(sourceGrid, 0, 0, destinationGrid, destinationX, destinationY, width, height);
    public static void Blit(Grid sourceGrid, int sourceX, int sourceY, Grid destinationGrid, int destinationX, int destinationY, int width, int height)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than 0.");
        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be greater than 0.");

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Check before reading
                // Writing will be checked in destionationGrid.Set()
                if (sourceGrid.IsValidXY(sourceX + x, sourceY + y))
                {
                    destinationGrid.Set(destinationX + x, destinationY + y, sourceGrid[sourceX + x, sourceY + y]);
                }
            }
        }
    }

    public bool IsValidXY(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;

    internal GridCell this[int x, int y] => Cells[x, y];
=======
﻿using System.Text;

using Silk.NET.Maths;

namespace RLLib;

public class Grid
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    internal GridCell[,] Cells;

    public Grid(int width, int height)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than zero.");
        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be greater than zero.");

        Width = width;
        Height = height;
        Cells = new GridCell[Width, height];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Cells[x, y] = new GridCell(0, Color.ANSI.White, Color.ANSI.Black, false, false);
            }
        }
    }

    #region Clear
    public void Clear() => Fill(0, Color.ANSI.White, Color.ANSI.Black, false, false);
    #endregion

    #region Fill
    public void Fill(int? displayCharacter = null, Color? foreground = null, Color? background = null, bool? flipHorizontally = null, bool? flipVertically = null) => Fill(0, 0, Width, Height, displayCharacter, foreground, background, flipHorizontally, flipVertically);
    public void Fill(int startX, int startY, int width, int height, int? displayCharacter = null, Color? foreground = null, Color? background = null, bool? flipHorizontally = null, bool? flipVertically = null)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Set(startX + x, startY + y, displayCharacter, foreground, background, flipHorizontally, flipVertically);
            }
        }
    }

    public void Fill(Vector2D<int> character, Color? foreground = null, Color? background = null, bool? flipHorizontally = null, bool? flipVertically = null) => Fill(0, 0, Width, Height, character.X, character.Y, foreground, background, flipHorizontally, flipVertically);
    public void Fill(int displayX, int displayY, Color? foreground = null, Color? background = null, bool? flipHorizontally = null, bool? flipVertically = null) => Fill(0, 0, Width, Height, displayX, displayY, foreground, background, flipHorizontally, flipVertically);
    public void Fill(int startX, int startY, int width, int height, Vector2D<int> character, Color? foreground = null, Color? background = null, bool? flipHorizontally = null, bool? flipVertically = null) => Fill(startX, startY, width, height, character.X, character.Y, foreground, background, flipHorizontally, flipVertically);
    public void Fill(int startX, int startY, int width, int height, int displayX, int displayY, Color? foreground = null, Color? background = null, bool? flipHorizontally = null, bool? flipVertically = null)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Set(startX + x, startY + y, displayX, displayY, foreground, background, flipHorizontally, flipVertically);
            }
        }
    }

    internal void Fill(GridCell[,] cells) => Fill(0, 0, cells);
    internal void Fill(int startX, int startY, GridCell[,] cells)
    {
        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                Set(startX + x, startY + y, cells[x, y]);
            }
        }
    }
    #endregion

    #region Set
    public void Set(int positionX, int positionY, int? displayCharacter = null, Color? foreground = null, Color? background = null, bool? flipHorizontally = null, bool? flipVertically = null)
    {
        if (!IsValidXY(positionX, positionY))
        {
            return;
        }

        if (displayCharacter.HasValue)
        {
            Cells[positionX, positionY].SetCharacter(displayCharacter.Value);
        }
        if (foreground.HasValue)
        {
            Cells[positionX, positionY].SetForegroundColor(foreground.Value);
        }
        if (background.HasValue)
        {
            Cells[positionX, positionY].SetBackgroundColor(background.Value);
        }

        Cells[positionX, positionY].SetFlipped(flipHorizontally, flipVertically);
    }
    public void Set(int positionX, int positionY, Vector2D<int> character, Color? foreground = null, Color? background = null, bool? flipHorizontally = null, bool? flipVertically = null) => Set(positionX, positionY, character.X, character.Y, foreground, background, flipHorizontally, flipVertically);
    public void Set(int positionX, int positionY, int? displayX, int? displayY, Color? foreground = null, Color? background = null, bool? flipHorizontally = null, bool? flipVertically = null)
    {
        if (!IsValidXY(positionX, positionY))
        {
            return;
        }

        if (displayX.HasValue &&
            displayY.HasValue)
        {
            Cells[positionX, positionY].SetXY(displayX.Value, displayY.Value);
        }
        if (foreground.HasValue)
        {
            Cells[positionX, positionY].SetForegroundColor(foreground.Value);
        }
        if (background.HasValue)
        {
            Cells[positionX, positionY].SetBackgroundColor(background.Value);
        }

        Cells[positionX, positionY].SetFlipped(flipHorizontally, flipVertically);
    }
    internal void Set(int positionX, int positionY, GridCell cell)
    {
        if (!IsValidXY(positionX, positionY))
        {
            return;
        }

        Cells[positionX, positionY] = cell;
    }
    #endregion

    #region Print
    public void Print(int positionX, int positionY, string text, Color? foreground = null, Color? background = null)
    {
        for (int i = 0; i < text.Length; i++)
        {
            Set(positionX + i, positionY, text[i], foreground, background, false, false);
        }
    }

    public int Print(int positionX, int positionY, string text, Color? foreground = null, Color? background = null, int wrap = 10, int maxLines = -1)
    {
        if (wrap <= 0)
            throw new ArgumentOutOfRangeException(nameof(wrap), "Wrap must be greater than 0.");
        StringBuilder sb = new StringBuilder(wrap);
        string[] words = text.Split(' ');
        int i = 0;
        int lines = 0;

        while (i < words.Length && (maxLines == -1 || lines <= maxLines))
        {
            while (i < words.Length && sb.Length + words[i].Length + 1 < wrap)
            {
                sb.Append(words[i++] + " ");
            }

            string line = sb.ToString();
            sb.Clear();

            for (int j = 0; j < line.Length; j++)
            {
                Set(positionX + j, positionY + lines, line[j], foreground, background, false, false);
            }
            lines++;
        }
        return lines;
    }

    public void Print(int positionX, int positionY, ColoredString text)
    {
        int i = 0;
        foreach (var gridCell in text.Glyphs)
        {
            Set(positionX + i, positionY, gridCell);
            i++;
        }
    }
    #endregion

    public static void Blit(Grid sourceGrid, Grid destinationGrid, int width, int height) => Blit(sourceGrid, 0, 0, destinationGrid, 0, 0, width, height);
    public static void Blit(Grid sourceGrid, int sourceX, int sourceY, Grid destinationGrid, int width, int height) => Blit(sourceGrid, sourceX, sourceY, destinationGrid, 0, 0, width, height);
    public static void Blit(Grid sourceGrid, Grid destinationGrid, int destinationX, int destinationY, int width, int height) => Blit(sourceGrid, 0, 0, destinationGrid, destinationX, destinationY, width, height);
    public static void Blit(Grid sourceGrid, int sourceX, int sourceY, Grid destinationGrid, int destinationX, int destinationY, int width, int height)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than 0.");
        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be greater than 0.");

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Check before reading
                // Writing will be checked in destionationGrid.Set()
                if (sourceGrid.IsValidXY(sourceX + x, sourceY + y))
                {
                    destinationGrid.Set(destinationX + x, destinationY + y, sourceGrid[sourceX + x, sourceY + y]);
                }
            }
        }
    }

    public bool IsValidXY(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;

    internal GridCell this[int x, int y] => Cells[x, y];
>>>>>>> main
}