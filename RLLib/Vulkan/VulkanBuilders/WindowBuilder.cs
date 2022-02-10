using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace RLLib.VulkanBuilders;

public class WindowBuilder
{
    private BuilderSettings Settings;

    public WindowBuilder() : base()
    {
        Settings = new BuilderSettings();
    }

    public VulkanWindow Build() => new VulkanWindow(Settings);

    #region Builder
    /// <summary>
    /// Sets the width of the created window.
    /// </summary>
    /// <param name="width"></param>
    /// <returns></returns>
    public WindowBuilder SetWidth(int width)
    {
        Settings.Width = width;
        return this;
    }

    /// <summary>
    /// Sets the height of the created window.
    /// </summary>
    /// <param name="height"></param>
    /// <returns></returns>
    public WindowBuilder SetHeight(int height)
    {
        Settings.Height = height;
        return this;
    }

    /// <summary>
    /// Sets the width and height of the created window.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public WindowBuilder SetSize(int width, int height)
    {
        Settings.Size = new Vector2D<int>(width, height);
        return this;
    }

    /// <summary>
    /// Sets the width and height of the created window.
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public WindowBuilder SetSize(Vector2D<int> size)
    {
        Settings.Size = size;
        return this;
    }

    /// <summary>
    /// Sets the title of the created window.
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    public WindowBuilder SetTitle(string title)
    {
        Settings.Title = title;
        return this;
    }

    /// <summary>
    /// Sets the WindowBorder of the created window.
    /// </summary>
    /// <param name="windowBorder"></param>
    /// <returns></returns>
    public WindowBuilder SetWindowBorder(Silk.NET.Windowing.WindowBorder windowBorder)
    {
        Settings.WindowOptions = Settings.WindowOptions with
        {
            WindowBorder = windowBorder,
        };
        return this;
    }

    /// <summary>
    /// Sets the WindowState of the created window.
    /// </summary>
    /// <param name="windowState"></param>
    /// <returns></returns>
    public WindowBuilder SetWindowState(Silk.NET.Windowing.WindowState windowState)
    {
        Settings.WindowOptions = Settings.WindowOptions with
        {
            WindowState = windowState,
        };
        return this;
    }

    #region Events
    /// <summary>
    /// Add a window callback for Load().
    /// </summary>
    /// <param name="onLoad"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnWindowLoad(Action onLoad)
    {
        Settings.OnLoad.Add(onLoad);
        return this;
    }

    /// <summary>
    /// Add a window callback for Update(double deltaTime).
    /// </summary>
    /// <param name="onUpdate"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnWindowUpdate(Action<double> onUpdate)
    {
        Settings.OnUpdate.Add(onUpdate);
        return this;
    }

    /// <summary>
    /// Add a window callback for LateUpdate(double deltaTime).
    /// LateUpdate is called immediately after Update(double deltaTime).
    /// </summary>
    /// <param name="onLateUpdate"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnWindowLateUpdate(Action<double> onLateUpdate)
    {
        Settings.OnLateUpdate.Add(onLateUpdate);
        return this;
    }

    public object addActionOnWindowFileDrop(object onWindowFileDrop)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add a window callback for Render(double deltaTime).
    /// </summary>
    /// <param name="onRender"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnWindowRender(Action<double> onRender)
    {
        Settings.OnRender.Add(onRender);
        return this;
    }

    /// <summary>
    /// Add a window callback for Resize(Vector2D<int> newSize).
    /// </summary>
    /// <param name="onResize"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnWindowResize(Action<Vector2D<int>> onResize)
    {
        Settings.OnResize.Add(onResize);
        return this;
    }

    /// <summary>
    /// Add a window callback for FrameBufferResize(Vector2D<int> newSize).
    /// </summary>
    /// <param name="onFrameBufferResize"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnWindowFrameBufferResize(Action<Vector2D<int>> onFrameBufferResize)
    {
        Settings.OnFrameBufferResize.Add(onFrameBufferResize);
        return this;
    }

    /// <summary>
    /// Add a window callback for FocusChanged(bool gainedFocus).
    /// </summary>
    /// <param name="onFocusChanged"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnWindowFocusChanged(Action<bool> onFocusChanged)
    {
        Settings.OnFocusChanged.Add(onFocusChanged);
        return this;
    }

