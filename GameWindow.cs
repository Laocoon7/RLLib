<<<<<<< HEAD
﻿using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Vulkan;
using Silk.NET.Windowing;

using VkSharp;

using Buffer = Silk.NET.Vulkan.Buffer;

namespace RLLib;


public abstract class GameWindow : Grid, IDisposable
{
    #region Protected
    /// <summary>
    /// Name of your app.
    /// </summary>
    protected abstract string NAME { get; }
    /// <summary>
    /// MajorVersion of your app.
    /// </summary>
    protected abstract uint VERSION_MAJOR { get; }
    /// <summary>
    /// MinorVersion of your app.
    /// </summary>
    protected abstract uint VERSION_MINOR { get; }
    /// <summary>
    /// PatchVersion of your app.
    /// </summary>
    protected abstract uint VERSION_PATCH { get; }

    /// <summary>
    /// Path to an image/texture file which will be loaded on creation.
    /// This will be updated if a new image is loaded through <see cref="LoadTexture(string, int, int)"/>
    /// </summary>
    protected abstract string TEXTURE_FILENAME { get; set; }
    /// <summary>
    /// Width of each tile. Used to lookup individual tiles from the loaded texture.
    /// This will be updated if a new image is loaded through <see cref="LoadTexture(string, int, int)"/>
    /// </summary>
    protected abstract int TEXTURE_TILE_WIDTH { get; set; }
    /// <summary>
    /// Height of each tile. Used to lookup individual tiles from the loaded texture.
    /// This will be updated if a new image is loaded through <see cref="LoadTexture(string, int, int)"/>
    /// </summary>
    protected abstract int TEXTURE_TILE_HEIGHT { get; set; }


    /// <summary>
    /// Called as the Window is created.
    /// </summary>
    protected virtual void OnWindowLoad() { }
    /// <summary>
    /// Called each frame before drawing the screen.
    /// </summary>
    /// <param name="deltaTime">The amount of time since the last frame.</param>
    protected virtual void OnWindowUpdate(double deltaTime) { }
    /// <summary>
    /// Called each frame after <see cref="OnWindowUpdate(double)"/> but before drawing the screen.
    /// </summary>
    /// <param name="deltaTime">The amount of time since the last frame.</param>
    protected virtual void OnWindowLateUpdate(double deltaTime) { }
    /// <summary>
    /// Called if the window state changes.
    /// </summary>
    /// <param name="windowState">The new window state.</param>
    protected virtual void OnWindowStateChanged(WindowState windowState) { }
    /// <summary>
    /// Called if the window loses/gains focus.
    /// </summary>
    /// <param name="hasFocus">TRUE if window now has focus.</param>
    protected virtual void OnWindowFocusChanged(bool hasFocus) { }
    /// <summary>
    /// Called if files are dropped onto the window.
    /// </summary>
    /// <param name="files">Array of filenames.</param>
    protected virtual void OnWindowFileDrop(string[] files) { }
    /// <summary>
    /// Called when the window closes.
    /// </summary>
    protected virtual void OnWindowClose() { }
    /// <summary>
    /// Called when a mouse button is pressed down.
    /// </summary>
    /// <param name="button">The mouse button pressed down.</param>
    protected virtual void OnMouseDown(MouseButton button) { }
    /// <summary>
    /// Called when a mouse button is unpressed.
    /// </summary>
    /// <param name="button">The mouse button unpressed.</param>
    protected virtual void OnMouseUp(MouseButton button) { }
    /// <summary>
    /// Called when a mouse button is clicked.
    /// </summary>
    /// <param name="button">The mouse button clicked.</param>
    /// <param name="x">The X tile coordinate.</param>
    /// <param name="y">The Y tile coordinate.</param>
    protected virtual void OnMouseClick(MouseButton button, int x, int y) { }
    /// <summary>
    /// Called when a mouse button is double clicked.
    /// </summary>
    /// <param name="button">The mouse button double clicked.</param>
    /// <param name="x">The X tile coordinate.</param>
    /// <param name="y">The Y tile coordinate.</param>
    protected virtual void OnMouseDoubleClick(MouseButton button, int x, int y) { }
    /// <summary>
    /// Called when the mouse moves.
    /// </summary>
    /// <param name="x">The X tile coordinate.</param>
    /// <param name="y">The Y tile coordinate.</param>
    protected virtual void OnMouseMove(int x, int y) { }
    /// <summary>
    /// Called when the mouse scroll wheel scrolls.
    /// </summary>
    /// <param name="scrollWheel">The mouse scroll wheel.</param>
    protected virtual void OnMouseScroll(ScrollWheel scrollWheel) { }
    /// <summary>
    /// Called when a key is pressed down.
    /// </summary>
    /// <param name="key">The key pressed down.</param>
    /// <param name="scancode">Number of repeats.</param>
    protected virtual void OnKeyDown(Key key, int scancode) { }
    /// <summary>
    /// Called when a key is unpressed.
    /// </summary>
    /// <param name="key">The key unpressed.</param>
    /// <param name="scancode">Number of repeats.</param>
    protected virtual void OnKeyUp(Key key, int scancode) { }
    /// <summary>
    /// Called when a key char is pressed.
    /// </summary>
    /// <param name="character">The key char.</param>
    protected virtual void OnKeyChar(char character) { }
    #endregion

