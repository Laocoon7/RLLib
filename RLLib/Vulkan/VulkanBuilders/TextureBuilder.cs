using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Silk.NET.Vulkan;
using System.Runtime.CompilerServices;
using RLLib.VulkanInfo;

namespace RLLib.VulkanBuilders;

public class TextureBuilder
{
    private BuilderSettings Settings;

    public TextureBuilder(VulkanCommandPool vulkanCommandPool)
    {
        Settings = new BuilderSettings(vulkanCommandPool);
    }

    public VulkanTexture Build() => new VulkanTexture(Settings);

    #region Builder
    public TextureBuilder SetFile(string filepath)
    {
        Settings.TextureFile = new FileInfo(filepath);
        return this;
    }
    public TextureBuilder SetFile(FileInfo file)
    {
        Settings.TextureFile = file;
        return this;
    }

    public TextureBuilder SetMipLevels(uint count)
    {
        Settings.MipLevels = count;
        return this;
    }

    public TextureBuilder SetImageTiling(ImageTiling imageTiling)
    {
        Settings.ImageTiling = imageTiling;
        return this;
    }

    public TextureBuilder SetSampleCount(SampleCountFlags flags)
    {
        Settings.SampleCountFlags = flags;
        return this;
    }

    public TextureBuilder SetSamplerMinFilter(Filter filter)
    {
        Settings.SamplerMinFilter = filter;
        return this;
    }

    public TextureBuilder SetSamplerMagFilter(Filter filter)
    {
        Settings.SamplerMagFilter = filter;
        return this;
    }

    public TextureBuilder SetSamplerAddressModeU(SamplerAddressMode mode)
    {
        Settings.SamplerAddressModeU = mode;
        return this;
    }
    public TextureBuilder SetSamplerAddressModeV(SamplerAddressMode mode)
    {
        Settings.SamplerAddressModeV = mode;
        return this;
    }
    public TextureBuilder SetSamplerAddressModeW(SamplerAddressMode mode)
    {
        Settings.SamplerAddressModeW = mode;
        return this;
    }

    public TextureBuilder SetSamplerAnisotropyEnable(bool enable = true)
    {
        Settings.SamplerAnisotropyEnable = enable;
        return this;
    }

    public TextureBuilder SetSamplerBorderColor(BorderColor borderColor)
    {
        Settings.SamplerBorderColor = borderColor;
        return this;
    }

    public TextureBuilder SetSamplerCompareEnable(bool enable = true)
    {
        Settings.SamplerCompareEnable = enable;
        return this;
    }

    public TextureBuilder SetSamplerCompareOp(CompareOp compareOp)
    {
        Settings.SamplerCompareOp = compareOp;
        return this;
    }

    public TextureBuilder SetSamplerMipmapMode(SamplerMipmapMode mode)
    {
        Settings.SamplerMipmapMode = mode;
        return this;
    }
    #endregion

    public TextureBuilder Reset(VulkanCommandPool vulkanCommandPool)
    {
        Settings.Reset(vulkanCommandPool);
        return this;
    }
    public TextureBuilder Reset()
    {
        Settings.Reset();
        return this;
    }

    internal class BuilderSettings
    {
        public VulkanCommandPool VulkanCommandPool { get; set; }

        public FileInfo? TextureFile { get; set; }

        public uint MipLevels { get; set; } = 1;
        public ImageTiling ImageTiling { get; set; } = ImageTiling.Optimal;
        public SampleCountFlags SampleCountFlags { get; set; } = SampleCountFlags.SampleCount1Bit;

        public Filter SamplerMinFilter { get; set; } = Filter.Linear;
        public Filter SamplerMagFilter { get; set; } = Filter.Linear;

        public SamplerAddressMode SamplerAddressModeU { get; set; } = SamplerAddressMode.Repeat;
        public SamplerAddressMode SamplerAddressModeV { get; set; } = SamplerAddressMode.Repeat;
        public SamplerAddressMode SamplerAddressModeW { get; set; } = SamplerAddressMode.Repeat;

