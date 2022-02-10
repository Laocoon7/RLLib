using RLLib;

namespace Sample;
internal class SampleWindow : GameWindow
{
    // Just hard-coded width/height as we normally wouldn't want to change the number of tiles on the screen
    private const int WINDOW_TILE_WIDTH = 80;
    private const int WINDOW_TILE_HEIGHT = 60;

    // Constructor
    // Will set Width, Height for us and create our grid.
    public SampleWindow() : base(WINDOW_TILE_WIDTH, WINDOW_TILE_HEIGHT)
    {
        // Make a new grid like in Chapter 3
        SubConsole = new Grid(Width, 1);
    }

    // The name of our program
    protected override string NAME => nameof(SampleWindow);
    // Version of our program
    protected override uint VERSION_MAJOR => 0;
    protected override uint VERSION_MINOR => 1;
    protected override uint VERSION_PATCH => 0;

    // This is the initial texture loaded.
    // This will also be updated during calls to LoadTexture
    // We should never write to this ourselves.
    protected override string TEXTURE_FILENAME { get; set; } = "terminal16x16.png";
    // These are the initial dimensions of our initial texture.
    // These will also be updated during calls to LoadTexture
    // We should never write to these ourselves.
    protected override int TEXTURE_TILE_WIDTH { get; set; } = 16;
    protected override int TEXTURE_TILE_HEIGHT { get; set; } = 16;

    // Just an enum to keep track of which texture we have loaded.
    private TextureSize CurrentTexture = TextureSize.SixteenBySixteen;
    // File names for other textures we may want to load.
    private string Texture8x8 = "terminal8x8.png";
    private string Texture16x16 = "terminal16x16.png";
    private string Texture32x32 = "terminal32x32.png";
    // Keep track of mouse coordinates.
    private int MouseX, MouseY;
    // Our sub console.
    private Grid SubConsole;

    // Called once per frame before drawing.
    protected override void OnWindowUpdate(double deltaTime)
    {
        // Perform actions in our sub console
        UpdateSubConsole();

        // Clear our grid
        Clear();

        // Test Blit sub console to grid
        Blit(SubConsole, this, Width, 1);

        // Test Set background color at mouse coordinates
        Set(MouseX, MouseY, background: Color.ANSI.Blue);

        // Test set tile by (x, y) texture position
        Set(25, 25, 1, 1, Color.ANSI.Cyan, Color.ANSI.Yellow);

        // Test ColoredStringBuilder
        // This lets us easily write strings with multiple colors throughout the string.
        var text = new ColoredStringBuilder()
            .SetForegroundColor(Color.Blue)
            .SetBackgroundColor(Color.Black)
            .Add("Hello ")
            .SetForegroundColor(Color.Black)
            .SetBackgroundColor(Color.White)
            .Add("World!")
            .Build();

        // print our new ColoredString
        Print(0, 1, text);
    }

    // Called when the mouse moves on screen (doesn't have to change cells)
    protected override void OnMouseMove(int x, int y)
    {
        // x, y are cell coordinates
        MouseX = x;
        MouseY = y;
    }

    // Called when a key is let up on the keyboard
    protected override void OnKeyUp(Key key, int scancode)
    {
        switch (key)
        {
            case Key.Escape:
                Close();
                break;
            case Key.KeypadAdd:
                // Switch to a bigger font texture if possible.

                if (CurrentTexture == TextureSize.EightByEight)
                {
                    // We can only have one loaded texture at a time.
                    // So this will unload the current texture and load a new one.
                    LoadTexture(Texture16x16, 16, 16);
                    CurrentTexture = TextureSize.SixteenBySixteen;
                }
                else if (CurrentTexture == TextureSize.SixteenBySixteen)
                {
                    LoadTexture(Texture32x32, 32, 32);
                    CurrentTexture = TextureSize.ThirtyTwoByThirtyTwo;
                }
                break;
            case Key.KeypadSubtract:
                // Switch to a smaller font texture if possible.

                if (CurrentTexture == TextureSize.ThirtyTwoByThirtyTwo)
                {
                    LoadTexture(Texture16x16, 16, 16);
                    CurrentTexture = TextureSize.SixteenBySixteen;
                }
                else if (CurrentTexture == TextureSize.SixteenBySixteen)
                {
                    LoadTexture(Texture8x8, 8, 8);
                    CurrentTexture = TextureSize.EightByEight;
                }
                break;
            default:
                break;
        }
    }

    private void UpdateSubConsole()
    {
        // Clear Our Sub Console
        SubConsole.Clear();

        // Test Print the mouse coordinates
        SubConsole.Print(0, 0, $"({MouseX}, {MouseY})", Color.Red, Color.Black);
    }

    private enum TextureSize
    {
        EightByEight,
        SixteenBySixteen,
        ThirtyTwoByThirtyTwo,
    }
}
