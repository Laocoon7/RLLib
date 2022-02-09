<<<<<<< HEAD
﻿namespace RLLib;

public struct Color
{
    public float R;
    public float G;
    public float B;
    public float A;

    public Color() : this(0.0f, 0.0f, 0.0f, 1.0f) { }
    public Color(byte r, byte g, byte b) : this(r, g, b, 0xFF) { }
    public Color(byte r, byte g, byte b, byte a)
    {
        R = r / 255f;
        G = g / 255f;
        B = b / 255f;
        A = a / 255f;
    }
    public Color(float r, float g, float b) : this(r, g, b, 1.0f) { }
    public Color(float r, float g, float b, float a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public void Out(out float r, out float g, out float b, out float a)
    {
        r = R;
        g = G;
        b = B;
        a = A;
    }

    public void Out(out byte r, out byte g, out byte b, out byte a)
    {
        r = (byte)Math.Round(R * 255);
        g = (byte)Math.Round(G * 255);
        b = (byte)Math.Round(B * 255);
        a = (byte)Math.Round(A * 255);
    }

    public uint ToARGB()
    {
        Out(out byte r, out byte g, out byte b, out byte a);

        return (uint)(a << 24 | r << 16 | g << 8 | b);
    }

    public uint ToABGR()
    {
        Out(out byte r, out byte g, out byte b, out byte a);

        return (uint)(a << 24 | b << 16 | g << 8 | r);
    }

    public uint ToRGBA()
    {
        Out(out byte r, out byte g, out byte b, out byte a);

        return (uint)(r << 24 | g << 16 | b << 8 | a);
    }

    public uint ToBGRA()
    {
        Out(out byte r, out byte g, out byte b, out byte a);

        return (uint)(b << 24 | g << 16 | r << 8 | a);
    }

    public static Color FromARGB(uint value)
    {
        var a = (byte)(value >> 24);
        var r = (byte)(value >> 16);
        var g = (byte)(value >> 8);
        var b = (byte)(value);

        return new Color(r, g, b, a);
    }

    public static Color FromABGR(uint value)
    {
        var a = (byte)(value >> 24);
        var b = (byte)(value >> 16);
        var g = (byte)(value >> 8);
        var r = (byte)(value);

        return new Color(r, g, b, a);
    }

    public static Color FromRGBA(uint value)
    {
        var r = (byte)(value >> 24);
        var g = (byte)(value >> 16);
        var b = (byte)(value >> 8);
        var a = (byte)(value);

        return new Color(r, g, b, a);
    }

    public static Color FromBGRA(uint value)
    {
        var b = (byte)(value >> 24);
        var g = (byte)(value >> 16);
        var r = (byte)(value >> 8);
        var a = (byte)(value);

        return new Color(r, g, b, a);
    }


    /// <summary>
    /// Blends the two colors
    /// </summary>
    /// <param name="ColorA">Primary Color</param>
    /// <param name="ColorB">Secondary Color</param>
    /// <param name="Ratio">Ratio of the colors that are blended (255 - Full Primary, 0 - Full Secondary)</param>
    /// <returns>New Blended Color</returns>
    public static Color Blend(Color primary, Color secondary, byte ratio)
    {
        return Blend(primary, secondary, (float)ratio / 255f);
    }

    /// <summary>
    /// Evenly blends two colors
    /// </summary>
    /// <param name="color1"></param>
    /// <param name="color2"></param>
    /// <returns>New Blended Color</returns>
    public static Color Blend(Color color1, Color color2)
    {
        return Blend(color1, color2, .5f);
    }

    /// <summary>
    /// Blends the two colors
    /// </summary>
    /// <param name="ColorA">Primary Color</param>
    /// <param name="ColorB">Secondary Color</param>
    /// <param name="Ratio">Ratio of the colors that are blended (1f - Full Primary, 0f - Full Secondary)</param>
    /// <returns>New Blended Color</returns>
    public static Color Blend(Color primary, Color secondary, float ratio)
    {
        return secondary - ((secondary - primary) * ratio);
    }

    public static Color operator +(Color color, float f)
    {
        return new Color(color.R + f, color.G + f, color.B + f);
    }

    public static Color operator -(Color color, float f)
    {
        return new Color(color.R - f, color.G - f, color.B - f);
    }

    public static Color operator *(Color color, float f)
    {
        return new Color(color.R * f, color.G * f, color.B * f);
    }

    public static Color operator /(Color color, float f)
    {
        return new Color(color.R / f, color.G / f, color.B / f);
    }

    public static Color operator +(Color color, byte b)
    {
        float f = (byte)b / 255f;
        return new Color(color.R + f, color.G + f, color.B + f);
    }

    public static Color operator -(Color color, byte b)
    {
        float f = (byte)b / 255f;
        return new Color(color.R - f, color.G - f, color.B - f);
    }

    public static Color operator *(Color color, byte b)
    {
        float f = (byte)b / 255f;
        return new Color(color.R * f, color.G * f, color.B * f);
    }

    public static Color operator /(Color color, byte b)
    {
        float f = (byte)b / 255f;
        return new Color(color.R / f, color.G / f, color.B / f);
    }

    public static Color operator +(Color colorA, Color colorB)
    {
        return new Color(colorA.R + colorB.R, colorA.G + colorB.G, colorA.B + colorB.B);
    }

    public static Color operator -(Color colorA, Color colorB)
    {
        return new Color(colorA.R - colorB.R, colorA.G - colorB.G, colorA.B - colorB.B);
    }

    public static Color operator *(Color colorA, Color colorB)
    {
        return new Color(colorA.R * colorB.R, colorA.G * colorB.G, colorA.B * colorB.B);
    }

    public static Color operator /(Color colorA, Color colorB)
    {
        return new Color(colorA.R / colorB.R, colorA.G / colorB.G, colorA.B / colorB.B);
    }

    public override string ToString()
    {
        return string.Format($"R:{R}, G:{G}, B:{B}, A:{A}");
    }

    public static implicit operator System.Drawing.Color(Color c) => System.Drawing.Color.FromArgb((int)c.ToARGB());
    public static implicit operator Color(System.Drawing.Color c) => new Color(c.R, c.G, c.B, c.A);

    #region Predefined Colors

    private static System.Random RNG = new System.Random();

    public static Color Random => new Color((byte)RNG.Next(byte.MaxValue), (byte)RNG.Next(byte.MaxValue), (byte)RNG.Next(byte.MaxValue));
    public static Color RandomAlpha => new Color((byte)RNG.Next(byte.MaxValue), (byte)RNG.Next(byte.MaxValue), (byte)RNG.Next(byte.MaxValue), (byte)RNG.Next(byte.MaxValue));
    public static Color Black => new Color(0, 0, 0);
    public static Color Red => new Color(255, 0, 0);
    public static Color Green => new Color(0, 255, 0);
    public static Color Yellow => new Color(255, 255, 0);
    public static Color Blue => new Color(0, 0, 255);
    public static Color Magenta => new Color(255, 0, 255);
    public static Color Cyan => new Color(0, 255, 255);
    public static Color White => new Color(255, 255, 255);
    public static class ANSI
    {
        public static Color Black => new Color(0, 0, 0);
        public static Color Red => new Color(170, 0, 0);
        public static Color Green => new Color(0, 170, 0);
        public static Color Yellow => new Color(170, 85, 0);
        public static Color Blue => new Color(0, 0, 170);
        public static Color Magenta => new Color(170, 0, 170);
        public static Color Cyan => new Color(0, 170, 170);
        public static Color White => new Color(170, 170, 170);
        public static class BRIGHT
        {
            public static Color Black => new Color(85, 85, 85);
            public static Color Red => new Color(255, 85, 85);
            public static Color Green => new Color(85, 255, 85);
            public static Color Yellow => new Color(255, 255, 85);
            public static Color Blue => new Color(85, 85, 255);
            public static Color Magenta => new Color(255, 85, 255);
            public static Color Cyan => new Color(85, 255, 255);
            public static Color White => new Color(255, 255, 255);
        }
    }
    #endregion
}
=======
﻿namespace RLLib;

public struct Color
{
    public float R;
    public float G;
    public float B;
    public float A;