        public bool SamplerAnisotropyEnable { get; set; } = true;

        public BorderColor SamplerBorderColor { get; set; } = BorderColor.IntOpaqueBlack;

        public bool SamplerCompareEnable { get; set; } = false;
        public CompareOp SamplerCompareOp { get; set; } = CompareOp.Always;

        public SamplerMipmapMode SamplerMipmapMode { get; set; } = SamplerMipmapMode.Linear;

        public BuilderSettings(VulkanCommandPool vulkanCommandPool)
        {
            VulkanCommandPool = vulkanCommandPool;
        }

        public void Reset(VulkanCommandPool vulkanCommandPool)
        {
            VulkanCommandPool = vulkanCommandPool;

            Reset();
        }

        public void Reset()
        {
            TextureFile = null;

            MipLevels = 1;
            ImageTiling = ImageTiling.Optimal;
            SampleCountFlags = SampleCountFlags.SampleCount1Bit;

            SamplerMinFilter = Filter.Linear;
            SamplerMagFilter = Filter.Linear;

            SamplerAddressModeU = SamplerAddressMode.Repeat;
            SamplerAddressModeV = SamplerAddressMode.Repeat;
            SamplerAddressModeW = SamplerAddressMode.Repeat;

            SamplerAnisotropyEnable = true;

            SamplerBorderColor = BorderColor.IntOpaqueBlack;

            SamplerCompareEnable = false;
            SamplerCompareOp = CompareOp.Always;

            SamplerMipmapMode = SamplerMipmapMode.Linear;
        }
    }
}

public class VulkanTexture : VkObject
{
    internal VulkanPhysicalDevice VulkanPhysicalDevice => VulkanLogicalDevice.VulkanPhysicalDevice;
    internal VulkanLogicalDevice VulkanLogicalDevice => VulkanCommandPool.VulkanLogicalDevice;
    internal VulkanQueue GraphicsQueue => VulkanCommandPool.VulkanQueue;
    internal VulkanCommandPool VulkanCommandPool { get; init; }

    public string Filepath { get; init; }
    public Image<Rgba32> TextureImage { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }
    public Silk.NET.Vulkan.Image Image { get; init; }
    public DeviceMemory ImageMemory { get; init; }
    public ImageView ImageView { get; init; }
    public Sampler ImageSampler { get; init; }