    /// <summary>
    /// Add a window callback for StateChanged(WindowState state).
    /// </summary>
    /// <param name="onStateChanged"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnWindowStateChanged(Action<Silk.NET.Windowing.WindowState> onStateChanged)
    {
        Settings.OnStateChanged.Add(onStateChanged);
        return this;
    }

    /// <summary>
    /// Add a window callback for Move(Vector2D<int> position).
    /// </summary>
    /// <param name="onMove"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnWindowMove(Action<Vector2D<int>> onMove)
    {
        Settings.OnMove.Add(onMove);
        return this;
    }

    /// <summary>
    /// Add a window callback for FileDrop(string[] names).
    /// </summary>
    /// <param name="onFileDrop"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnWindowFileDrop(Action<string[]> onFileDrop)
    {
        Settings.OnFileDrop.Add(onFileDrop);
        return this;
    }

    /// <summary>
    /// Add a window callback for Closing().
    /// </summary>
    /// <param name="onClosing"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnWindowClosing(Action onClosing)
    {
        Settings.OnClosing.Add(onClosing);
        return this;
    }

    /// <summary>
    /// Add a mouse callback for MouseDown(MouseButton button).
    /// </summary>
    /// <param name="onMouseDown"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnMouseDown(Action<Silk.NET.Input.MouseButton> onMouseDown)
    {
        Settings.OnMouseDown.Add(onMouseDown);
        return this;
    }
    /// <summary>
    /// Add a mouse callback for MouseUp(MouseButton button).
    /// </summary>
    /// <param name="onMouseUp"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnMouseUp(Action<Silk.NET.Input.MouseButton> onMouseUp)
    {
        Settings.OnMouseUp.Add(onMouseUp);
        return this;
    }
    /// <summary>
    /// Add a mouse callback for MouseClick(MouseButton button, Vector2 position).
    /// </summary>
    /// <param name="onMouseClick"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnMouseClick(Action<Silk.NET.Input.MouseButton, System.Numerics.Vector2> onMouseClick)
    {
        Settings.OnMouseClick.Add(onMouseClick);
        return this;
    }
    /// <summary>
    /// Add a mouse callback for MouseDoubleClick(MouseButton button, Vector2 position).
    /// </summary>
    /// <param name="onMouseDoubleClick"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnMouseDoubleClick(Action<Silk.NET.Input.MouseButton, System.Numerics.Vector2> onMouseDoubleClick)
    {
        Settings.OnMouseDoubleClick.Add(onMouseDoubleClick);
        return this;
    }
    /// <summary>
    /// Add a mouse callback for MouseMove(Vector2 position).
    /// </summary>
    /// <param name="onMouseMove"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnMouseMove(Action<System.Numerics.Vector2> onMouseMove)
    {
        Settings.OnMouseMove.Add(onMouseMove);
        return this;
    }
    /// <summary>
    /// Add a mouse callback for MouseScroll(ScrollWheel scroll).
    /// </summary>
    /// <param name="onMouseScroll"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnMouseScroll(Action<Silk.NET.Input.ScrollWheel> onMouseScroll)
    {
        Settings.OnMouseScroll.Add(onMouseScroll);
        return this;
    }

    /// <summary>
    /// Add a key callback for KeyDown(Key key, int repeat).
    /// </summary>
    /// <param name="onKeyDown"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnKeyDown(Action<Silk.NET.Input.Key, int> onKeyDown)
    {
        Settings.OnKeyDown.Add(onKeyDown);
        return this;
    }
    /// <summary>
    /// Add a key callback for KeyUp(Key key, int repeat).
    /// </summary>
    /// <param name="onKeyUp"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnKeyUp(Action<Silk.NET.Input.Key, int> onKeyUp)
    {
        Settings.OnKeyUp.Add(onKeyUp);
        return this;
    }
    /// <summary>
    /// Add a key callback for KeyChar(char character).
    /// </summary>
    /// <param name="onKeyChar"></param>
    /// <returns></returns>
    public WindowBuilder AddActionOnKeyChar(Action<char> onKeyChar)
    {
        Settings.OnKeyChar.Add(onKeyChar);
        return this;
    }
    #endregion
    #endregion