    #region Public
    public unsafe GameWindow(int WINDOW_TILE_WIDTH, int WINDOW_TILE_HEIGHT) : base(WINDOW_TILE_WIDTH, WINDOW_TILE_HEIGHT)
    {
        VulkanWindow = new WindowBuilder()
            .SetTitle(NAME)
            .SetSize(ScreenWidth, ScreenHeight)
            .SetWindowBorder(WindowBorder.Fixed)
            .SetWindowState(WindowState.Normal)
            .AddActionOnWindowLoad(OnWindowLoad)
            .AddActionOnWindowUpdate(OnWindowUpdate)
            .AddActionOnWindowLateUpdate(OnWindowLateUpdate)
            .AddActionOnWindowRender(Draw)
            .AddActionOnWindowStateChanged(OnWindowStateChanged)
            .AddActionOnWindowFocusChanged(OnWindowFocusChanged)
            .AddActionOnWindowFileDrop(OnWindowFileDrop)
            .AddActionOnWindowClosing(OnWindowClose)
            .AddActionOnMouseDown(OnMouseDown)
            .AddActionOnMouseUp(OnMouseUp)
            .AddActionOnMouseClick((b, p) => OnMouseClick(b, (int)(p.X / TEXTURE_TILE_WIDTH), (int)(p.Y / TEXTURE_TILE_HEIGHT)))
            .AddActionOnMouseDoubleClick((b, p) => OnMouseDoubleClick(b, (int)(p.X / TEXTURE_TILE_WIDTH), (int)(p.Y / TEXTURE_TILE_HEIGHT)))
            .AddActionOnMouseMove((p) => OnMouseMove((int)(p.X / TEXTURE_TILE_WIDTH), (int)(p.Y / TEXTURE_TILE_HEIGHT)))
            .AddActionOnMouseScroll(OnMouseScroll)
            .AddActionOnKeyDown(OnKeyDown)
            .AddActionOnKeyUp(OnKeyUp)
            .AddActionOnKeyChar(OnKeyChar)
            .Build();
        VulkanInstance = new InstanceBuilder()
#if DEBUG
            .RequireValidationLayers()
            .SetDebugMessengerSeverity(DebugUtilsMessageSeverityFlagsEXT.DebugUtilsMessageSeverityWarningBitExt | DebugUtilsMessageSeverityFlagsEXT.DebugUtilsMessageSeverityErrorBitExt)
            .SetDebugMessengerType(DebugUtilsMessageTypeFlagsEXT.DebugUtilsMessageTypeGeneralBitExt | DebugUtilsMessageTypeFlagsEXT.DebugUtilsMessageTypeValidationBitExt | DebugUtilsMessageTypeFlagsEXT.DebugUtilsMessageTypePerformanceBitExt)
            .SetDebugCallback(OnDebugMessage)
#endif
            .SetWindow(VulkanWindow)
            .RequiredApiVersion(Vk.Version12)
            .SetAppName(NAME)
            .SetAppVersion(VERSION_MAJOR, VERSION_MINOR, VERSION_PATCH)
            .SetEngineName(ENGINE_NAME)
            .SetEngineVersion(ENGINE_VERSION_MAJOR, ENGINE_VERSION_MINOR, ENGINE_VERSION_PATCH)
            .Build();
        VulkanSurface = new SurfaceBuilder(VulkanInstance)
            .Build();
        var queueBuilder = new QueueBuilder();
        VulkanGraphicsQueue = queueBuilder
            .RequireGraphics()
            .Build();
        VulkanPresentQueue = queueBuilder.Reset()
            .RequirePresent()
            .Build();
        VulkanPhysicalDevice = new PhysicalDeviceBuilder(VulkanInstance, VulkanSurface)
            .SetPerferredDeviceType(PhysicalDeviceType.DiscreteGpu)
            .AddRequiredQueue(VulkanGraphicsQueue)
            .AddRequiredQueue(VulkanPresentQueue)
            .Build();
        VulkanLogicalDevice = new LogicalDeviceBuilder(VulkanPhysicalDevice)
            .Build();
        VulkanSwapchain = new SwapchainBuilder(VulkanLogicalDevice, VulkanGraphicsQueue, VulkanPresentQueue)
            .AddDefaultFormats()
            .AddDefaultPresentModes()
            .Build();
        VulkanRenderPass = new RenderPassBuilder(VulkanSwapchain)
            .Build();
        VulkanSwapchain.GetFramebuffers(VulkanRenderPass);
        VulkanDescriptorSetLayout = new DescriptorSetLayoutBuilder(VulkanLogicalDevice)
            .AddDescriptor(0, DescriptorType.UniformBuffer, ShaderStageFlags.ShaderStageVertexBit, VulkanSwapchain.ImageCount)
            .AddDescriptor(1, DescriptorType.CombinedImageSampler, ShaderStageFlags.ShaderStageFragmentBit, VulkanSwapchain.ImageCount)
            .Build();
        var shaderBuilder = new ShaderBuilder(VulkanLogicalDevice);
        VulkanVertexShader = shaderBuilder
            .SetShaderStageFlags(ShaderStageFlags.ShaderStageVertexBit)
            .SetFile(VERTEX_SHADER_FILENAME)
            .Build();
        VulkanFragmentShader = shaderBuilder.Reset()
            .SetShaderStageFlags(ShaderStageFlags.ShaderStageFragmentBit)
            .SetFile(FRAGMENT_SHADER_FILENAME)
            .Build();
        VulkanGraphicsPipeline = new GraphicsPipelineBuilder(VulkanRenderPass, VulkanDescriptorSetLayout)
            .AddVertexInput(Vertex.GetBindingDescription, Vertex.GetAttributeDescriptions)
            .AddShader(VulkanVertexShader)
            .AddShader(VulkanFragmentShader)
            .Build();
        VulkanCommandPool = new CommandPoolBuilder(VulkanLogicalDevice, VulkanGraphicsQueue)
            .SetCommandPoolCreateFlags(CommandPoolCreateFlags.CommandPoolCreateResetCommandBufferBit)
            .Build();
        VulkanCommandBuffers = new CommandBuffersBuilder(VulkanCommandPool)
            .SetCount((uint)VulkanSwapchain.Framebuffers!.Length)
            .Build();
        VulkanDescriptorPool = new DescriptorPoolBuilder(VulkanDescriptorSetLayout)
            .SetMaxSets(VulkanSwapchain.ImageCount)
            .Build();
        VulkanDescriptorSets = new DescriptorSetsBuilder(VulkanDescriptorPool)
            .Build();
        var semaphoreBuilder = new SemaphoreBuilder(VulkanLogicalDevice);
        var fenceBuilder = new FenceBuilder(VulkanLogicalDevice)
            .SetFenceCreateFlags(FenceCreateFlags.FenceCreateSignaledBit);
        ImageAvailableSemaphores = new VulkanSemaphore[MAX_FRAMES_IN_FLIGHT];
        RenderFinishedSemaphores = new VulkanSemaphore[MAX_FRAMES_IN_FLIGHT];
        InFlightFences = new VulkanFence[MAX_FRAMES_IN_FLIGHT];
        ImagesInFlight = new VulkanFence[VulkanSwapchain.ImageCount];
        for (int i = 0; i < MAX_FRAMES_IN_FLIGHT; i++)
        {
            ImageAvailableSemaphores[i] = semaphoreBuilder.Build();
            RenderFinishedSemaphores[i] = semaphoreBuilder.Build();
            InFlightFences[i] = fenceBuilder.Build();
        }

        var uniformBufferBuilder = new BufferBuilder(VulkanLogicalDevice)
            .SetBufferUsageFlags(BufferUsageFlags.BufferUsageUniformBufferBit)
            .SetMemoryPropertyFlags(MemoryPropertyFlags.MemoryPropertyDeviceLocalBit | MemoryPropertyFlags.MemoryPropertyHostVisibleBit)
            .SetSize((ulong)Unsafe.SizeOf<UniformBufferObject>());
        VertexBufferBuilder = new BufferBuilder(VulkanLogicalDevice)
            .SetBufferUsageFlags(BufferUsageFlags.BufferUsageTransferDstBit | BufferUsageFlags.BufferUsageVertexBufferBit)
            .SetMemoryPropertyFlags(MemoryPropertyFlags.MemoryPropertyDeviceLocalBit | MemoryPropertyFlags.MemoryPropertyHostVisibleBit);
        IndexBufferBuilder = new BufferBuilder(VulkanLogicalDevice)
            .SetBufferUsageFlags(BufferUsageFlags.BufferUsageTransferDstBit | BufferUsageFlags.BufferUsageIndexBufferBit)
            .SetMemoryPropertyFlags(MemoryPropertyFlags.MemoryPropertyDeviceLocalBit | MemoryPropertyFlags.MemoryPropertyHostVisibleBit);

        VulkanUniformBuffers = new VulkanBuffer[VulkanSwapchain.ImageCount];
        VulkanVertexBuffers = new VulkanBuffer[VulkanSwapchain.ImageCount];
        VulkanIndexBuffers = new VulkanBuffer[VulkanSwapchain.ImageCount];

        for (int i = 0; i < VulkanUniformBuffers.Length; i++)
        {
            VulkanUniformBuffers[i] = uniformBufferBuilder.Build();
        }
        VulkanTexture = new TextureBuilder(VulkanCommandPool)
            .SetFile(TEXTURE_FILENAME)
            .SetSamplerAnisotropyEnable(false)
            .SetSamplerMinFilter(Filter.Nearest)
            .SetSamplerMagFilter(Filter.Nearest)
            .Build();
        for (uint i = 0; i < VulkanSwapchain.ImageCount; i++)
        {
            UpdateDescriptorSets(i);
        }

        Camera = new OrthographicCamera(Width / 2, Height / 2, Width, Height);
    }

    public void Run() => VulkanWindow.Run();

    public void Close() => VulkanWindow.Close();
    #endregion

    #region private
    private const string VERTEX_SHADER_FILENAME = "VertexShader.spv";
    private const string FRAGMENT_SHADER_FILENAME = "FragmentShader.spv";

    private const string ENGINE_NAME = "RLLib";
    private const uint ENGINE_VERSION_MAJOR = 0;
    private const uint ENGINE_VERSION_MINOR = 1;
    private const uint ENGINE_VERSION_PATCH = 0;

    private const int MAX_FRAMES_IN_FLIGHT = 2;

    private VulkanWindow VulkanWindow;
    private VulkanInstance VulkanInstance;
    private VulkanSurface VulkanSurface;
    private VulkanQueue VulkanGraphicsQueue;
    private VulkanQueue VulkanPresentQueue;
    private VulkanPhysicalDevice VulkanPhysicalDevice;
    private VulkanLogicalDevice VulkanLogicalDevice;
    private VulkanSwapchain VulkanSwapchain;
    private VulkanRenderPass VulkanRenderPass;
    private VulkanDescriptorSetLayout VulkanDescriptorSetLayout;
    private VulkanShader VulkanVertexShader;
    private VulkanShader VulkanFragmentShader;
    private VulkanGraphicsPipeline VulkanGraphicsPipeline;
    private VulkanDescriptorPool VulkanDescriptorPool;
    private VulkanDescriptorSets VulkanDescriptorSets;
    private VulkanCommandPool VulkanCommandPool;
    private VulkanCommandBuffers VulkanCommandBuffers;
    private VulkanSemaphore[] ImageAvailableSemaphores;
    private VulkanSemaphore[] RenderFinishedSemaphores;
    private VulkanFence[] InFlightFences;
    private VulkanFence[] ImagesInFlight;

    private VulkanBuffer[] VulkanUniformBuffers;
    private BufferBuilder VertexBufferBuilder;
    private VulkanBuffer?[] VulkanVertexBuffers;
    private BufferBuilder IndexBufferBuilder;
    private VulkanBuffer?[] VulkanIndexBuffers;

    private VulkanTexture VulkanTexture;
    private OrthographicCamera Camera;

    private int ImageWidth => VulkanTexture.Width;
    private int ImageHeight => VulkanTexture.Height;
    private int TilesPerRow => ImageWidth / TEXTURE_TILE_WIDTH;
    private int TilesPerCol => ImageHeight / TEXTURE_TILE_HEIGHT;

    private int ScreenWidth => Width * TEXTURE_TILE_WIDTH;
    private int ScreenHeight => Height * TEXTURE_TILE_HEIGHT;

    private int CurrentFrame = 0;