    internal unsafe VulkanTexture(TextureBuilder.BuilderSettings settings)
    {
        if (settings.TextureFile == null)
        {
            throw new Exception("Failed to create vulkan texture. No file provided.");
        }
        else if (!File.Exists(settings.TextureFile.FullName))
        {
            throw new FileNotFoundException(settings.TextureFile.FullName);
        }

        VulkanCommandPool = settings.VulkanCommandPool;

        Filepath = settings.TextureFile.FullName;

        // Open the texture image
        TextureImage = (Image<Rgba32>)SixLabors.ImageSharp.Image.Load(Filepath);

        if (!TextureImage.DangerousTryGetSinglePixelMemory(out Memory<Rgba32> memory))
        {
            throw new Exception("Failed to load texture image! - Make sure to initialize MemoryAllocator.Default!");
        }

        using (System.Buffers.MemoryHandle hMemory = memory.Pin())
        {
            // Set width / height
            Width = TextureImage.Width;
            Height = TextureImage.Height;

            var pixels = new Span<Rgba32>(hMemory.Pointer, Width * Height);

            // Create staging buffer.
            ulong imageLength = (ulong)(pixels.Length * Unsafe.SizeOf<Rgba32>());

            using (var stagingBuffer = new BufferBuilder(VulkanLogicalDevice)
                .SetBufferUsageFlags(BufferUsageFlags.BufferUsageTransferSrcBit)
                .SetMemoryPropertyFlags(MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit)
                .SetSize(imageLength)
                .Build())
            {

                // Write texture image data to staging buffer
                stagingBuffer.Write(pixels);

                // Create image
                var imageInfo = new ImageCreateInfo()
                {
                    SType = StructureType.ImageCreateInfo,
                    ImageType = ImageType.ImageType2D,
                    Extent =
            {
                Width = (uint)Width,
                Height = (uint)Height,
                Depth = 1,
            },
                    MipLevels = settings.MipLevels,
                    ArrayLayers = 1,
                    Format = Format.R8G8B8A8Srgb,
                    Tiling = settings.ImageTiling,
                    InitialLayout = ImageLayout.Undefined,
                    Usage = ImageUsageFlags.ImageUsageTransferDstBit | ImageUsageFlags.ImageUsageSampledBit,
                    Samples = settings.SampleCountFlags,
                    SharingMode = SharingMode.Exclusive,
                };

                Image = VkFunc.CreateImage(VulkanLogicalDevice, imageInfo, null);

                // Allocate image memory.
                VkFunc.GetImageMemoryRequirements(VulkanLogicalDevice, Image, out var memRequirements);

                var allocInfo = new MemoryAllocateInfo()
                {
                    SType = StructureType.MemoryAllocateInfo,
                    AllocationSize = memRequirements.Size,
                    MemoryTypeIndex = VkUtility.FindMemoryType(VulkanPhysicalDevice, memRequirements.MemoryTypeBits, MemoryPropertyFlags.MemoryPropertyDeviceLocalBit),
                };

                if (VkFunc.AllocateMemory(VulkanLogicalDevice, allocInfo, null, out var imageMemory) != Result.Success)
                {
                    throw new Exception("Failed to allocate device memory.");
                }
                ImageMemory = imageMemory;

                // Bind image / image memory.
                VkFunc.BindImageMemory(VulkanLogicalDevice, Image, ImageMemory, 0);

                TransitionImageLayout(Image, Format.R8G8B8A8Srgb, ImageLayout.Undefined, ImageLayout.TransferDstOptimal);

                // Copy buffer to image
                var commandBuffer = new VulkanSingleTimeCommandBuffer(VulkanCommandPool);

                var region = new BufferImageCopy()
                {
                    BufferOffset = 0,
                    BufferRowLength = 0,
                    BufferImageHeight = 0,
                    ImageSubresource =
            {
                AspectMask = ImageAspectFlags.ImageAspectColorBit,
                MipLevel = 0,
                BaseArrayLayer = 0,
                LayerCount = 1,
            },
                    ImageOffset = new Offset3D(0, 0, 0),
                    ImageExtent = new Extent3D((uint)Width, (uint)Height, 1),

                };

                commandBuffer.CmdCopyBufferToImage(stagingBuffer, Image, ImageLayout.TransferDstOptimal, 1, region);

                commandBuffer.End();

                TransitionImageLayout(Image, Format.R8G8B8A8Srgb, ImageLayout.TransferDstOptimal, ImageLayout.ShaderReadOnlyOptimal);
            }

            // Create image view
            var createInfo = new ImageViewCreateInfo()
            {
                SType = StructureType.ImageViewCreateInfo,
                Image = Image,
                ViewType = ImageViewType.ImageViewType2D,
                Format = Format.R8G8B8A8Srgb,
                //Components =
                //    {
                //        R = ComponentSwizzle.Identity,
                //        G = ComponentSwizzle.Identity,
                //        B = ComponentSwizzle.Identity,
                //        A = ComponentSwizzle.Identity,
                //    },
                SubresourceRange =
                {
                    AspectMask = ImageAspectFlags.ImageAspectColorBit,
                    BaseMipLevel = 0,
                    LevelCount = 1,
                    BaseArrayLayer = 0,
                    LayerCount = 1,
                }

            };
            if (VkFunc.CreateImageView(VulkanLogicalDevice, createInfo, null, out var imageView) != Result.Success)
            {
                throw new Exception("failed to create image views!");
            }
            ImageView = imageView;

            // Create texture sampler
            var properties = VkFunc.GetPhysicalDeviceProperties(VulkanPhysicalDevice);

            var samplerInfo = new SamplerCreateInfo()
            {
                SType = StructureType.SamplerCreateInfo,
                MagFilter = settings.SamplerMagFilter,
                MinFilter = settings.SamplerMinFilter,
                AddressModeU = settings.SamplerAddressModeU,
                AddressModeV = settings.SamplerAddressModeV,
                AddressModeW = settings.SamplerAddressModeW,
                AnisotropyEnable = settings.SamplerAnisotropyEnable,
                MaxAnisotropy = properties.Limits.MaxSamplerAnisotropy,
                BorderColor = settings.SamplerBorderColor,
                UnnormalizedCoordinates = false,
                CompareEnable = settings.SamplerCompareEnable,
                CompareOp = settings.SamplerCompareOp,
                MipmapMode = settings.SamplerMipmapMode,
            };

            ImageSampler = VkFunc.CreateSampler(VulkanLogicalDevice, samplerInfo, null);
        }
    }

