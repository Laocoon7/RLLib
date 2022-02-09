<<<<<<< HEAD
﻿namespace RLLib;

public class ColoredStringBuilder
{
    private BuilderSettings Settings;
    private Color InitialForeground;
    private Color InitialBackground;

    public ColoredStringBuilder() : this(Color.ANSI.White, Color.ANSI.Black) { }
    public ColoredStringBuilder(Color initialForeground, Color initialBackground)
    {
        InitialForeground = initialForeground;
        InitialBackground = initialBackground;
        Settings = new BuilderSettings(InitialForeground, InitialBackground);
    }

    public ColoredString Build() => new ColoredString(Settings);

    #region Builder
    public ColoredStringBuilder SetForegroundColor(Color color)
    {
        Settings.CurrentForeground = color;
        return this;
    }

    public ColoredStringBuilder SetBackgroundColor(Color color)
    {
        Settings.CurrentBackground = color;
        return this;
    }

    public ColoredStringBuilder SetFlippedHorizontally(bool flip = true)
    {
        Settings.FlipHorizontally = flip;
        return this;
    }

    public ColoredStringBuilder SetFlippedVertically(bool flip = true)
    {
        Settings.FlipVertically = flip;
        return this;
    }

    public ColoredStringBuilder Add(string text)
    {
        foreach (var character in text)
        {
            Settings.Glyphs.Add(new GridCell(character, Settings.CurrentForeground, Settings.CurrentBackground, Settings.FlipHorizontally, Settings.FlipVertically));
        }
        return this;
    }
    #endregion

    public ColoredStringBuilder Reset() => Reset(InitialForeground, InitialBackground);
    public ColoredStringBuilder Reset(Color foreground, Color background)
    {
        Settings.Reset(foreground, background);
        return this;
    }

    internal class BuilderSettings
    {
        public Color CurrentForeground;
        public Color CurrentBackground;
        public bool FlipHorizontally;
        public bool FlipVertically;

        public List<GridCell> Glyphs;

        public BuilderSettings(Color currentForeground, Color currentBackground)
        {
            CurrentForeground = currentForeground;
            CurrentBackground = currentBackground;
            Glyphs = new List<GridCell>();
        }

        public void Reset(Color currentForeground, Color currentBackground)
        {
            CurrentForeground = currentForeground;
            CurrentBackground = currentBackground;

            Glyphs.Clear();
        }
    }
}

public class ColoredString
{
    internal List<GridCell> Glyphs;

    internal ColoredString(ColoredStringBuilder.BuilderSettings settings)
    {
        Glyphs = new List<GridCell>(settings.Glyphs);
    }
=======
﻿namespace RLLib;

public class ColoredStringBuilder
{
    private BuilderSettings Settings;
    private Color InitialForeground;
    private Color InitialBackground;

    public ColoredStringBuilder() : this(Color.ANSI.White, Color.ANSI.Black) { }
    public ColoredStringBuilder(Color initialForeground, Color initialBackground)
    {
        InitialForeground = initialForeground;
        InitialBackground = initialBackground;
        Settings = new BuilderSettings(InitialForeground, InitialBackground);
    }

    public ColoredString Build() => new ColoredString(Settings);

    #region Builder
    public ColoredStringBuilder SetForegroundColor(Color color)
    {
        Settings.CurrentForeground = color;
        return this;
    }

    public ColoredStringBuilder SetBackgroundColor(Color color)
    {
        Settings.CurrentBackground = color;
        return this;
    }

    public ColoredStringBuilder SetFlippedHorizontally(bool flip = true)
    {
        Settings.FlipHorizontally = flip;
        return this;
    }

    public ColoredStringBuilder SetFlippedVertically(bool flip = true)
    {
        Settings.FlipVertically = flip;
        return this;
    }

    public ColoredStringBuilder Add(string text)
    {
        foreach (var character in text)
        {
            Settings.Glyphs.Add(new GridCell(character, Settings.CurrentForeground, Settings.CurrentBackground, Settings.FlipHorizontally, Settings.FlipVertically));
        }
        return this;
    }
    #endregion

    public ColoredStringBuilder Reset() => Reset(InitialForeground, InitialBackground);
    public ColoredStringBuilder Reset(Color foreground, Color background)
    {
        Settings.Reset(foreground, background);
        return this;
    }

    internal class BuilderSettings
    {
        public Color CurrentForeground;
        public Color CurrentBackground;
        public bool FlipHorizontally;
        public bool FlipVertically;

        public List<GridCell> Glyphs;

        public BuilderSettings(Color currentForeground, Color currentBackground)
        {
            CurrentForeground = currentForeground;
            CurrentBackground = currentBackground;
            Glyphs = new List<GridCell>();
        }

        public void Reset(Color currentForeground, Color currentBackground)
        {
            CurrentForeground = currentForeground;
            CurrentBackground = currentBackground;

            Glyphs.Clear();
        }
    }
}

public class ColoredString
{
    internal List<GridCell> Glyphs;

    internal ColoredString(ColoredStringBuilder.BuilderSettings settings)
    {
        Glyphs = new List<GridCell>(settings.Glyphs);
    }
>>>>>>> main
}