    public WindowBuilder Reset()
    {
        Settings.Reset();
        return this;
    }

    internal class BuilderSettings
    {
        public WindowOptions WindowOptions { get; set; } = WindowOptions.DefaultVulkan;

        public int Width
        {
            get => WindowOptions.Size.X;
            set
            {
                WindowOptions = WindowOptions with
                {
                    Size = new Vector2D<int>(value, Height),
                };
            }
        }
        public int Height
        {
            get => WindowOptions.Size.Y;
            set
            {
                WindowOptions = WindowOptions with
                {
                    Size = new Vector2D<int>(Width, value),
                };
            }
        }
        public Vector2D<int> Size
        {
            get => WindowOptions.Size;
            set
            {
                WindowOptions = WindowOptions with
                {
                    Size = value,
                };
            }
        }
        public string Title
        {
            get => WindowOptions.Title;
            set
            {
                WindowOptions = WindowOptions with
                {
                    Title = value,
                };
            }
        }



        public List<Action> OnLoad = new List<Action>();
        public List<Action<double>> OnUpdate = new List<Action<double>>();
        public List<Action<double>> OnLateUpdate = new List<Action<double>>();
        public List<Action<double>> OnRender = new List<Action<double>>();
        public List<Action<Vector2D<int>>> OnResize = new List<Action<Vector2D<int>>>();
        public List<Action<Vector2D<int>>> OnFrameBufferResize = new List<Action<Vector2D<int>>>();
        public List<Action<bool>> OnFocusChanged = new List<Action<bool>>();
        public List<Action<Silk.NET.Windowing.WindowState>> OnStateChanged = new List<Action<Silk.NET.Windowing.WindowState>>();
        public List<Action<Vector2D<int>>> OnMove = new List<Action<Vector2D<int>>>();
        public List<Action<string[]>> OnFileDrop = new List<Action<string[]>>();
        public List<Action> OnClosing = new List<Action>();

        public List<Action<Silk.NET.Input.MouseButton>> OnMouseDown = new List<Action<Silk.NET.Input.MouseButton>>();
        public List<Action<Silk.NET.Input.MouseButton>> OnMouseUp = new List<Action<Silk.NET.Input.MouseButton>>();
        public List<Action<Silk.NET.Input.MouseButton, System.Numerics.Vector2>> OnMouseClick = new List<Action<Silk.NET.Input.MouseButton, System.Numerics.Vector2>>();
        public List<Action<Silk.NET.Input.MouseButton, System.Numerics.Vector2>> OnMouseDoubleClick = new List<Action<Silk.NET.Input.MouseButton, System.Numerics.Vector2>>();
        public List<Action<System.Numerics.Vector2>> OnMouseMove = new List<Action<System.Numerics.Vector2>>();
        public List<Action<Silk.NET.Input.ScrollWheel>> OnMouseScroll = new List<Action<Silk.NET.Input.ScrollWheel>>();

        public List<Action<Silk.NET.Input.Key, int>> OnKeyDown = new List<Action<Silk.NET.Input.Key, int>>();
        public List<Action<Silk.NET.Input.Key, int>> OnKeyUp = new List<Action<Silk.NET.Input.Key, int>>();
        public List<Action<char>> OnKeyChar = new List<Action<char>>();

        public void Reset()
        {
            WindowOptions = WindowOptions.DefaultVulkan;

            OnLoad.Clear();
            OnUpdate.Clear();
            OnLateUpdate.Clear();
            OnRender.Clear();
            OnResize.Clear();
            OnFrameBufferResize.Clear();
            OnFocusChanged.Clear();
            OnStateChanged.Clear();
            OnMove.Clear();
            OnFileDrop.Clear();
            OnClosing.Clear();
        }
    }
}

public class VulkanWindow : VkObject
{
    public IWindow Window { get; init; }
    public IInputContext Input { get; init; }

    public int Width
    {
        get
        {
            return Window.Size.X;
        }
        set
        {
            Window.Size = new Vector2D<int>(value, Height);
        }
    }
    public int Height
    {
        get
        {
            return Window.Size.Y;
        }
        set
        {
            Window.Size = new Vector2D<int>(Width, value);
        }
    }
    public Vector2D<int> Size
    {
        get
        {
            return Window.Size;
        }
        set
        {
            Window.Size = value;
        }
    }
    public string Title
    {
        get
        {
            return Window.Title;
        }
        set
        {
            Window.Title = value;
        }
    }