    private unsafe void Draw(double deltaTime)
    {
        VkInfo.VkFunc.WaitForFences(VulkanLogicalDevice, new[] { InFlightFences[CurrentFrame].Fence }, true, ulong.MaxValue);

        uint imageIndex = 0;
        var result = VulkanSwapchain.KhrSwapchain.AcquireNextImage(VulkanLogicalDevice, VulkanSwapchain, ulong.MaxValue, ImageAvailableSemaphores[CurrentFrame], default, ref imageIndex);

        if (result != Result.Success)
        {
            throw new Exception($"Failed to acquire swap chain image.");
        }

        UpdateFromGrid(imageIndex);

        if (ImagesInFlight[imageIndex] != null &&
            ImagesInFlight[imageIndex].Fence.Handle != default)
        {
            VkInfo.VkFunc.WaitForFences(VulkanLogicalDevice, new[] { ImagesInFlight[imageIndex].Fence }, true, ulong.MaxValue);
        }
        ImagesInFlight[imageIndex] = InFlightFences[CurrentFrame];

        var submitInfo = new SubmitInfo()
        {
            SType = StructureType.SubmitInfo,
        };

        var waitSemaphores = stackalloc[] { ImageAvailableSemaphores[CurrentFrame].Semaphore };
        var waitStages = stackalloc[] { PipelineStageFlags.PipelineStageColorAttachmentOutputBit };

        var buffer = VulkanCommandBuffers[imageIndex].CommandBuffer;

        submitInfo = submitInfo with
        {
            WaitSemaphoreCount = 1,
            PWaitSemaphores = waitSemaphores,
            PWaitDstStageMask = waitStages,

            CommandBufferCount = 1,
            PCommandBuffers = &buffer
        };

        var signalSemaphores = stackalloc[] { RenderFinishedSemaphores![CurrentFrame].Semaphore };
        submitInfo = submitInfo with
        {
            SignalSemaphoreCount = 1,
            PSignalSemaphores = signalSemaphores,
        };

        VkInfo.VkFunc.ResetFences(VulkanLogicalDevice, InFlightFences[CurrentFrame]);

        if (VkInfo.VkFunc.QueueSubmit(VulkanGraphicsQueue, InFlightFences[CurrentFrame], submitInfo) != Result.Success)
        {
            throw new Exception("Failed to submit draw command buffer!");
        }

        var swapChains = stackalloc[] { VulkanSwapchain.SwapchainKHR };
        var presentInfo = new PresentInfoKHR()
        {
            SType = StructureType.PresentInfoKhr,

            WaitSemaphoreCount = 1,
            PWaitSemaphores = signalSemaphores,

            SwapchainCount = 1,
            PSwapchains = swapChains,

            PImageIndices = &imageIndex
        };

        result = VulkanSwapchain.KhrSwapchain.QueuePresent(VulkanPresentQueue, presentInfo);

        if (result != Result.Success)
        {
            throw new Exception($"Failed to present swapchain image.");
        }

        CurrentFrame = (CurrentFrame + 1) % MAX_FRAMES_IN_FLIGHT;
    }

    #region Update
    private void UpdateFromGrid(uint imageIndex)
    {
        UpdateUniformBuffer(imageIndex);

        var vertices = GenerateVertices();
        var indices = GenerateIndices(vertices);

        VulkanVertexBuffers[imageIndex]?.Dispose();
        VulkanVertexBuffers[imageIndex] = GenerateVertexBuffer(vertices);
        VulkanIndexBuffers[imageIndex]?.Dispose();
        VulkanIndexBuffers[imageIndex] = GenerateIndexBuffer(indices);
        UpdateCommandBuffer(imageIndex, VulkanVertexBuffers[imageIndex]!, VulkanIndexBuffers[imageIndex]!, (uint)indices.Length);
    }

    private unsafe void UpdateUniformBuffer(uint imageIndex)
    {
        var ubo = new UniformBufferObject()
        {
            Model = Matrix4X4<float>.Identity,
            View = Camera.ViewMatrix,
            Projection = Camera.ProjectionMatrix,
        };

        VulkanUniformBuffers[imageIndex].Write<UniformBufferObject>(new[] { ubo });
    }
    private unsafe void UpdateCommandBuffer(uint imageIndex, VulkanBuffer vertexBuffer, VulkanBuffer indexBuffer, uint indicesLength)
    {
        var commandBuffer = VulkanCommandBuffers[imageIndex];

        commandBuffer.ResetCommandBuffer(0);

        var beginInfo = new CommandBufferBeginInfo()
        {
            SType = StructureType.CommandBufferBeginInfo,
        };

        if (commandBuffer.BeginCommandBuffer(beginInfo) != Result.Success)
        {
            throw new Exception("Failed to begin recording command buffer!");
        }

        var renderPassInfo = new RenderPassBeginInfo()
        {
            SType = StructureType.RenderPassBeginInfo,
            RenderPass = VulkanRenderPass,
            Framebuffer = VulkanSwapchain.Framebuffers![imageIndex],
            RenderArea =
                {
                    Offset = { X = 0, Y = 0 },
                    Extent = VulkanSwapchain.Extent,
                }
        };

        var clearColor = new ClearValue()
        {
            Color = new() { Float32_0 = 0, Float32_1 = 0, Float32_2 = 0, Float32_3 = 1 },
        };

        renderPassInfo.ClearValueCount = 1;
        renderPassInfo.PClearValues = &clearColor;

        commandBuffer.CmdBeginRenderPass(renderPassInfo, SubpassContents.Inline);

        commandBuffer.CmdBindPipeline(PipelineBindPoint.Graphics, VulkanGraphicsPipeline);

        var vertexBuffers = new Buffer[] { vertexBuffer };
        var offsets = new ulong[] { 0 };

        commandBuffer.CmdBindVertexBuffers(0, vertexBuffers, offsets);

        commandBuffer.CmdBindIndexBuffer(indexBuffer, 0, IndexType.Uint32);

        commandBuffer.CmdBindDescriptorSets(PipelineBindPoint.Graphics, VulkanGraphicsPipeline.PipelineLayout, 0, new[] { VulkanDescriptorSets.DescriptorSets[imageIndex] }, null);

        commandBuffer.CmdDrawIndexed(indicesLength, 1, 0, 0, 0);


        commandBuffer.CmdEndRenderPass();

        if (commandBuffer.EndCommandBuffer() != Result.Success)
        {
            throw new Exception("Failed to record command buffer!");
        }
    }

