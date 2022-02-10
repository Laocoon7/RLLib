using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.KHR;

using RLLib.VulkanInfo;

namespace RLLib.VulkanBuilders;

public class SwapchainBuilder
{
    private BuilderSettings Settings;

    public SwapchainBuilder(VulkanLogicalDevice vulkanLogicalDevice, VulkanQueue graphicsQueue, VulkanQueue presentQueue)
    {
        Settings = new BuilderSettings(vulkanLogicalDevice, graphicsQueue, presentQueue);
    }

	public VulkanSwapchain Build() => new VulkanSwapchain(Settings);

	#region Builder
	public SwapchainBuilder SetGraphicsQueue(VulkanQueue queue)
	{
		Settings.GraphicsQueue = queue;
		return this;
	}

	public SwapchainBuilder SetPresentQueue(VulkanQueue queue)
	{
		Settings.PresentQueue = queue;
		return this;
	}

	public SwapchainBuilder SetDesiredExtent(uint width, uint height)
	{
		Settings.DesiredWidth = width;
		Settings.DesiredHeight = height;
		return this;
	}

	public SwapchainBuilder AddDesiredFormat(SurfaceFormatKHR format)
	{
		Settings.DesiredFormats.Add(format);
		return this;
	}

	public SwapchainBuilder AddFallbackFormat(SurfaceFormatKHR format)
	{
		Settings.FallbackFormats.Add(format);
		return this;
	}

	public SwapchainBuilder AddDefaultFormats()
	{
		Settings.AddDefaultFormats();

		return this;
	}

	public SwapchainBuilder AddDesiredPresentMode(PresentModeKHR presentMode)
	{
		Settings.DesiredPresentModes.Add(presentMode);
		return this;
	}

	public SwapchainBuilder AddFallbackPresentMode(PresentModeKHR presentMode)
	{
		Settings.FallbackPresentModes.Add(presentMode);
		return this;
	}

	public SwapchainBuilder AddDefaultPresentModes()
	{
		Settings.AddDefaultPresentModes();
		return this;
	}

	public SwapchainBuilder SetImageUsageFlags(ImageUsageFlags flags)
	{
		Settings.ImageUsageFlags = flags;
		return this;
	}

	public SwapchainBuilder AddImageUsageFlags(ImageUsageFlags flags)
	{
		Settings.ImageUsageFlags |= flags;
		return this;
	}

	public SwapchainBuilder SetFormatFeatureFlags(FormatFeatureFlags flags)
	{
		Settings.FormatFeatureFlags = flags;
		return this;
	}

	public SwapchainBuilder AddFormatFeatureFlags(FormatFeatureFlags flags)
	{
		Settings.FormatFeatureFlags |= flags;
		return this;
	}

	public SwapchainBuilder SetImageArrayLayerCount(uint count)
	{
		Settings.ImageArrayLayerCount = count;
		return this;
	}

	public SwapchainBuilder SetClipped(bool clipped = true)
	{
		Settings.Clipped = clipped;
		return this;
	}

	public SwapchainBuilder SetCreateFlags(SwapchainCreateFlagsKHR flags)
	{
		Settings.SwapchainCreateFlags = flags;
		return this;
	}

	public SwapchainBuilder SetPreTransformFlags(SurfaceTransformFlagsKHR flags)
	{
		Settings.SurfaceTransformFlags = flags;
		return this;
	}

	public SwapchainBuilder SetCompositeAlphaFlags(CompositeAlphaFlagsKHR flags)
	{
		Settings.CompositeAlphaFlags = flags;
		return this;
	}

	public SwapchainBuilder SetOldSwapchain(VulkanSwapchain vulkanSwapchain)
    {
		Settings.OldSwapchain = vulkanSwapchain;
		return this;
    }
	#endregion

	public SwapchainBuilder Reset(VulkanLogicalDevice vulkanLogicalDevice, VulkanQueue graphicsQueue, VulkanQueue presentQueue)
    {
        Settings.Reset(vulkanLogicalDevice, graphicsQueue, presentQueue);
        return this;
    }