    private List<Action> OnLoad;
    private List<Action<double>> OnUpdate;
    private List<Action<double>> OnLateUpdate;
    private List<Action<double>> OnRender;
    private List<Action<Vector2D<int>>> OnResize;
    private List<Action<Vector2D<int>>> OnFrameBufferResize;
    private List<Action<bool>> OnFocusChanged;
    private List<Action<Silk.NET.Windowing.WindowState>> OnStateChanged;
    private List<Action<Vector2D<int>>> OnMove;
    private List<Action<string[]>> OnFileDrop;
    private List<Action> OnClosing;

    private List<Action<Silk.NET.Input.MouseButton>> OnMouseDown;
    private List<Action<Silk.NET.Input.MouseButton>> OnMouseUp;
    private List<Action<Silk.NET.Input.MouseButton, System.Numerics.Vector2>> OnMouseClick;
    private List<Action<Silk.NET.Input.MouseButton, System.Numerics.Vector2>> OnMouseDoubleClick;
    private List<Action<System.Numerics.Vector2>> OnMouseMove;
    private List<Action<Silk.NET.Input.ScrollWheel>> OnMouseScroll;

    private List<Action<Silk.NET.Input.Key, int>> OnKeyDown;
    private List<Action<Silk.NET.Input.Key, int>> OnKeyUp;
    private List<Action<char>> OnKeyChar;