    private Vertex[] GenerateVertices()
    {
        var vertices = new Vertex[Width * Height * 4];

        int i = 0;
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var cell = Cells[x, y];

                var FR = cell.Color.R;
                var FG = cell.Color.G;
                var FB = cell.Color.B;
                var FA = cell.Color.A;

                var BR = cell.BackColor.R;
                var BG = cell.BackColor.G;
                var BB = cell.BackColor.B;
                var BA = cell.BackColor.A;

                var flipHorizontally = (cell.Flipped & GridCell.Flip.Horizontally) != 0;
                var flipVertically = (cell.Flipped & GridCell.Flip.Vertically) != 0;

                float tcLeft;
                float tcRight;
                float tcBottom;
                float tcTop;

                switch (cell.CellDisplayType)
                {
                    case GridCell.DisplayType.XY:
                        if (!LookupTile(cell.X, cell.Y, out tcLeft, out tcRight, out tcBottom, out tcTop, flipHorizontally, flipVertically))
                        {
                            Console.WriteLine($"Invalid cell({cell.X}, {cell.Y}).");
                        }
                        break;
                    default:
                    case GridCell.DisplayType.Character:
                        if (!LookupTile(cell.Character, out tcLeft, out tcRight, out tcBottom, out tcTop, flipHorizontally, flipVertically))
                        {
                            Console.WriteLine($"Invalid character({cell.Character}).");
                        }
                        break;
                }

                vertices[i + 0] = new Vertex()
                {
                    Position = new Vector3D<float>(x, y, 0),
                    Color = new Vector4D<float>(FR, FG, FB, FA),
                    BackColor = new Vector4D<float>(BR, BG, BB, BA),
                    TextureCoord = new Vector2D<float>(tcLeft, tcBottom),
                };
                vertices[i + 1] = new Vertex()
                {
                    Position = new Vector3D<float>(x, y + 1, 0),
                    Color = new Vector4D<float>(FR, FG, FB, FA),
                    BackColor = new Vector4D<float>(BR, BG, BB, BA),
                    TextureCoord = new Vector2D<float>(tcLeft, tcTop),
                };
                vertices[i + 2] = new Vertex()
                {
                    Position = new Vector3D<float>(x + 1, y, 0),
                    Color = new Vector4D<float>(FR, FG, FB, FA),
                    BackColor = new Vector4D<float>(BR, BG, BB, BA),
                    TextureCoord = new Vector2D<float>(tcRight, tcBottom),
                };
                vertices[i + 3] = new Vertex()
                {
                    Position = new Vector3D<float>(x + 1, y + 1, 0),
                    Color = new Vector4D<float>(FR, FG, FB, FA),
                    BackColor = new Vector4D<float>(BR, BG, BB, BA),
                    TextureCoord = new Vector2D<float>(tcRight, tcTop),
                };
                i += 4;
            }
        }

        return vertices;
    }
    private uint[] GenerateIndices(Vertex[] vertices)
    {
        var indices = new uint[vertices.Length * 6];

        int i = 0;
        uint j = 0;
        for (int k = 0; k < vertices.Length; k++)
        {
            indices[i + 0] = j + 0; // Bottom Left
            indices[i + 1] = j + 3; // Top Right
            indices[i + 2] = j + 2; // Bottom Right
            i += 3;
            indices[i + 0] = j + 0; // Bottom Left
            indices[i + 1] = j + 1; // Top Left
            indices[i + 2] = j + 3; // Top Right
            i += 3;

            j += 4;
        }

        return indices;
    }

    private VulkanBuffer GenerateVertexBuffer(Vertex[] vertices)
    {
        VertexBufferBuilder
            .SetSize((ulong)(Unsafe.SizeOf<Vertex>() * vertices.Length));

        var vertexBuffer = VertexBufferBuilder
            .Build();

        vertexBuffer.Write<Vertex>(vertices);

        return vertexBuffer;
    }
    private VulkanBuffer GenerateIndexBuffer(uint[] indices)
    {
        IndexBufferBuilder
            .SetSize((ulong)(Unsafe.SizeOf<uint>() * indices.Length));

        var indexBuffer = IndexBufferBuilder
            .Build();

        indexBuffer.Write<uint>(indices);

        return indexBuffer;
    }

    private bool LookupTile(int tile, out float left, out float right, out float bottom, out float top, bool flipHorizontal = false, bool flipVertical = false) => LookupTile(tile % TilesPerRow, tile / TilesPerRow, out left, out right, out bottom, out top, flipHorizontal, flipVertical);
    private bool LookupTile(int x, int y, out float left, out float right, out float bottom, out float top, bool flipHorizontal = false, bool flipVertical = false)
    {
        var found = true;
        if (x < 0 ||
            y < 0 ||
            x >= TilesPerRow ||
            y >= TilesPerCol)
        {
            x = 0;
            y = 0;
            found = false;
        }


        left = ((float)(x * TEXTURE_TILE_WIDTH) / ImageWidth);
        right = ((float)((x + 1) * TEXTURE_TILE_WIDTH) / ImageWidth);
        bottom = ((float)(y * TEXTURE_TILE_HEIGHT) / ImageHeight);
        top = ((float)((y + 1) * TEXTURE_TILE_HEIGHT) / ImageHeight);

        if (flipHorizontal)
        {
            var swap = left;
            left = right;
            right = swap;
        }

        if (flipVertical)
        {
            var swap = bottom;
            bottom = top;
            top = swap;
        }

        return found;
    }
    #endregion

    #region New Texture Loading
    protected void LoadTexture(string textureFilename, int textureTileWidth, int textureTileHeight)
    {
        TEXTURE_FILENAME = textureFilename;
        TEXTURE_TILE_WIDTH = textureTileWidth;
        TEXTURE_TILE_HEIGHT = textureTileHeight;

        VulkanWindow.Size = new Vector2D<int>(ScreenWidth, ScreenHeight);

        VkInfo.VkFunc.DeviceWaitIdle(VulkanLogicalDevice);

        VulkanTexture.Dispose();
        for (int i = 0; i < VulkanUniformBuffers.Length; i++)
        {
            VulkanUniformBuffers[i].Dispose();
        }
        VulkanDescriptorPool.Dispose();
        VulkanCommandBuffers.Dispose();
        VulkanGraphicsPipeline.Dispose();
        VulkanRenderPass.Dispose();
        VulkanSwapchain.Dispose();

        VulkanSwapchain = new SwapchainBuilder(VulkanLogicalDevice, VulkanGraphicsQueue, VulkanPresentQueue)
            .AddDefaultFormats()
            .AddDefaultPresentModes()
            .Build();
        VulkanRenderPass = new RenderPassBuilder(VulkanSwapchain)
            .Build();
        VulkanSwapchain.GetFramebuffers(VulkanRenderPass);
        VulkanGraphicsPipeline = new GraphicsPipelineBuilder(VulkanRenderPass, VulkanDescriptorSetLayout)
            .AddVertexInput(Vertex.GetBindingDescription, Vertex.GetAttributeDescriptions)
            .AddShader(VulkanVertexShader)
            .AddShader(VulkanFragmentShader)
            .Build();
        VulkanDescriptorPool = new DescriptorPoolBuilder(VulkanDescriptorSetLayout)
            .SetMaxSets(VulkanSwapchain.ImageCount)
            .Build();
        VulkanDescriptorSets = new DescriptorSetsBuilder(VulkanDescriptorPool)
            .Build();
        VulkanCommandBuffers = new CommandBuffersBuilder(VulkanCommandPool)
            .SetCount((uint)VulkanSwapchain.Framebuffers!.Length)
            .Build();
        ImagesInFlight = new VulkanFence[VulkanSwapchain.ImageCount];

        var uniformBufferBuilder = new BufferBuilder(VulkanLogicalDevice)
            .SetBufferUsageFlags(BufferUsageFlags.BufferUsageUniformBufferBit)
            .SetMemoryPropertyFlags(MemoryPropertyFlags.MemoryPropertyDeviceLocalBit | MemoryPropertyFlags.MemoryPropertyHostVisibleBit)
            .SetSize((ulong)Unsafe.SizeOf<UniformBufferObject>());

        VulkanUniformBuffers = new VulkanBuffer[VulkanSwapchain.ImageCount];
        VulkanVertexBuffers = new VulkanBuffer[VulkanSwapchain.ImageCount];
        VulkanIndexBuffers = new VulkanBuffer[VulkanSwapchain.ImageCount];

        for (int i = 0; i < VulkanUniformBuffers.Length; i++)
        {
            VulkanUniformBuffers[i] = uniformBufferBuilder.Build();
        }

        VulkanTexture = new TextureBuilder(VulkanCommandPool)
            .SetFile(TEXTURE_FILENAME)
            .SetSamplerAnisotropyEnable(false)
            .SetSamplerMinFilter(Filter.Nearest)
            .SetSamplerMagFilter(Filter.Nearest)
            .Build();
        for (uint i = 0; i < VulkanSwapchain.ImageCount; i++)
        {
            UpdateDescriptorSets(i);
        }
    }
    private unsafe void UpdateDescriptorSets(uint imageIndex)
    {
        //VkInfo.VkFunc.ResetDescriptorPool(VulkanLogicalDevice, VulkanDescriptorPool, 0);
        //VulkanDescriptorSets = new DescriptorSetsBuilder(VulkanDescriptorPool).Build();

        var bufferInfo = new DescriptorBufferInfo()
        {
            Buffer = VulkanUniformBuffers[imageIndex],
            Offset = 0,
            Range = (ulong)Unsafe.SizeOf<UniformBufferObject>(),
        };

        var imageInfo = new DescriptorImageInfo()
        {
            ImageLayout = ImageLayout.ShaderReadOnlyOptimal,
            ImageView = VulkanTexture.ImageView,
            Sampler = VulkanTexture.ImageSampler,
        };


        var descriptorWrites = new WriteDescriptorSet[]
        {
            new WriteDescriptorSet()
            {
                SType = StructureType.WriteDescriptorSet,
                DstSet = VulkanDescriptorSets.DescriptorSets[imageIndex],
                DstBinding = 0,
                DstArrayElement = 0,
                DescriptorType = DescriptorType.UniformBuffer,
                DescriptorCount = 1,
                PBufferInfo = &bufferInfo,
            },
            new WriteDescriptorSet()
            {
                SType = StructureType.WriteDescriptorSet,
                DstSet = VulkanDescriptorSets.DescriptorSets[imageIndex],
                DstBinding = 1,
                DstArrayElement = 0,
                DescriptorType = DescriptorType.CombinedImageSampler,
                DescriptorCount = 1,
                PImageInfo = &imageInfo,
            }
        };

        VkInfo.VkFunc.UpdateDescriptorSets(VulkanLogicalDevice, descriptorWrites, null);
    }
    #endregion
    #endregion


#if DEBUG
    private unsafe uint OnDebugMessage(DebugUtilsMessageSeverityFlagsEXT messageSeverity, DebugUtilsMessageTypeFlagsEXT messageTypes, DebugUtilsMessengerCallbackDataEXT* pCallbackData, void* pUserData)
    {
        var message = Marshal.PtrToStringAnsi((nint)pCallbackData->PMessage);
        if (message != null)
        {
            Console.WriteLine($"{messageSeverity}: {messageTypes}: {message}");
        }

        return Vk.False;
    }
#endif

    #region IDisposable
    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            VkInfo.VkFunc.DeviceWaitIdle(VulkanLogicalDevice);

            VulkanTexture.Dispose();

            for (int i = 0; i < VulkanIndexBuffers.Length; i++)
            {
                VulkanIndexBuffers[i]?.Dispose();
            }
            for (int i = 0; i < VulkanVertexBuffers.Length; i++)
            {
                VulkanVertexBuffers[i]?.Dispose();
            }
            for (int i = 0; i < VulkanUniformBuffers.Length; i++)
            {
                VulkanUniformBuffers[i].Dispose();
            }
            for (int i = 0; i < InFlightFences.Length; i++)
            {
                InFlightFences[i].Dispose();
            }
            for (int i = 0; i < RenderFinishedSemaphores.Length; i++)
            {
                RenderFinishedSemaphores[i].Dispose();
            }
            for (int i = 0; i < ImageAvailableSemaphores.Length; i++)
            {
                ImageAvailableSemaphores[i].Dispose();
            }
            VulkanCommandBuffers.Dispose();
            VulkanCommandPool.Dispose();
            //VulkanDescriptorSets.Dispose();
            VulkanDescriptorPool.Dispose();
            VulkanGraphicsPipeline.Dispose();
            VulkanFragmentShader.Dispose();
            VulkanVertexShader.Dispose();
            VulkanDescriptorSetLayout.Dispose();
            VulkanRenderPass.Dispose();
            VulkanSwapchain.Dispose();
            VulkanLogicalDevice.Dispose();
            VulkanPhysicalDevice.Dispose();
            VulkanPresentQueue.Dispose();
            VulkanGraphicsQueue.Dispose();
            VulkanSurface.Dispose();
            VulkanInstance.Dispose();
            VulkanWindow.Dispose();
            disposedValue = true;
        }
    }

    
    ~GameWindow()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