	public SwapchainBuilder Reset()
    {
		Settings.Reset();
		return this;
    }

    internal class BuilderSettings
    {
		public VulkanLogicalDevice VulkanLogicalDevice { get; set; }

        public VulkanQueue GraphicsQueue { get; set; }
        public VulkanQueue PresentQueue { get; set; }

        public uint DesiredWidth { get; set; } = 0;
        public uint DesiredHeight { get; set; } = 0;

        public List<SurfaceFormatKHR> DesiredFormats { get; set; } = new List<SurfaceFormatKHR>();
        public List<SurfaceFormatKHR> FallbackFormats { get; set; } = new List<SurfaceFormatKHR>();

        public List<PresentModeKHR> DesiredPresentModes { get; set; } = new List<PresentModeKHR>();
        public List<PresentModeKHR> FallbackPresentModes { get; set; } = new List<PresentModeKHR>();

		public ImageUsageFlags ImageUsageFlags { get; set; } = ImageUsageFlags.ImageUsageColorAttachmentBit;

		public FormatFeatureFlags FormatFeatureFlags { get; set; } = FormatFeatureFlags.FormatFeatureSampledImageBit;

		public SurfaceTransformFlagsKHR SurfaceTransformFlags { get; set; }
		public CompositeAlphaFlagsKHR CompositeAlphaFlags { get; set; } = CompositeAlphaFlagsKHR.CompositeAlphaOpaqueBitKhr;

		public uint ImageArrayLayerCount { get; set; } = 1;

		public bool Clipped { get; set; } = true;

		public SwapchainCreateFlagsKHR SwapchainCreateFlags { get; set; } = 0;

        public VulkanSwapchain? OldSwapchain { get; set; } = null;

        public BuilderSettings(VulkanLogicalDevice vulkanLogicalDevice, VulkanQueue graphicsQueue, VulkanQueue presentQueue)
        {
			VulkanLogicalDevice = vulkanLogicalDevice;
            GraphicsQueue = graphicsQueue;
            PresentQueue = presentQueue;
        }

        internal void AddDefaultFormats()
        {
            FallbackFormats.AddRange(new[]
            {
            new SurfaceFormatKHR()
            {
                Format = Format.R8G8B8A8Srgb,
                ColorSpace = ColorSpaceKHR.ColorSpaceSrgbNonlinearKhr,
            },
            new SurfaceFormatKHR()
            {
                Format = Format.B8G8R8A8Srgb,
                ColorSpace = ColorSpaceKHR.ColorSpaceSrgbNonlinearKhr,
            }
        });
        }

        internal void AddDefaultPresentModes()
        {
            FallbackPresentModes.AddRange(new[]
            {
            PresentModeKHR.PresentModeFifoKhr,
            PresentModeKHR.PresentModeMailboxKhr,
        });
        }

        public void Reset(VulkanLogicalDevice vulkanLogicalDevice, VulkanQueue graphicsQueue, VulkanQueue presentQueue)
        {
			VulkanLogicalDevice = vulkanLogicalDevice;

            GraphicsQueue = graphicsQueue;
            PresentQueue = presentQueue;

			Reset();
        }

		public void Reset()
        {
            DesiredWidth = 0;
            DesiredHeight = 0;

            DesiredFormats.Clear();
            FallbackFormats.Clear();

            DesiredPresentModes.Clear();
            FallbackPresentModes.Clear();

            ImageUsageFlags = 0;
            FormatFeatureFlags = 0;
            SurfaceTransformFlags = 0;
            CompositeAlphaFlags = 0;

            ImageArrayLayerCount = 0;

            Clipped = false;

            SwapchainCreateFlags = 0;

            OldSwapchain = null;
        }
    }
}

public class VulkanSwapchain : VkObject
{
	internal VulkanInstance VulkanInstance => VulkanLogicalDevice.VulkanInstance;
	internal VulkanSurface VulkanSurface => VulkanLogicalDevice.VulkanSurface!;
	internal VulkanPhysicalDevice VulkanPhysicalDevice => VulkanLogicalDevice.VulkanPhysicalDevice;
	internal VulkanLogicalDevice VulkanLogicalDevice { get; init; }

