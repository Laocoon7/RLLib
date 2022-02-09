<<<<<<< HEAD
﻿namespace RLLib;

internal struct GridCell
{
    internal DisplayType CellDisplayType;
    public int Character;
    public int X;
    public int Y;
    public Color Color;
    public Color BackColor;
    public Flip Flipped;

    public GridCell(int displayCharacter, Color color, Color backColor, bool? flipHorizontally = null, bool? flipVertically = null)
    {
        CellDisplayType = DisplayType.Character;
        Color = color;
        BackColor = backColor;
        Character = displayCharacter;
        X = 0;
        Y = 0;
        Flipped = Flip.None;

        if (flipHorizontally.HasValue &&
            flipHorizontally.Value)
        {
            Flipped |= Flip.Horizontally;
        }
        if (flipVertically.HasValue &&
            flipVertically.Value)
        {
            Flipped |= Flip.Vertically;
        }
    }

    public GridCell(int displayX, int displayY, Color color, Color backColor, bool? flipHorizontally = null, bool? flipVertically = null)
    {
        CellDisplayType = DisplayType.XY;
        Color = color;
        BackColor = backColor;
        Character = 0;
        X = displayX;
        Y = displayY;

        Flipped = Flip.None;

        if (flipHorizontally.HasValue &&
            flipHorizontally.Value)
        {
            Flipped |= Flip.Horizontally;
        }
        if (flipVertically.HasValue &&
            flipVertically.Value)
        {
            Flipped |= Flip.Vertically;
        }
    }

    public void SetCharacter(int displayCharacter)
    {
        CellDisplayType = DisplayType.Character;
        Character = displayCharacter;
    }

    public void FlipHorizontally(bool flip = true)
    {
        if (flip)
        {
            Flipped |= Flip.Horizontally;
        }
        else
        {
            Flipped &= ~Flip.Horizontally;
        }
    }

    public void FlipVertically(bool flip = true)
    {
        if (flip)
        {
            Flipped |= Flip.Vertically;
        }
        else
        {
            Flipped &= ~Flip.Vertically;
        }
    }

    public void SetFlipped(bool? flipHorizontally = null, bool? flipVertically = null)
    {
        if (flipHorizontally.HasValue)
        {
            FlipHorizontally(flipHorizontally.Value);
        }
        if (flipVertically.HasValue)
        {
            FlipVertically(flipVertically.Value);
        }
    }

    public void SetXY(int displayX, int displayY)
    {
        CellDisplayType = DisplayType.XY;
        X = displayX;
        Y = displayY;
    }

    public void SetForegroundColor(Color foreground)
    {
        Color = foreground;
    }

    public void SetBackgroundColor(Color background)
    {
        BackColor = background;
    }

    public override string ToString()
    {
        switch (CellDisplayType)
        {
            case DisplayType.XY:
                return $"X:{X}, Y:{Y}, F:{Color}, B:{BackColor}";
            default:
            case DisplayType.Character:
                return $"C:{Character}, F:{Color}, B:{BackColor}";
        }
    }

    [Flags]
    public enum Flip
    {
        None = 0,
        Horizontally = 1,
        Vertically = 2,
    }

    internal enum DisplayType
    {
        Character,
        XY,
    }
}
=======
﻿namespace RLLib;

internal struct GridCell
{
    internal DisplayType CellDisplayType;
    public int Character;
    public int X;
    public int Y;
    public Color Color;
    public Color BackColor;
    public Flip Flipped;

    public GridCell(int displayCharacter, Color color, Color backColor, bool? flipHorizontally = null, bool? flipVertically = null)
    {
        CellDisplayType = DisplayType.Character;
        Color = color;
        BackColor = backColor;
        Character = displayCharacter;
        X = 0;
        Y = 0;
        Flipped = Flip.None;

        if (flipHorizontally.HasValue &&
            flipHorizontally.Value)
        {
            Flipped |= Flip.Horizontally;
        }
        if (flipVertically.HasValue &&
            flipVertically.Value)
        {
            Flipped |= Flip.Vertically;
        }
    }

    public GridCell(int displayX, int displayY, Color color, Color backColor, bool? flipHorizontally = null, bool? flipVertically = null)
    {
        CellDisplayType = DisplayType.XY;
        Color = color;
        BackColor = backColor;
        Character = 0;
        X = displayX;
        Y = displayY;

        Flipped = Flip.None;

        if (flipHorizontally.HasValue &&
            flipHorizontally.Value)
        {
            Flipped |= Flip.Horizontally;
        }
        if (flipVertically.HasValue &&
            flipVertically.Value)
        {
            Flipped |= Flip.Vertically;
        }
    }

    public void SetCharacter(int displayCharacter)
    {
        CellDisplayType = DisplayType.Character;
        Character = displayCharacter;
    }

    public void FlipHorizontally(bool flip = true)
    {
        if (flip)
        {
            Flipped |= Flip.Horizontally;
        }
        else
        {
            Flipped &= ~Flip.Horizontally;
        }
    }

    public void FlipVertically(bool flip = true)
    {
        if (flip)
        {
            Flipped |= Flip.Vertically;
        }
        else
        {
            Flipped &= ~Flip.Vertically;
        }
    }

    public void SetFlipped(bool? flipHorizontally = null, bool? flipVertically = null)
    {
        if (flipHorizontally.HasValue)
        {
            FlipHorizontally(flipHorizontally.Value);
        }
        if (flipVertically.HasValue)
        {
            FlipVertically(flipVertically.Value);
        }
    }

    public void SetXY(int displayX, int displayY)
    {
        CellDisplayType = DisplayType.XY;
        X = displayX;
        Y = displayY;
    }

    public void SetForegroundColor(Color foreground)
    {
        Color = foreground;
    }

    public void SetBackgroundColor(Color background)
    {
        BackColor = background;
    }

    public override string ToString()
    {
        switch (CellDisplayType)
        {
            case DisplayType.XY:
                return $"X:{X}, Y:{Y}, F:{Color}, B:{BackColor}";
            default:
            case DisplayType.Character:
                return $"C:{Character}, F:{Color}, B:{BackColor}";
        }
    }

    [Flags]
    public enum Flip
    {
        None = 0,
        Horizontally = 1,
        Vertically = 2,
    }

    internal enum DisplayType
    {
        Character,
        XY,
    }
}
>>>>>>> main