=======
﻿using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Vulkan;
using Silk.NET.Windowing;

using VkSharp;

using Buffer = Silk.NET.Vulkan.Buffer;

namespace RLLib;


public abstract class GameWindow : Grid, IDisposable
{
    #region Protected
    /// <summary>
    /// Name of your app.
    /// </summary>
    protected abstract string NAME { get; }
    /// <summary>
    /// MajorVersion of your app.
    /// </summary>
    protected abstract uint VERSION_MAJOR { get; }
    /// <summary>
    /// MinorVersion of your app.
    /// </summary>
    protected abstract uint VERSION_MINOR { get; }
    /// <summary>
    /// PatchVersion of your app.
    /// </summary>
    protected abstract uint VERSION_PATCH { get; }

    /// <summary>
    /// Path to an image/texture file which will be loaded on creation.
    /// This will be updated if a new image is loaded through <see cref="LoadTexture(string, int, int)"/>
    /// </summary>
    protected abstract string TEXTURE_FILENAME { get; set; }
    /// <summary>
    /// Width of each tile. Used to lookup individual tiles from the loaded texture.
    /// This will be updated if a new image is loaded through <see cref="LoadTexture(string, int, int)"/>
    /// </summary>
    protected abstract int TEXTURE_TILE_WIDTH { get; set; }
    /// <summary>
    /// Height of each tile. Used to lookup individual tiles from the loaded texture.
    /// This will be updated if a new image is loaded through <see cref="LoadTexture(string, int, int)"/>
    /// </summary>
    protected abstract int TEXTURE_TILE_HEIGHT { get; set; }


    /// <summary>
    /// Called as the Window is created.
    /// </summary>
    protected virtual void OnWindowLoad() { }
    /// <summary>
    /// Called each frame before drawing the screen.
    /// </summary>
    /// <param name="deltaTime">The amount of time since the last frame.</param>
    protected virtual void OnWindowUpdate(double deltaTime) { }
    /// <summary>
    /// Called each frame after <see cref="OnWindowUpdate(double)"/> but before drawing the screen.
    /// </summary>
    /// <param name="deltaTime">The amount of time since the last frame.</param>
    protected virtual void OnWindowLateUpdate(double deltaTime) { }
    /// <summary>
    /// Called if the window state changes.
    /// </summary>
    /// <param name="windowState">The new window state.</param>
    protected virtual void OnWindowStateChanged(WindowState windowState) { }
    /// <summary>
    /// Called if the window loses/gains focus.
    /// </summary>
    /// <param name="hasFocus">TRUE if window now has focus.</param>
    protected virtual void OnWindowFocusChanged(bool hasFocus) { }
    /// <summary>
    /// Called if files are dropped onto the window.
    /// </summary>
    /// <param name="files">Array of filenames.</param>
    protected virtual void OnWindowFileDrop(string[] files) { }
    /// <summary>
    /// Called when the window closes.
    /// </summary>
    protected virtual void OnWindowClose() { }
    /// <summary>
    /// Called when a mouse button is pressed down.
    /// </summary>
    /// <param name="button">The mouse button pressed down.</param>
    protected virtual void OnMouseDown(MouseButton button) { }
    /// <summary>
    /// Called when a mouse button is unpressed.
    /// </summary>
    /// <param name="button">The mouse button unpressed.</param>
    protected virtual void OnMouseUp(MouseButton button) { }
    /// <summary>
    /// Called when a mouse button is clicked.
    /// </summary>
    /// <param name="button">The mouse button clicked.</param>
    /// <param name="x">The X tile coordinate.</param>
    /// <param name="y">The Y tile coordinate.</param>
    protected virtual void OnMouseClick(MouseButton button, int x, int y) { }
    /// <summary>
    /// Called when a mouse button is double clicked.
    /// </summary>
    /// <param name="button">The mouse button double clicked.</param>
    /// <param name="x">The X tile coordinate.</param>
    /// <param name="y">The Y tile coordinate.</param>
    protected virtual void OnMouseDoubleClick(MouseButton button, int x, int y) { }
    /// <summary>
    /// Called when the mouse moves.
    /// </summary>
    /// <param name="x">The X tile coordinate.</param>
    /// <param name="y">The Y tile coordinate.</param>
    protected virtual void OnMouseMove(int x, int y) { }
    /// <summary>
    /// Called when the mouse scroll wheel scrolls.
    /// </summary>
    /// <param name="scrollWheel">The mouse scroll wheel.</param>
    protected virtual void OnMouseScroll(ScrollWheel scrollWheel) { }
    /// <summary>
    /// Called when a key is pressed down.
    /// </summary>
    /// <param name="key">The key pressed down.</param>
    /// <param name="scancode">Number of repeats.</param>
    protected virtual void OnKeyDown(Key key, int scancode) { }
    /// <summary>
    /// Called when a key is unpressed.
    /// </summary>
    /// <param name="key">The key unpressed.</param>
    /// <param name="scancode">Number of repeats.</param>
    protected virtual void OnKeyUp(Key key, int scancode) { }
    /// <summary>
    /// Called when a key char is pressed.
    /// </summary>
    /// <param name="character">The key char.</param>
    protected virtual void OnKeyChar(char character) { }
    #endregion