    internal VulkanWindow(WindowBuilder.BuilderSettings settings)
    {
        // Create a native Window
        Window = Silk.NET.Windowing.Window.Create(settings.WindowOptions);
        
        // Setup Window Callbacks
        OnLoad = new List<Action>(settings.OnLoad);
        OnUpdate = new List<Action<double>>(settings.OnUpdate);
        OnLateUpdate = new List<Action<double>>(settings.OnLateUpdate);
        OnRender = new List<Action<double>>(settings.OnRender);
        OnResize = new List<Action<Vector2D<int>>>(settings.OnResize);
        OnFrameBufferResize = new List<Action<Vector2D<int>>>(settings.OnFrameBufferResize);
        OnFocusChanged = new List<Action<bool>>(settings.OnFocusChanged);
        OnStateChanged = new List<Action<Silk.NET.Windowing.WindowState>>(settings.OnStateChanged);
        OnMove = new List<Action<Vector2D<int>>>(settings.OnMove);
        OnFileDrop = new List<Action<string[]>>(settings.OnFileDrop);
        OnClosing = new List<Action>(settings.OnClosing);

        // Setup Mouse Callbacks
        OnMouseDown = new List<Action<Silk.NET.Input.MouseButton>>(settings.OnMouseDown);
        OnMouseUp = new List<Action<Silk.NET.Input.MouseButton>>(settings.OnMouseUp);
        OnMouseClick = new List<Action<Silk.NET.Input.MouseButton, System.Numerics.Vector2>>(settings.OnMouseClick);
        OnMouseDoubleClick = new List<Action<Silk.NET.Input.MouseButton, System.Numerics.Vector2>>(settings.OnMouseDoubleClick);
        OnMouseMove = new List<Action<System.Numerics.Vector2>>(settings.OnMouseMove);
        OnMouseScroll = new List<Action<Silk.NET.Input.ScrollWheel>>(settings.OnMouseScroll);

        // Setup Keyboard Callbacks
        OnKeyDown = new List<Action<Silk.NET.Input.Key, int>>(settings.OnKeyDown);
        OnKeyUp = new List<Action<Silk.NET.Input.Key, int>>(settings.OnKeyUp);
        OnKeyChar = new List<Action<char>>(settings.OnKeyChar);


        // Hook Window Callbacks
        Window.Load += () =>
        {
            foreach (var fxn in OnLoad)
            {
                fxn();
            }
        };
        Window.Update += (deltaTime) =>
        {
            foreach (var fxn in OnUpdate)
            {
                fxn(deltaTime);
            }
            foreach (var fxn in OnLateUpdate)
            {
                fxn(deltaTime);
            }
        };
        Window.Render += (deltaTime) =>
        {
            foreach (var fxn in OnRender)
            {
                fxn(deltaTime);
            }
        };
        Window.Resize += (size) =>
        {
            foreach (var fxn in OnResize)
            {
                fxn(size);
            }
        };
        Window.FramebufferResize += (frameBufferSize) =>
        {
            foreach (var fxn in OnFrameBufferResize)
            {
                fxn(frameBufferSize);
            }
        };
        Window.FocusChanged += (hasFocus) =>
        {
            foreach (var fxn in OnFocusChanged)
            {
                fxn(hasFocus);
            }
        };
        Window.StateChanged += (windowState) =>
        {
            foreach (var fxn in OnStateChanged)
            {
                fxn(windowState);
            }
        };
        Window.Move += (position) =>
        {
            foreach (var fxn in OnMove)
            {
                fxn(position);
            }
        };
        Window.FileDrop += (files) =>
        {
            foreach (var fxn in OnFileDrop)
            {
                fxn(files);
            }
        };
        Window.Closing += () =>
        {
            foreach (var fxn in OnClosing)
            {
                fxn();
            }
        };


        Window.Initialize();
        if (Window.VkSurface == null)
        {
            throw new Exception($"Windowing platform does not support Vulkan.");
        }

        Input = Window.CreateInput();

        // Hook Mouse Callbacks
        foreach (var mouse in Input.Mice)
        {
            mouse.MouseDown += (mouse, button) =>
            {
                foreach (var fxn in OnMouseDown)
                {
                    fxn(button);
                }
            };
            mouse.MouseUp += (mouse, button) =>
            {
                foreach (var fxn in OnMouseUp)
                {
                    fxn(button);
                }
            };
            mouse.Click += (mouse, button, position) =>
            {
                foreach (var fxn in OnMouseClick)
                {
                    fxn(button, position);
                }
            };
            mouse.DoubleClick += (mouse, button, position) =>
            {
                foreach (var fxn in OnMouseDoubleClick)
                {
                    fxn(button, position);
                }
            };
            mouse.MouseMove += (mouse, position) =>
            {
                foreach (var fxn in OnMouseMove)
                {
                    fxn(position);
                }
            };
            mouse.Scroll += (mouse, scroll) =>
            {
                foreach (var fxn in OnMouseScroll)
                {
                    fxn(scroll);
                }
            };
        }


        // Hook Keyboard Callbacks
        foreach (var keyboard in Input.Keyboards)
        {
            keyboard.KeyDown += (keyboard, key, repeat) =>
            {
                foreach (var fxn in OnKeyDown)
                {
                    fxn(key, repeat);
                }
            };
            keyboard.KeyUp += (keyboard, key, repeat) =>
            {
                foreach (var fxn in OnKeyUp)
                {
                    fxn(key, repeat);
                }
            };
            keyboard.KeyChar += (keyboard, keyChar) =>
            {
                foreach (var fxn in OnKeyChar)
                {
                    fxn(keyChar);
                }
            };
        }
    }