    public Color() : this(0.0f, 0.0f, 0.0f, 1.0f) { }
    public Color(byte r, byte g, byte b) : this(r, g, b, 0xFF) { }
    public Color(byte r, byte g, byte b, byte a)
    {
        R = r / 255f;
        G = g / 255f;
        B = b / 255f;
        A = a / 255f;
    }
    public Color(float r, float g, float b) : this(r, g, b, 1.0f) { }
    public Color(float r, float g, float b, float a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public void Out(out float r, out float g, out float b, out float a)
    {
        r = R;
        g = G;
        b = B;
        a = A;
    }

    public void Out(out byte r, out byte g, out byte b, out byte a)
    {
        r = (byte)Math.Round(R * 255);
        g = (byte)Math.Round(G * 255);
        b = (byte)Math.Round(B * 255);
        a = (byte)Math.Round(A * 255);
    }

    public uint ToARGB()
    {
        Out(out byte r, out byte g, out byte b, out byte a);

        return (uint)(a << 24 | r << 16 | g << 8 | b);
    }

    public uint ToABGR()
    {
        Out(out byte r, out byte g, out byte b, out byte a);

        return (uint)(a << 24 | b << 16 | g << 8 | r);
    }

    public uint ToRGBA()
    {
        Out(out byte r, out byte g, out byte b, out byte a);

        return (uint)(r << 24 | g << 16 | b << 8 | a);
    }

    public uint ToBGRA()
    {
        Out(out byte r, out byte g, out byte b, out byte a);

        return (uint)(b << 24 | g << 16 | r << 8 | a);
    }

    public static Color FromARGB(uint value)
    {
        var a = (byte)(value >> 24);
        var r = (byte)(value >> 16);
        var g = (byte)(value >> 8);
        var b = (byte)(value);

        return new Color(r, g, b, a);
    }

    public static Color FromABGR(uint value)
    {
        var a = (byte)(value >> 24);
        var b = (byte)(value >> 16);
        var g = (byte)(value >> 8);
        var r = (byte)(value);

        return new Color(r, g, b, a);
    }

    public static Color FromRGBA(uint value)
    {
        var r = (byte)(value >> 24);
        var g = (byte)(value >> 16);
        var b = (byte)(value >> 8);
        var a = (byte)(value);

        return new Color(r, g, b, a);
    }

    public static Color FromBGRA(uint value)
    {
        var b = (byte)(value >> 24);
        var g = (byte)(value >> 16);
        var r = (byte)(value >> 8);
        var a = (byte)(value);

        return new Color(r, g, b, a);
    }


    /// <summary>
    /// Blends the two colors
    /// </summary>
    /// <param name="ColorA">Primary Color</param>
    /// <param name="ColorB">Secondary Color</param>
    /// <param name="Ratio">Ratio of the colors that are blended (255 - Full Primary, 0 - Full Secondary)</param>
    /// <returns>New Blended Color</returns>
    public static Color Blend(Color primary, Color secondary, byte ratio)
    {
        return Blend(primary, secondary, (float)ratio / 255f);
    }

    /// <summary>
    /// Evenly blends two colors
    /// </summary>
    /// <param name="color1"></param>
    /// <param name="color2"></param>
    /// <returns>New Blended Color</returns>
    public static Color Blend(Color color1, Color color2)
    {
        return Blend(color1, color2, .5f);
    }

    /// <summary>
    /// Blends the two colors
    /// </summary>
    /// <param name="ColorA">Primary Color</param>
    /// <param name="ColorB">Secondary Color</param>
    /// <param name="Ratio">Ratio of the colors that are blended (1f - Full Primary, 0f - Full Secondary)</param>
    /// <returns>New Blended Color</returns>
    public static Color Blend(Color primary, Color secondary, float ratio)
    {
        return secondary - ((secondary - primary) * ratio);
    }

    public static Color operator +(Color color, float f)
    {
        return new Color(color.R + f, color.G + f, color.B + f);
    }

    public static Color operator -(Color color, float f)
    {
        return new Color(color.R - f, color.G - f, color.B - f);
    }

    public static Color operator *(Color color, float f)
    {
        return new Color(color.R * f, color.G * f, color.B * f);
    }

    public static Color operator /(Color color, float f)
    {
        return new Color(color.R / f, color.G / f, color.B / f);
    }

    public static Color operator +(Color color, byte b)
    {
        float f = (byte)b / 255f;
        return new Color(color.R + f, color.G + f, color.B + f);
    }

    public static Color operator -(Color color, byte b)
    {
        float f = (byte)b / 255f;
        return new Color(color.R - f, color.G - f, color.B - f);
    }

    public static Color operator *(Color color, byte b)
    {
        float f = (byte)b / 255f;
        return new Color(color.R * f, color.G * f, color.B * f);
    }

    public static Color operator /(Color color, byte b)
    {
        float f = (byte)b / 255f;
        return new Color(color.R / f, color.G / f, color.B / f);
    }

    public static Color operator +(Color colorA, Color colorB)
    {
        return new Color(colorA.R + colorB.R, colorA.G + colorB.G, colorA.B + colorB.B);
    }

    public static Color operator -(Color colorA, Color colorB)
    {
        return new Color(colorA.R - colorB.R, colorA.G - colorB.G, colorA.B - colorB.B);
    }

    public static Color operator *(Color colorA, Color colorB)
    {
        return new Color(colorA.R * colorB.R, colorA.G * colorB.G, colorA.B * colorB.B);
    }

    public static Color operator /(Color colorA, Color colorB)
    {
        return new Color(colorA.R / colorB.R, colorA.G / colorB.G, colorA.B / colorB.B);
    }

    public override string ToString()
    {
        return string.Format($"R:{R}, G:{G}, B:{B}, A:{A}");
    }

    public static implicit operator System.Drawing.Color(Color c) => System.Drawing.Color.FromArgb((int)c.ToARGB());
    public static implicit operator Color(System.Drawing.Color c) => new Color(c.R, c.G, c.B, c.A);

    #region Predefined Colors

    private static System.Random RNG = new System.Random();

    public static Color Random => new Color((byte)RNG.Next(byte.MaxValue), (byte)RNG.Next(byte.MaxValue), (byte)RNG.Next(byte.MaxValue));
    public static Color RandomAlpha => new Color((byte)RNG.Next(byte.MaxValue), (byte)RNG.Next(byte.MaxValue), (byte)RNG.Next(byte.MaxValue), (byte)RNG.Next(byte.MaxValue));
    public static Color Black => new Color(0, 0, 0);
    public static Color Red => new Color(255, 0, 0);
    public static Color Green => new Color(0, 255, 0);
    public static Color Yellow => new Color(255, 255, 0);
    public static Color Blue => new Color(0, 0, 255);
    public static Color Magenta => new Color(255, 0, 255);
    public static Color Cyan => new Color(0, 255, 255);
    public static Color White => new Color(255, 255, 255);
    public static class ANSI
    {
        public static Color Black => new Color(0, 0, 0);
        public static Color Red => new Color(170, 0, 0);
        public static Color Green => new Color(0, 170, 0);
        public static Color Yellow => new Color(170, 85, 0);
        public static Color Blue => new Color(0, 0, 170);
        public static Color Magenta => new Color(170, 0, 170);
        public static Color Cyan => new Color(0, 170, 170);
        public static Color White => new Color(170, 170, 170);
        public static class BRIGHT
        {
            public static Color Black => new Color(85, 85, 85);
            public static Color Red => new Color(255, 85, 85);
            public static Color Green => new Color(85, 255, 85);
            public static Color Yellow => new Color(255, 255, 85);
            public static Color Blue => new Color(85, 85, 255);
            public static Color Magenta => new Color(255, 85, 255);
            public static Color Cyan => new Color(85, 255, 255);
            public static Color White => new Color(255, 255, 255);
        }
    }
    #endregion
}
>>>>>>> main