    #region Public
    public unsafe GameWindow(int WINDOW_TILE_WIDTH, int WINDOW_TILE_HEIGHT) : base(WINDOW_TILE_WIDTH, WINDOW_TILE_HEIGHT)
    {
        VulkanWindow = new WindowBuilder()
            .SetTitle(NAME)
            .SetSize(ScreenWidth, ScreenHeight)
            .SetWindowBorder(WindowBorder.Fixed)
            .SetWindowState(WindowState.Normal)
            .AddActionOnWindowLoad(OnWindowLoad)
            .AddActionOnWindowUpdate(OnWindowUpdate)
            .AddActionOnWindowLateUpdate(OnWindowLateUpdate)
            .AddActionOnWindowRender(Draw)
            .AddActionOnWindowStateChanged(OnWindowStateChanged)
            .AddActionOnWindowFocusChanged(OnWindowFocusChanged)
            .AddActionOnWindowFileDrop(OnWindowFileDrop)
            .AddActionOnWindowClosing(OnWindowClose)
            .AddActionOnMouseDown(OnMouseDown)
            .AddActionOnMouseUp(OnMouseUp)
            .AddActionOnMouseClick((b, p) => OnMouseClick(b, (int)(p.X / TEXTURE_TILE_WIDTH), (int)(p.Y / TEXTURE_TILE_HEIGHT)))
            .AddActionOnMouseDoubleClick((b, p) => OnMouseDoubleClick(b, (int)(p.X / TEXTURE_TILE_WIDTH), (int)(p.Y / TEXTURE_TILE_HEIGHT)))
            .AddActionOnMouseMove((p) => OnMouseMove((int)(p.X / TEXTURE_TILE_WIDTH), (int)(p.Y / TEXTURE_TILE_HEIGHT)))
            .AddActionOnMouseScroll(OnMouseScroll)
            .AddActionOnKeyDown(OnKeyDown)
            .AddActionOnKeyUp(OnKeyUp)
            .AddActionOnKeyChar(OnKeyChar)
            .Build();
        VulkanInstance = new InstanceBuilder()
#if DEBUG
            .RequireValidationLayers()
            .SetDebugMessengerSeverity(DebugUtilsMessageSeverityFlagsEXT.DebugUtilsMessageSeverityWarningBitExt | DebugUtilsMessageSeverityFlagsEXT.DebugUtilsMessageSeverityErrorBitExt)
            .SetDebugMessengerType(DebugUtilsMessageTypeFlagsEXT.DebugUtilsMessageTypeGeneralBitExt | DebugUtilsMessageTypeFlagsEXT.DebugUtilsMessageTypeValidationBitExt | DebugUtilsMessageTypeFlagsEXT.DebugUtilsMessageTypePerformanceBitExt)
            .SetDebugCallback(OnDebugMessage)
#endif
            .SetWindow(VulkanWindow)
            .RequiredApiVersion(Vk.Version12)
            .SetAppName(NAME)
            .SetAppVersion(VERSION_MAJOR, VERSION_MINOR, VERSION_PATCH)
            .SetEngineName(ENGINE_NAME)
            .SetEngineVersion(ENGINE_VERSION_MAJOR, ENGINE_VERSION_MINOR, ENGINE_VERSION_PATCH)
            .Build();
        VulkanSurface = new SurfaceBuilder(VulkanInstance)
            .Build();
        var queueBuilder = new QueueBuilder();
        VulkanGraphicsQueue = queueBuilder
            .RequireGraphics()
            .Build();
        VulkanPresentQueue = queueBuilder.Reset()
            .RequirePresent()
            .Build();
        VulkanPhysicalDevice = new PhysicalDeviceBuilder(VulkanInstance, VulkanSurface)
            .SetPerferredDeviceType(PhysicalDeviceType.DiscreteGpu)
            .AddRequiredQueue(VulkanGraphicsQueue)
            .AddRequiredQueue(VulkanPresentQueue)
            .Build();
        VulkanLogicalDevice = new LogicalDeviceBuilder(VulkanPhysicalDevice)
            .Build();
        VulkanSwapchain = new SwapchainBuilder(VulkanLogicalDevice, VulkanGraphicsQueue, VulkanPresentQueue)
            .AddDefaultFormats()
            .AddDefaultPresentModes()
            .Build();
        VulkanRenderPass = new RenderPassBuilder(VulkanSwapchain)
            .Build();
        VulkanSwapchain.GetFramebuffers(VulkanRenderPass);
        VulkanDescriptorSetLayout = new DescriptorSetLayoutBuilder(VulkanLogicalDevice)
            .AddDescriptor(0, DescriptorType.UniformBuffer, ShaderStageFlags.ShaderStageVertexBit, VulkanSwapchain.ImageCount)
            .AddDescriptor(1, DescriptorType.CombinedImageSampler, ShaderStageFlags.ShaderStageFragmentBit, VulkanSwapchain.ImageCount)
            .Build();
        var shaderBuilder = new ShaderBuilder(VulkanLogicalDevice);
        VulkanVertexShader = shaderBuilder
            .SetShaderStageFlags(ShaderStageFlags.ShaderStageVertexBit)
            .SetFile(VERTEX_SHADER_FILENAME)
            .Build();
        VulkanFragmentShader = shaderBuilder.Reset()
            .SetShaderStageFlags(ShaderStageFlags.ShaderStageFragmentBit)
            .SetFile(FRAGMENT_SHADER_FILENAME)
            .Build();
        VulkanGraphicsPipeline = new GraphicsPipelineBuilder(VulkanRenderPass, VulkanDescriptorSetLayout)
            .AddVertexInput(Vertex.GetBindingDescription, Vertex.GetAttributeDescriptions)
            .AddShader(VulkanVertexShader)
            .AddShader(VulkanFragmentShader)
            .Build();
        VulkanCommandPool = new CommandPoolBuilder(VulkanLogicalDevice, VulkanGraphicsQueue)
            .SetCommandPoolCreateFlags(CommandPoolCreateFlags.CommandPoolCreateResetCommandBufferBit)
            .Build();
        VulkanCommandBuffers = new CommandBuffersBuilder(VulkanCommandPool)
            .SetCount((uint)VulkanSwapchain.Framebuffers!.Length)
            .Build();
        VulkanDescriptorPool = new DescriptorPoolBuilder(VulkanDescriptorSetLayout)
            .SetMaxSets(VulkanSwapchain.ImageCount)
            .Build();
        VulkanDescriptorSets = new DescriptorSetsBuilder(VulkanDescriptorPool)
            .Build();
        var semaphoreBuilder = new SemaphoreBuilder(VulkanLogicalDevice);
        var fenceBuilder = new FenceBuilder(VulkanLogicalDevice)
            .SetFenceCreateFlags(FenceCreateFlags.FenceCreateSignaledBit);
        ImageAvailableSemaphores = new VulkanSemaphore[MAX_FRAMES_IN_FLIGHT];
        RenderFinishedSemaphores = new VulkanSemaphore[MAX_FRAMES_IN_FLIGHT];
        InFlightFences = new VulkanFence[MAX_FRAMES_IN_FLIGHT];
        ImagesInFlight = new VulkanFence[VulkanSwapchain.ImageCount];
        for (int i = 0; i < MAX_FRAMES_IN_FLIGHT; i++)
        {
            ImageAvailableSemaphores[i] = semaphoreBuilder.Build();
            RenderFinishedSemaphores[i] = semaphoreBuilder.Build();
            InFlightFences[i] = fenceBuilder.Build();
        }

        var uniformBufferBuilder = new BufferBuilder(VulkanLogicalDevice)
            .SetBufferUsageFlags(BufferUsageFlags.BufferUsageUniformBufferBit)
            .SetMemoryPropertyFlags(MemoryPropertyFlags.MemoryPropertyDeviceLocalBit | MemoryPropertyFlags.MemoryPropertyHostVisibleBit)
            .SetSize((ulong)Unsafe.SizeOf<UniformBufferObject>());
        VertexBufferBuilder = new BufferBuilder(VulkanLogicalDevice)
            .SetBufferUsageFlags(BufferUsageFlags.BufferUsageTransferDstBit | BufferUsageFlags.BufferUsageVertexBufferBit)
            .SetMemoryPropertyFlags(MemoryPropertyFlags.MemoryPropertyDeviceLocalBit | MemoryPropertyFlags.MemoryPropertyHostVisibleBit);
        IndexBufferBuilder = new BufferBuilder(VulkanLogicalDevice)
            .SetBufferUsageFlags(BufferUsageFlags.BufferUsageTransferDstBit | BufferUsageFlags.BufferUsageIndexBufferBit)
            .SetMemoryPropertyFlags(MemoryPropertyFlags.MemoryPropertyDeviceLocalBit | MemoryPropertyFlags.MemoryPropertyHostVisibleBit);

        VulkanUniformBuffers = new VulkanBuffer[VulkanSwapchain.ImageCount];
        VulkanVertexBuffers = new VulkanBuffer[VulkanSwapchain.ImageCount];
        VulkanIndexBuffers = new VulkanBuffer[VulkanSwapchain.ImageCount];

        for (int i = 0; i < VulkanUniformBuffers.Length; i++)
        {
            VulkanUniformBuffers[i] = uniformBufferBuilder.Build();
        }
        VulkanTexture = new TextureBuilder(VulkanCommandPool)
            .SetFile(TEXTURE_FILENAME)
            .SetSamplerAnisotropyEnable(false)
            .SetSamplerMinFilter(Filter.Nearest)
            .SetSamplerMagFilter(Filter.Nearest)
            .Build();
        for (uint i = 0; i < VulkanSwapchain.ImageCount; i++)
        {
            UpdateDescriptorSets(i);
        }

        Camera = new OrthographicCamera(Width / 2, Height / 2, Width, Height);
    }

    public void Run() => VulkanWindow.Run();

    public void Close() => VulkanWindow.Close();
    #endregion

    #region private
    private const string VERTEX_SHADER_FILENAME = "VertexShader.spv";
    private const string FRAGMENT_SHADER_FILENAME = "FragmentShader.spv";

    private const string ENGINE_NAME = "RLLib";
    private const uint ENGINE_VERSION_MAJOR = 0;
    private const uint ENGINE_VERSION_MINOR = 1;
    private const uint ENGINE_VERSION_PATCH = 0;

    private const int MAX_FRAMES_IN_FLIGHT = 2;

    private VulkanWindow VulkanWindow;
    private VulkanInstance VulkanInstance;
    private VulkanSurface VulkanSurface;
    private VulkanQueue VulkanGraphicsQueue;
    private VulkanQueue VulkanPresentQueue;
    private VulkanPhysicalDevice VulkanPhysicalDevice;
    private VulkanLogicalDevice VulkanLogicalDevice;
    private VulkanSwapchain VulkanSwapchain;
    private VulkanRenderPass VulkanRenderPass;
    private VulkanDescriptorSetLayout VulkanDescriptorSetLayout;
    private VulkanShader VulkanVertexShader;
    private VulkanShader VulkanFragmentShader;
    private VulkanGraphicsPipeline VulkanGraphicsPipeline;
    private VulkanDescriptorPool VulkanDescriptorPool;
    private VulkanDescriptorSets VulkanDescriptorSets;
    private VulkanCommandPool VulkanCommandPool;
    private VulkanCommandBuffers VulkanCommandBuffers;
    private VulkanSemaphore[] ImageAvailableSemaphores;
    private VulkanSemaphore[] RenderFinishedSemaphores;
    private VulkanFence[] InFlightFences;
    private VulkanFence[] ImagesInFlight;

    private VulkanBuffer[] VulkanUniformBuffers;
    private BufferBuilder VertexBufferBuilder;
    private VulkanBuffer?[] VulkanVertexBuffers;
    private BufferBuilder IndexBufferBuilder;
    private VulkanBuffer?[] VulkanIndexBuffers;

    private VulkanTexture VulkanTexture;
    private OrthographicCamera Camera;

    private int ImageWidth => VulkanTexture.Width;
    private int ImageHeight => VulkanTexture.Height;
    private int TilesPerRow => ImageWidth / TEXTURE_TILE_WIDTH;
    private int TilesPerCol => ImageHeight / TEXTURE_TILE_HEIGHT;

    private int ScreenWidth => Width * TEXTURE_TILE_WIDTH;
    private int ScreenHeight => Height * TEXTURE_TILE_HEIGHT;

    private int CurrentFrame = 0;

