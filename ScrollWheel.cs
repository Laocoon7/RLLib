using System.Globalization;
using System.Text;

namespace RLLib;

public struct ScrollWheel
{
    //
    // Summary:
    //     The X position of the scroll wheel.
    public float X
    {
        get;
    }

    //
    // Summary:
    //     The Y position of the scroll wheel.
    public float Y
    {
        get;
    }

    //
    // Summary:
    //     Creates a new instance of the scroll wheel struct.
    //
    // Parameters:
    //   x:
    //     The X position of the scroll wheel.
    //
    //   y:
    //     The Y position of the scroll wheel.
    public ScrollWheel(float x, float y)
    {
        X = x;
        Y = y;
    }

    public ScrollWheel(Silk.NET.Input.ScrollWheel scrollWheel)
    {
        X = scrollWheel.X;
        Y = scrollWheel.Y;
    }

    //
    // Summary:
    //     Returns a String representing this Silk.NET.Input.ScrollWheel instance.
    //
    // Returns:
    //     The string representation.
    public override readonly string ToString()
    {
        return ToString("G", CultureInfo.CurrentCulture);
    }

    //
    // Summary:
    //     Returns a String representing this Silk.NET.Input.ScrollWheel instance, using
    //     the specified format to format individual elements.
    //
    // Parameters:
    //   format:
    //     The format of individual elements.
    //
    // Returns:
    //     The string representation.
    public readonly string ToString(string? format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }

    //
    // Summary:
    //     Returns a String representing this Silk.NET.Input.ScrollWheel instance, using
    //     the specified format to format individual elements and the given IFormatProvider.
    //
    // Parameters:
    //   format:
    //     The format of individual elements.
    //
    //   formatProvider:
    //     The format provider to use when formatting elements.
    //
    // Returns:
    //     The string representation.
    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        StringBuilder stringBuilder = new StringBuilder();
        string numberGroupSeparator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
        stringBuilder.Append('<');
        stringBuilder.Append(X.ToString(format, formatProvider));
        stringBuilder.Append(numberGroupSeparator);
        stringBuilder.Append(' ');
        stringBuilder.Append(Y.ToString(format, formatProvider));
        stringBuilder.Append('>');
        return stringBuilder.ToString();
    }
}