	public VulkanQueue GraphicsQueue { get; init; }
	public VulkanQueue PresentQueue { get; init; }

	public KhrSwapchain KhrSwapchain { get; init; }
	public SwapchainKHR SwapchainKHR { get; private set; }
	public uint ImageCount { get; private set; } = 0;
	public Format ImageFormat { get; private set; } = Format.Undefined;
	public PresentModeKHR PresentMode { get; private set; } = PresentModeKHR.PresentModeFifoKhr;
	public Extent2D Extent { get; private set; } = new Extent2D(0, 0);
	public Image[] Images { get; private set; }
	public ImageView[] ImageViews { get; private set; }
	public Framebuffer[]? Framebuffers { get; private set; }

	internal unsafe VulkanSwapchain(SwapchainBuilder.BuilderSettings settings)
	{
		VulkanLogicalDevice = settings.VulkanLogicalDevice;
		if (VulkanLogicalDevice.VulkanSurface == null)
        {
			throw new Exception("Failed to create swapchain. No surface provided.");
        }

		GraphicsQueue = settings.GraphicsQueue!;
		PresentQueue = settings.PresentQueue!;

		if (settings.FallbackFormats.Count == 0)
		{
			settings.AddDefaultFormats();
		}

		if (settings.FallbackPresentModes.Count == 0)
		{
			settings.AddDefaultPresentModes();
		}

		var capabilities = VulkanSurface.GetCapabilities(VulkanPhysicalDevice);
		var formats = VulkanSurface.GetSurfaceFormats(VulkanPhysicalDevice);
		var presentModes = VulkanSurface.GetPresentModes(VulkanPhysicalDevice);

		if (!formats.Any() || !presentModes.Any())
		{
			throw new Exception($"Surface not supported.");
		}

		var imageCount = capabilities.MinImageCount + 1;
		if (capabilities.MaxImageCount > 0 && imageCount > capabilities.MaxImageCount)
		{
			imageCount = capabilities.MaxImageCount;
		}

		var surfaceFormat = ChooseSurfaceFormat(formats, settings.DesiredFormats, settings.FallbackFormats, settings.FormatFeatureFlags);
		var presentMode = ChoosePresentMode(presentModes, settings.DesiredPresentModes, settings.FallbackPresentModes);
		var extent = ChooseExtent(capabilities, settings.DesiredWidth, settings.DesiredHeight);

		var imageArrayLayerCount = settings.ImageArrayLayerCount;

		if (imageArrayLayerCount == 0)
		{
			imageArrayLayerCount = 1;
		}
		if (capabilities.MaxImageArrayLayers < imageArrayLayerCount)
		{
			imageArrayLayerCount = capabilities.MaxImageArrayLayers;
		}

		var preTransform = settings.SurfaceTransformFlags;
		if (preTransform == (SurfaceTransformFlagsKHR)0)
		{
			preTransform = capabilities.CurrentTransform;
		}

		var createSwapChainInfo = new SwapchainCreateInfoKHR()
		{
			SType = StructureType.SwapchainCreateInfoKhr,
			Surface = VulkanSurface,
			Flags = settings.SwapchainCreateFlags,

			ImageUsage = settings.ImageUsageFlags,
			ImageFormat = surfaceFormat.Format,
			ImageColorSpace = surfaceFormat.ColorSpace,
			PresentMode = presentMode,
			ImageExtent = extent,

			MinImageCount = imageCount,
			ImageArrayLayers = imageArrayLayerCount,

			PreTransform = preTransform,
			CompositeAlpha = settings.CompositeAlphaFlags,
			Clipped = settings.Clipped,
			OldSwapchain = settings.OldSwapchain == null ? default : settings.OldSwapchain.SwapchainKHR,
		};

		var queueFamilyIndices = stackalloc uint[] { GraphicsQueue.QueueFamilyIndex, PresentQueue.QueueFamilyIndex };

		if (GraphicsQueue.QueueFamilyIndex != PresentQueue.QueueFamilyIndex)
		{
			createSwapChainInfo = createSwapChainInfo with
			{
				ImageSharingMode = SharingMode.Concurrent,
				QueueFamilyIndexCount = 2,
				PQueueFamilyIndices = queueFamilyIndices,
			};
		}
		else
		{
			createSwapChainInfo.ImageSharingMode = SharingMode.Exclusive;
		}

		if (!VkFunc.GetDeviceExtension<KhrSwapchain>(VulkanInstance, VulkanLogicalDevice, out var khrSwapchain))
        {
			throw new Exception($"Failed to get VK_KHR_swapchain extension.");
		}

		KhrSwapchain = khrSwapchain;

		SwapchainKHR swapChain;
		if (VulkanInstance.AllocationCallbacks != null)
		{
			var allocationCallbacks = VulkanInstance.AllocationCallbacks.Value;
			if (KhrSwapchain.CreateSwapchain(VulkanLogicalDevice, createSwapChainInfo, &allocationCallbacks, out swapChain) != Result.Success)
			{
				throw new Exception("Failed to create swapchain.");
			}
		}
		else
		{
			if (KhrSwapchain.CreateSwapchain(VulkanLogicalDevice, createSwapChainInfo, null, out swapChain) != Result.Success)
			{
				throw new Exception("Failed to create swapchain.");
			}
		}
		SwapchainKHR = swapChain;
		ImageFormat = surfaceFormat.Format;
		PresentMode = presentMode;
		Extent = extent;
		Images = GetImages();
		ImageCount = (uint)Images.Length;
		ImageViews = GetImageViews();
	}