    private unsafe void Draw(double deltaTime)
    {
        VkInfo.VkFunc.WaitForFences(VulkanLogicalDevice, new[] { InFlightFences[CurrentFrame].Fence }, true, ulong.MaxValue);

        uint imageIndex = 0;
        var result = VulkanSwapchain.KhrSwapchain.AcquireNextImage(VulkanLogicalDevice, VulkanSwapchain, ulong.MaxValue, ImageAvailableSemaphores[CurrentFrame], default, ref imageIndex);

        if (result != Result.Success)
        {
            throw new Exception($"Failed to acquire swap chain image.");
        }

        UpdateFromGrid(imageIndex);

        if (ImagesInFlight[imageIndex] != null &&
            ImagesInFlight[imageIndex].Fence.Handle != default)
        {
            VkInfo.VkFunc.WaitForFences(VulkanLogicalDevice, new[] { ImagesInFlight[imageIndex].Fence }, true, ulong.MaxValue);
        }
        ImagesInFlight[imageIndex] = InFlightFences[CurrentFrame];

        var submitInfo = new SubmitInfo()
        {
            SType = StructureType.SubmitInfo,
        };

        var waitSemaphores = stackalloc[] { ImageAvailableSemaphores[CurrentFrame].Semaphore };
        var waitStages = stackalloc[] { PipelineStageFlags.PipelineStageColorAttachmentOutputBit };

        var buffer = VulkanCommandBuffers[imageIndex].CommandBuffer;

        submitInfo = submitInfo with
        {
            WaitSemaphoreCount = 1,
            PWaitSemaphores = waitSemaphores,
            PWaitDstStageMask = waitStages,

            CommandBufferCount = 1,
            PCommandBuffers = &buffer
        };

        var signalSemaphores = stackalloc[] { RenderFinishedSemaphores![CurrentFrame].Semaphore };
        submitInfo = submitInfo with
        {
            SignalSemaphoreCount = 1,
            PSignalSemaphores = signalSemaphores,
        };

        VkInfo.VkFunc.ResetFences(VulkanLogicalDevice, InFlightFences[CurrentFrame]);

        if (VkInfo.VkFunc.QueueSubmit(VulkanGraphicsQueue, InFlightFences[CurrentFrame], submitInfo) != Result.Success)
        {
            throw new Exception("Failed to submit draw command buffer!");
        }

        var swapChains = stackalloc[] { VulkanSwapchain.SwapchainKHR };
        var presentInfo = new PresentInfoKHR()
        {
            SType = StructureType.PresentInfoKhr,

            WaitSemaphoreCount = 1,
            PWaitSemaphores = signalSemaphores,

            SwapchainCount = 1,
            PSwapchains = swapChains,

            PImageIndices = &imageIndex
        };

        result = VulkanSwapchain.KhrSwapchain.QueuePresent(VulkanPresentQueue, presentInfo);

        if (result != Result.Success)
        {
            throw new Exception($"Failed to present swapchain image.");
        }

        CurrentFrame = (CurrentFrame + 1) % MAX_FRAMES_IN_FLIGHT;
    }

    #region Update
    private void UpdateFromGrid(uint imageIndex)
    {
        UpdateUniformBuffer(imageIndex);

        var vertices = GenerateVertices();
        var indices = GenerateIndices(vertices);

        VulkanVertexBuffers[imageIndex]?.Dispose();
        VulkanVertexBuffers[imageIndex] = GenerateVertexBuffer(vertices);
        VulkanIndexBuffers[imageIndex]?.Dispose();
        VulkanIndexBuffers[imageIndex] = GenerateIndexBuffer(indices);
        UpdateCommandBuffer(imageIndex, VulkanVertexBuffers[imageIndex]!, VulkanIndexBuffers[imageIndex]!, (uint)indices.Length);
    }

    private unsafe void UpdateUniformBuffer(uint imageIndex)
    {
        var ubo = new UniformBufferObject()
        {
            Model = Matrix4X4<float>.Identity,
            View = Camera.ViewMatrix,
            Projection = Camera.ProjectionMatrix,
        };

        VulkanUniformBuffers[imageIndex].Write<UniformBufferObject>(new[] { ubo });
    }
    private unsafe void UpdateCommandBuffer(uint imageIndex, VulkanBuffer vertexBuffer, VulkanBuffer indexBuffer, uint indicesLength)
    {
        var commandBuffer = VulkanCommandBuffers[imageIndex];

        commandBuffer.ResetCommandBuffer(0);

        var beginInfo = new CommandBufferBeginInfo()
        {
            SType = StructureType.CommandBufferBeginInfo,
        };

        if (commandBuffer.BeginCommandBuffer(beginInfo) != Result.Success)
        {
            throw new Exception("Failed to begin recording command buffer!");
        }

        var renderPassInfo = new RenderPassBeginInfo()
        {
            SType = StructureType.RenderPassBeginInfo,
            RenderPass = VulkanRenderPass,
            Framebuffer = VulkanSwapchain.Framebuffers![imageIndex],
            RenderArea =
                {
                    Offset = { X = 0, Y = 0 },
                    Extent = VulkanSwapchain.Extent,
                }
        };

        var clearColor = new ClearValue()
        {
            Color = new() { Float32_0 = 0, Float32_1 = 0, Float32_2 = 0, Float32_3 = 1 },
        };

        renderPassInfo.ClearValueCount = 1;
        renderPassInfo.PClearValues = &clearColor;

        commandBuffer.CmdBeginRenderPass(renderPassInfo, SubpassContents.Inline);

        commandBuffer.CmdBindPipeline(PipelineBindPoint.Graphics, VulkanGraphicsPipeline);

        var vertexBuffers = new Buffer[] { vertexBuffer };
        var offsets = new ulong[] { 0 };

        commandBuffer.CmdBindVertexBuffers(0, vertexBuffers, offsets);

        commandBuffer.CmdBindIndexBuffer(indexBuffer, 0, IndexType.Uint32);

        commandBuffer.CmdBindDescriptorSets(PipelineBindPoint.Graphics, VulkanGraphicsPipeline.PipelineLayout, 0, new[] { VulkanDescriptorSets.DescriptorSets[imageIndex] }, null);

        commandBuffer.CmdDrawIndexed(indicesLength, 1, 0, 0, 0);


        commandBuffer.CmdEndRenderPass();

        if (commandBuffer.EndCommandBuffer() != Result.Success)
        {
            throw new Exception("Failed to record command buffer!");
        }
    }