    /// <summary>
    /// If the IWindow is pregenerated, you can use this as a wrapper without callback functionality.
    /// </summary>
    /// <param name="window"></param>
    /// <exception cref="Exception"></exception>
    public VulkanWindow(IWindow window)
    {
        Window = window;

        if (Window.VkSurface == null)
        {
            throw new Exception($"Windowing platform does not support Vulkan.");
        }

        // Setup Window Callbacks
        OnLoad = new List<Action>();
        OnUpdate = new List<Action<double>>();
        OnLateUpdate = new List<Action<double>>();
        OnRender = new List<Action<double>>();
        OnResize = new List<Action<Vector2D<int>>>();
        OnFrameBufferResize = new List<Action<Vector2D<int>>>();
        OnFocusChanged = new List<Action<bool>>();
        OnStateChanged = new List<Action<Silk.NET.Windowing.WindowState>>();
        OnMove = new List<Action<Vector2D<int>>>();
        OnFileDrop = new List<Action<string[]>>();
        OnClosing = new List<Action>();

        // Setup Mouse Callbacks
        OnMouseDown = new List<Action<Silk.NET.Input.MouseButton>>();
        OnMouseUp = new List<Action<Silk.NET.Input.MouseButton>>();
        OnMouseClick = new List<Action<Silk.NET.Input.MouseButton, System.Numerics.Vector2>>();
        OnMouseDoubleClick = new List<Action<Silk.NET.Input.MouseButton, System.Numerics.Vector2>>();
        OnMouseMove = new List<Action<System.Numerics.Vector2>>();
        OnMouseScroll = new List<Action<Silk.NET.Input.ScrollWheel>>();

        // Setup Keyboard Callbacks
        OnKeyDown = new List<Action<Silk.NET.Input.Key, int>>();
        OnKeyUp = new List<Action<Silk.NET.Input.Key, int>>();
        OnKeyChar = new List<Action<char>>();

        // Hook Window Callbacks
        Window.Load += () =>
        {
            foreach (var fxn in OnLoad)
            {
                fxn();
            }
        };
        Window.Update += (deltaTime) =>
        {
            foreach (var fxn in OnUpdate)
            {
                fxn(deltaTime);
            }
            foreach (var fxn in OnLateUpdate)
            {
                fxn(deltaTime);
            }
        };
        Window.Render += (deltaTime) =>
        {
            foreach (var fxn in OnRender)
            {
                fxn(deltaTime);
            }
        };
        Window.Resize += (size) =>
        {
            foreach (var fxn in OnResize)
            {
                fxn(size);
            }
        };
        Window.FramebufferResize += (frameBufferSize) =>
        {
            foreach (var fxn in OnFrameBufferResize)
            {
                fxn(frameBufferSize);
            }
        };
        Window.FocusChanged += (hasFocus) =>
        {
            foreach (var fxn in OnFocusChanged)
            {
                fxn(hasFocus);
            }
        };
        Window.StateChanged += (windowState) =>
        {
            foreach (var fxn in OnStateChanged)
            {
                fxn(windowState);
            }
        };
        Window.Move += (position) =>
        {
            foreach (var fxn in OnMove)
            {
                fxn(position);
            }
        };
        Window.FileDrop += (files) =>
        {
            foreach (var fxn in OnFileDrop)
            {
                fxn(files);
            }
        };
        Window.Closing += () =>
        {
            foreach (var fxn in OnClosing)
            {
                fxn();
            }
        };

        Input = Window.CreateInput();

        // Hook Mouse Callbacks
        foreach (var mouse in Input.Mice)
        {
            mouse.Click += (mouse, button, position) =>
            {
                foreach (var fxn in OnMouseClick)
                {
                    fxn(button, position);
                }
            };
            mouse.DoubleClick += (mouse, button, position) =>
            {
                foreach (var fxn in OnMouseDoubleClick)
                {
                    fxn(button, position);
                }
            };
            mouse.MouseDown += (mouse, button) =>
            {
                foreach (var fxn in OnMouseDown)
                {
                    fxn(button);
                }
            };
            mouse.MouseUp += (mouse, button) =>
            {
                foreach (var fxn in OnMouseUp)
                {
                    fxn(button);
                }
            };
            mouse.MouseMove += (mouse, position) =>
            {
                foreach (var fxn in OnMouseMove)
                {
                    fxn(position);
                }
            };
            mouse.Scroll += (mouse, scroll) =>
            {
                foreach (var fxn in OnMouseScroll)
                {
                    fxn(scroll);
                }
            };
        }

        // Hook Keyboard Callbacks
        foreach (var keyboard in Input.Keyboards)
        {
            keyboard.KeyDown += (keyboard, key, repeat) =>
            {
                foreach (var fxn in OnKeyDown)
                {
                    fxn(key, repeat);
                }
            };
            keyboard.KeyUp += (keyboard, key, repeat) =>
            {
                foreach (var fxn in OnKeyUp)
                {
                    fxn(key, repeat);
                }
            };
            keyboard.KeyChar += (keyboard, keyChar) =>
            {
                foreach (var fxn in OnKeyChar)
                {
                    fxn(keyChar);
                }
            };
        }
    }

    public void Run()
    {
        Window.Run();
    }

    public void Close()
    {
        Window.Close();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Window.Dispose();
        }
        base.Dispose(disposing);
    }
}