	public unsafe Framebuffer[] GetFramebuffers(VulkanRenderPass vulkanRenderPass)
    {
		if (Framebuffers != null)
        {
			return Framebuffers;
        }

		var swapchainFramebuffers = new Framebuffer[ImageCount];

		for (int i = 0; i < ImageCount; i++)
		{
			var attachment = ImageViews[i];

			var framebufferInfo = new FramebufferCreateInfo()
			{
				SType = StructureType.FramebufferCreateInfo,
				RenderPass = vulkanRenderPass,
				AttachmentCount = 1,
				PAttachments = &attachment,
				Width = Extent.Width,
				Height = Extent.Height,
				Layers = 1,
			};

			if (VkFunc.CreateFramebuffer(VulkanLogicalDevice, framebufferInfo, VulkanInstance.AllocationCallbacks, out swapchainFramebuffers[i]) != Result.Success)
			{
				throw new Exception("Failed to create framebuffer!");
			}
		}
		Framebuffers = swapchainFramebuffers;

		return swapchainFramebuffers;
	}

	private unsafe Image[] GetImages()
	{
		uint imageCount = 0;
		KhrSwapchain.GetSwapchainImages(VulkanLogicalDevice, SwapchainKHR, ref imageCount, null);
		var swapChainImages = new Image[imageCount];
		fixed (Image* pSwapChainImages = swapChainImages)
		{
			KhrSwapchain.GetSwapchainImages(VulkanLogicalDevice, SwapchainKHR, ref imageCount, pSwapChainImages);
		}
		return swapChainImages;
	}