    private Vertex[] GenerateVertices()
    {
        var vertices = new Vertex[Width * Height * 4];

        int i = 0;
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var cell = Cells[x, y];

                var FR = cell.Color.R;
                var FG = cell.Color.G;
                var FB = cell.Color.B;
                var FA = cell.Color.A;

                var BR = cell.BackColor.R;
                var BG = cell.BackColor.G;
                var BB = cell.BackColor.B;
                var BA = cell.BackColor.A;

                var flipHorizontally = (cell.Flipped & GridCell.Flip.Horizontally) != 0;
                var flipVertically = (cell.Flipped & GridCell.Flip.Vertically) != 0;

                float tcLeft;
                float tcRight;
                float tcBottom;
                float tcTop;

                switch (cell.CellDisplayType)
                {
                    case GridCell.DisplayType.XY:
                        if (!LookupTile(cell.X, cell.Y, out tcLeft, out tcRight, out tcBottom, out tcTop, flipHorizontally, flipVertically))
                        {
                            Console.WriteLine($"Invalid cell({cell.X}, {cell.Y}).");
                        }
                        break;
                    default:
                    case GridCell.DisplayType.Character:
                        if (!LookupTile(cell.Character, out tcLeft, out tcRight, out tcBottom, out tcTop, flipHorizontally, flipVertically))
                        {
                            Console.WriteLine($"Invalid character({cell.Character}).");
                        }
                        break;
                }

                vertices[i + 0] = new Vertex()
                {
                    Position = new Vector3D<float>(x, y, 0),
                    Color = new Vector4D<float>(FR, FG, FB, FA),
                    BackColor = new Vector4D<float>(BR, BG, BB, BA),
                    TextureCoord = new Vector2D<float>(tcLeft, tcBottom),
                };
                vertices[i + 1] = new Vertex()
                {
                    Position = new Vector3D<float>(x, y + 1, 0),
                    Color = new Vector4D<float>(FR, FG, FB, FA),
                    BackColor = new Vector4D<float>(BR, BG, BB, BA),
                    TextureCoord = new Vector2D<float>(tcLeft, tcTop),
                };
                vertices[i + 2] = new Vertex()
                {
                    Position = new Vector3D<float>(x + 1, y, 0),
                    Color = new Vector4D<float>(FR, FG, FB, FA),
                    BackColor = new Vector4D<float>(BR, BG, BB, BA),
                    TextureCoord = new Vector2D<float>(tcRight, tcBottom),
                };
                vertices[i + 3] = new Vertex()
                {
                    Position = new Vector3D<float>(x + 1, y + 1, 0),
                    Color = new Vector4D<float>(FR, FG, FB, FA),
                    BackColor = new Vector4D<float>(BR, BG, BB, BA),
                    TextureCoord = new Vector2D<float>(tcRight, tcTop),
                };
                i += 4;
            }
        }

        return vertices;
    }
    private uint[] GenerateIndices(Vertex[] vertices)
    {
        var indices = new uint[vertices.Length * 6];

        int i = 0;
        uint j = 0;
        for (int k = 0; k < vertices.Length; k++)
        {
            indices[i + 0] = j + 0; // Bottom Left
            indices[i + 1] = j + 3; // Top Right
            indices[i + 2] = j + 2; // Bottom Right
            i += 3;
            indices[i + 0] = j + 0; // Bottom Left
            indices[i + 1] = j + 1; // Top Left
            indices[i + 2] = j + 3; // Top Right
            i += 3;

            j += 4;
        }

        return indices;
    }

    private VulkanBuffer GenerateVertexBuffer(Vertex[] vertices)
    {
        VertexBufferBuilder
            .SetSize((ulong)(Unsafe.SizeOf<Vertex>() * vertices.Length));

        var vertexBuffer = VertexBufferBuilder
            .Build();

        vertexBuffer.Write<Vertex>(vertices);

        return vertexBuffer;
    }
    private VulkanBuffer GenerateIndexBuffer(uint[] indices)
    {
        IndexBufferBuilder
            .SetSize((ulong)(Unsafe.SizeOf<uint>() * indices.Length));

        var indexBuffer = IndexBufferBuilder
            .Build();

        indexBuffer.Write<uint>(indices);

        return indexBuffer;
    }

    private bool LookupTile(int tile, out float left, out float right, out float bottom, out float top, bool flipHorizontal = false, bool flipVertical = false) => LookupTile(tile % TilesPerRow, tile / TilesPerRow, out left, out right, out bottom, out top, flipHorizontal, flipVertical);
    private bool LookupTile(int x, int y, out float left, out float right, out float bottom, out float top, bool flipHorizontal = false, bool flipVertical = false)
    {
        var found = true;
        if (x < 0 ||
            y < 0 ||
            x >= TilesPerRow ||
            y >= TilesPerCol)
        {
            x = 0;
            y = 0;
            found = false;
        }


        left = ((float)(x * TEXTURE_TILE_WIDTH) / ImageWidth);
        right = ((float)((x + 1) * TEXTURE_TILE_WIDTH) / ImageWidth);
        bottom = ((float)(y * TEXTURE_TILE_HEIGHT) / ImageHeight);
        top = ((float)((y + 1) * TEXTURE_TILE_HEIGHT) / ImageHeight);

        if (flipHorizontal)
        {
            var swap = left;
            left = right;
            right = swap;
        }

        if (flipVertical)
        {
            var swap = bottom;
            bottom = top;
            top = swap;
        }

        return found;
    }
    #endregion

    #region New Texture Loading
    protected void LoadTexture(string textureFilename, int textureTileWidth, int textureTileHeight)
    {
        TEXTURE_FILENAME = textureFilename;
        TEXTURE_TILE_WIDTH = textureTileWidth;
        TEXTURE_TILE_HEIGHT = textureTileHeight;

        VulkanWindow.Size = new Vector2D<int>(ScreenWidth, ScreenHeight);

        VkInfo.VkFunc.DeviceWaitIdle(VulkanLogicalDevice);

        VulkanTexture.Dispose();
        for (int i = 0; i < VulkanUniformBuffers.Length; i++)
        {
            VulkanUniformBuffers[i].Dispose();
        }
        VulkanDescriptorPool.Dispose();
        VulkanCommandBuffers.Dispose();
        VulkanGraphicsPipeline.Dispose();
        VulkanRenderPass.Dispose();
        VulkanSwapchain.Dispose();

        VulkanSwapchain = new SwapchainBuilder(VulkanLogicalDevice, VulkanGraphicsQueue, VulkanPresentQueue)
            .AddDefaultFormats()
            .AddDefaultPresentModes()
            .Build();
        VulkanRenderPass = new RenderPassBuilder(VulkanSwapchain)
            .Build();
        VulkanSwapchain.GetFramebuffers(VulkanRenderPass);
        VulkanGraphicsPipeline = new GraphicsPipelineBuilder(VulkanRenderPass, VulkanDescriptorSetLayout)
            .AddVertexInput(Vertex.GetBindingDescription, Vertex.GetAttributeDescriptions)
            .AddShader(VulkanVertexShader)
            .AddShader(VulkanFragmentShader)
            .Build();
        VulkanDescriptorPool = new DescriptorPoolBuilder(VulkanDescriptorSetLayout)
            .SetMaxSets(VulkanSwapchain.ImageCount)
            .Build();
        VulkanDescriptorSets = new DescriptorSetsBuilder(VulkanDescriptorPool)
            .Build();
        VulkanCommandBuffers = new CommandBuffersBuilder(VulkanCommandPool)
            .SetCount((uint)VulkanSwapchain.Framebuffers!.Length)
            .Build();
        ImagesInFlight = new VulkanFence[VulkanSwapchain.ImageCount];

        var uniformBufferBuilder = new BufferBuilder(VulkanLogicalDevice)
            .SetBufferUsageFlags(BufferUsageFlags.BufferUsageUniformBufferBit)
            .SetMemoryPropertyFlags(MemoryPropertyFlags.MemoryPropertyDeviceLocalBit | MemoryPropertyFlags.MemoryPropertyHostVisibleBit)
            .SetSize((ulong)Unsafe.SizeOf<UniformBufferObject>());

        VulkanUniformBuffers = new VulkanBuffer[VulkanSwapchain.ImageCount];
        VulkanVertexBuffers = new VulkanBuffer[VulkanSwapchain.ImageCount];
        VulkanIndexBuffers = new VulkanBuffer[VulkanSwapchain.ImageCount];

        for (int i = 0; i < VulkanUniformBuffers.Length; i++)
        {
            VulkanUniformBuffers[i] = uniformBufferBuilder.Build();
        }

        VulkanTexture = new TextureBuilder(VulkanCommandPool)
            .SetFile(TEXTURE_FILENAME)
            .SetSamplerAnisotropyEnable(false)
            .SetSamplerMinFilter(Filter.Nearest)
            .SetSamplerMagFilter(Filter.Nearest)
            .Build();
        for (uint i = 0; i < VulkanSwapchain.ImageCount; i++)
        {
            UpdateDescriptorSets(i);
        }
    }
    private unsafe void UpdateDescriptorSets(uint imageIndex)
    {
        //VkInfo.VkFunc.ResetDescriptorPool(VulkanLogicalDevice, VulkanDescriptorPool, 0);
        //VulkanDescriptorSets = new DescriptorSetsBuilder(VulkanDescriptorPool).Build();

        var bufferInfo = new DescriptorBufferInfo()
        {
            Buffer = VulkanUniformBuffers[imageIndex],
            Offset = 0,
            Range = (ulong)Unsafe.SizeOf<UniformBufferObject>(),
        };

        var imageInfo = new DescriptorImageInfo()
        {
            ImageLayout = ImageLayout.ShaderReadOnlyOptimal,
            ImageView = VulkanTexture.ImageView,
            Sampler = VulkanTexture.ImageSampler,
        };


        var descriptorWrites = new WriteDescriptorSet[]
        {
            new WriteDescriptorSet()
            {
                SType = StructureType.WriteDescriptorSet,
                DstSet = VulkanDescriptorSets.DescriptorSets[imageIndex],
                DstBinding = 0,
                DstArrayElement = 0,
                DescriptorType = DescriptorType.UniformBuffer,
                DescriptorCount = 1,
                PBufferInfo = &bufferInfo,
            },
            new WriteDescriptorSet()
            {
                SType = StructureType.WriteDescriptorSet,
                DstSet = VulkanDescriptorSets.DescriptorSets[imageIndex],
                DstBinding = 1,
                DstArrayElement = 0,
                DescriptorType = DescriptorType.CombinedImageSampler,
                DescriptorCount = 1,
                PImageInfo = &imageInfo,
            }
        };

        VkInfo.VkFunc.UpdateDescriptorSets(VulkanLogicalDevice, descriptorWrites, null);
    }
    #endregion
    #endregion


#if DEBUG
    private unsafe uint OnDebugMessage(DebugUtilsMessageSeverityFlagsEXT messageSeverity, DebugUtilsMessageTypeFlagsEXT messageTypes, DebugUtilsMessengerCallbackDataEXT* pCallbackData, void* pUserData)
    {
        var message = Marshal.PtrToStringAnsi((nint)pCallbackData->PMessage);
        if (message != null)
        {
            Console.WriteLine($"{messageSeverity}: {messageTypes}: {message}");
        }

        return Vk.False;
    }
#endif

    #region IDisposable
    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            VkInfo.VkFunc.DeviceWaitIdle(VulkanLogicalDevice);

            VulkanTexture.Dispose();

            for (int i = 0; i < VulkanIndexBuffers.Length; i++)
            {
                VulkanIndexBuffers[i]?.Dispose();
            }
            for (int i = 0; i < VulkanVertexBuffers.Length; i++)
            {
                VulkanVertexBuffers[i]?.Dispose();
            }
            for (int i = 0; i < VulkanUniformBuffers.Length; i++)
            {
                VulkanUniformBuffers[i].Dispose();
            }
            for (int i = 0; i < InFlightFences.Length; i++)
            {
                InFlightFences[i].Dispose();
            }
            for (int i = 0; i < RenderFinishedSemaphores.Length; i++)
            {
                RenderFinishedSemaphores[i].Dispose();
            }
            for (int i = 0; i < ImageAvailableSemaphores.Length; i++)
            {
                ImageAvailableSemaphores[i].Dispose();
            }
            VulkanCommandBuffers.Dispose();
            VulkanCommandPool.Dispose();
            //VulkanDescriptorSets.Dispose();
            VulkanDescriptorPool.Dispose();
            VulkanGraphicsPipeline.Dispose();
            VulkanFragmentShader.Dispose();
            VulkanVertexShader.Dispose();
            VulkanDescriptorSetLayout.Dispose();
            VulkanRenderPass.Dispose();
            VulkanSwapchain.Dispose();
            VulkanLogicalDevice.Dispose();
            VulkanPhysicalDevice.Dispose();
            VulkanPresentQueue.Dispose();
            VulkanGraphicsQueue.Dispose();
            VulkanSurface.Dispose();
            VulkanInstance.Dispose();
            VulkanWindow.Dispose();
            disposedValue = true;
        }
    }

    
    ~GameWindow()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
>>>>>>> main