    private unsafe void TransitionImageLayout(Silk.NET.Vulkan.Image image, Format format, ImageLayout oldLayout, ImageLayout newLayout)
    {
        var commandBuffer = new VulkanSingleTimeCommandBuffer(VulkanCommandPool);

        var barrier = new ImageMemoryBarrier()
        {
            SType = StructureType.ImageMemoryBarrier,
            OldLayout = oldLayout,
            NewLayout = newLayout,
            SrcQueueFamilyIndex = Vk.QueueFamilyIgnored,
            DstQueueFamilyIndex = Vk.QueueFamilyIgnored,
            Image = image,
            SubresourceRange =
            {
                AspectMask = ImageAspectFlags.ImageAspectColorBit,
                BaseMipLevel = 0,
                LevelCount = 1,
                BaseArrayLayer = 0,
                LayerCount = 1,
            }
        };

        PipelineStageFlags sourceStage;
        PipelineStageFlags destinationStage;

        if (oldLayout == ImageLayout.Undefined && newLayout == ImageLayout.TransferDstOptimal)
        {
            barrier.SrcAccessMask = 0;
            barrier.DstAccessMask = AccessFlags.AccessTransferWriteBit;

            sourceStage = PipelineStageFlags.PipelineStageTopOfPipeBit;
            destinationStage = PipelineStageFlags.PipelineStageTransferBit;
        }
        else if (oldLayout == ImageLayout.TransferDstOptimal && newLayout == ImageLayout.ShaderReadOnlyOptimal)
        {
            barrier.SrcAccessMask = AccessFlags.AccessTransferWriteBit;
            barrier.DstAccessMask = AccessFlags.AccessShaderReadBit;

            sourceStage = PipelineStageFlags.PipelineStageTransferBit;
            destinationStage = PipelineStageFlags.PipelineStageFragmentShaderBit;
        }
        else
        {
            throw new Exception("Unsupported layout transition!");
        }

        commandBuffer.CmdPipelineBarrier(sourceStage, destinationStage, 0, null, null, new[] { barrier });

        commandBuffer.End();
    }

    protected override void Dispose(bool disposing)
    {
        VkFunc.DestroySampler(VulkanLogicalDevice, ImageSampler, null);
        VkFunc.DestroyImageView(VulkanLogicalDevice, ImageView, null);
        VkFunc.DestroyImage(VulkanLogicalDevice, Image, null);
        VkFunc.FreeMemory(VulkanLogicalDevice, ImageMemory, null);

        TextureImage.Dispose();
        base.Dispose(disposing);
    }

    public static implicit operator Silk.NET.Vulkan.Image(VulkanTexture v) => v.Image;
    public static implicit operator DeviceMemory(VulkanTexture v) => v.ImageMemory;
    public static implicit operator ImageView(VulkanTexture v) => v.ImageView;
    public static implicit operator Sampler(VulkanTexture v) => v.ImageSampler;
}