	private unsafe ImageView[] GetImageViews()
    {
		var swapchainImageViews = new ImageView[ImageCount];

		var createInfo = new ImageViewCreateInfo()
		{
			SType = StructureType.ImageViewCreateInfo,
			ViewType = ImageViewType.ImageViewType2D,
			Format = ImageFormat,
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

		for (int i = 0; i < ImageCount; i++)
		{
			createInfo.Image = Images[i];
			if (VkFunc.CreateImageView(VulkanLogicalDevice, createInfo, VulkanInstance.AllocationCallbacks, out var imageView)  != Result.Success)
            {
				throw new Exception("Failed to create image view.");
            }

			swapchainImageViews[i] = imageView;
		}

		return swapchainImageViews;
	}

	private SurfaceFormatKHR ChooseSurfaceFormat(List<SurfaceFormatKHR> availableFormats, List<SurfaceFormatKHR> desiredFormats, List<SurfaceFormatKHR> fallbackFormats, FormatFeatureFlags features)
	{
		foreach (var desiredFormat in desiredFormats)
		{
			foreach (var availableFormat in availableFormats)
			{
				if (availableFormat.Format == desiredFormat.Format &&
					availableFormat.ColorSpace == desiredFormat.ColorSpace)
				{
					var properties = VkFunc.GetPhysicalDeviceFormatProperties(VulkanPhysicalDevice, desiredFormat.Format);

					if ((properties.OptimalTilingFeatures & features) == features)
					{
						return desiredFormat;
					}
				}
			}
		}

		foreach (var fallbackFormat in fallbackFormats)
		{
			foreach (var availableFormat in availableFormats)
			{
				if (availableFormat.Format == fallbackFormat.Format &&
					availableFormat.ColorSpace == fallbackFormat.ColorSpace)
				{
					var properties = VkFunc.GetPhysicalDeviceFormatProperties(VulkanPhysicalDevice, fallbackFormat.Format);

					if ((properties.OptimalTilingFeatures & features) == features)
					{
						return fallbackFormat;
					}
				}
			}
		}

		return availableFormats[0];
	}

	private PresentModeKHR ChoosePresentMode(List<PresentModeKHR> availableModes, List<PresentModeKHR> desiredModes, List<PresentModeKHR> fallbackModes)
	{
		foreach (var desiredMode in desiredModes)
		{
			foreach (var availableMode in availableModes)
			{
				if (availableMode == desiredMode)
				{
					return desiredMode;
				}
			}
		}

		foreach (var fallbackMode in fallbackModes)
		{
			foreach (var availableMode in availableModes)
			{
				if (availableMode == fallbackMode)
				{
					return fallbackMode;
				}
			}
		}

		// This default fallback mode is always present. Use as last case scenario
		return PresentModeKHR.PresentModeFifoKhr;
	}

	private Extent2D ChooseExtent(SurfaceCapabilitiesKHR capabilities, uint desiredWidth, uint desiredHeight)
	{
		if (capabilities.CurrentExtent.Width != uint.MaxValue)
		{
			return capabilities.CurrentExtent;
		}
		else
		{
			Extent2D actualExtent;
			if (desiredWidth > 0 && desiredHeight > 0)
			{
				actualExtent = new Extent2D(desiredWidth, desiredHeight);
			}
			else
			{
				var framebufferSize = VulkanSurface.VulkanWindow.Window.FramebufferSize;

				actualExtent = new Extent2D()
				{
					Width = (uint)framebufferSize.X,
					Height = (uint)framebufferSize.Y
				};
			}

			actualExtent.Width = Math.Clamp(actualExtent.Width, capabilities.MinImageExtent.Width, capabilities.MaxImageExtent.Width);
			actualExtent.Height = Math.Clamp(actualExtent.Height, capabilities.MinImageExtent.Height, capabilities.MaxImageExtent.Height);

			return actualExtent;
		}
	}

	protected unsafe override void Dispose(bool disposing)
	{
		if (Framebuffers != null)
		{
			foreach (var framebuffer in Framebuffers)
			{
				VkFunc.DestroyFramebuffer(VulkanLogicalDevice, framebuffer, VulkanInstance.AllocationCallbacks);
			}
		}

        foreach (var imageView in ImageViews)
        {
			VkFunc.DestroyImageView(VulkanLogicalDevice, imageView, VulkanInstance.AllocationCallbacks);
        }

		if (VulkanInstance.AllocationCallbacks != null)
		{
			var allocationCallbacks = VulkanInstance.AllocationCallbacks.Value;
			KhrSwapchain.DestroySwapchain(VulkanLogicalDevice, SwapchainKHR, &allocationCallbacks);
		}
		else
		{
			KhrSwapchain.DestroySwapchain(VulkanLogicalDevice, SwapchainKHR, null);
		}

		KhrSwapchain.Dispose();

		base.Dispose(disposing);
	}

	public static implicit operator SwapchainKHR(VulkanSwapchain v) => v.SwapchainKHR;
	public static implicit operator KhrSwapchain(VulkanSwapchain v) => v.KhrSwapchain;
}
