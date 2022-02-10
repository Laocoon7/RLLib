using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Vulkan;
using RLLib.VulkanInfo;

namespace RLLib.VulkanBuilders;

public class SurfaceBuilder
{
    private BuilderSettings Settings;

    public SurfaceBuilder(VulkanInstance vulkanInstance)
    {
        Settings = new BuilderSettings(vulkanInstance);
    }

    public VulkanSurface Build() => new VulkanSurface(Settings);

    #region Builder
    #endregion

    public SurfaceBuilder Reset(VulkanInstance vulkanInstance)
    {
        Settings.Reset(vulkanInstance);
        return this;
    }
    public SurfaceBuilder Reset()
    {
        Settings.Reset();
        return this;
    }

    internal class BuilderSettings
    {
        public VulkanInstance VulkanInstance { get; set; }

        public BuilderSettings(VulkanInstance vulkanInstance)
        {
            VulkanInstance = vulkanInstance;
        }

        public void Reset(VulkanInstance vulkanInstance)
        {
            VulkanInstance = vulkanInstance;

            Reset();
        }

        public void Reset()
        {

        }
    }
}

public class VulkanSurface : VkObject
{
    internal VulkanInstance VulkanInstance { get; init; }
    internal VulkanWindow VulkanWindow { get; init; }

    public KhrSurface KhrSurface { get; init; }
    public SurfaceKHR Surface { get; init; }
    internal unsafe VulkanSurface(SurfaceBuilder.BuilderSettings settings)
    {
        if (settings.VulkanInstance.VulkanWindow == null)
        {
            throw new Exception($"Cannot create VulkanSurface without a VulkanWindow.");
        }
        VulkanWindow = settings.VulkanInstance.VulkanWindow;
        VulkanInstance = settings.VulkanInstance;

        if (!VkFunc.GetInstanceExtension<KhrSurface>(VulkanInstance, out var khrSurface))
        {
            throw new Exception($"Could not get extension: KhrSurface.");
        }
        KhrSurface = khrSurface;

        if (VulkanInstance.AllocationCallbacks != null)
        {
            var allocationCallbacks = VulkanInstance.AllocationCallbacks.Value;
            Surface = VulkanWindow.Window.VkSurface!.Create(VulkanInstance.Instance.ToHandle(), &allocationCallbacks).ToSurface();
        }
        else
        {
            Surface = VulkanWindow.Window.VkSurface!.Create<AllocationCallbacks>(VulkanInstance.Instance.ToHandle(), null).ToSurface();
        }
    }

    public SurfaceCapabilitiesKHR GetCapabilities(PhysicalDevice physicalDevice)
    {
        KhrSurface.GetPhysicalDeviceSurfaceCapabilities(physicalDevice, Surface, out var capabilities);
        return capabilities;
    }

    public unsafe List<SurfaceFormatKHR> GetSurfaceFormats(PhysicalDevice physicalDevice)
    {
        uint formatCount = 0;
        KhrSurface.GetPhysicalDeviceSurfaceFormats(physicalDevice, Surface, ref formatCount, null);
        if (formatCount != 0)
        {
            var formats = new SurfaceFormatKHR[formatCount];
            fixed (SurfaceFormatKHR* pFormats = formats)
            {
                KhrSurface.GetPhysicalDeviceSurfaceFormats(physicalDevice, Surface, ref formatCount, pFormats);
            }
            return formats.ToList();
        }
        return new List<SurfaceFormatKHR>();
    }

    public unsafe List<PresentModeKHR> GetPresentModes(PhysicalDevice physicalDevice)
    {
        uint presentModeCount = 0;
        KhrSurface.GetPhysicalDeviceSurfacePresentModes(physicalDevice, Surface, ref presentModeCount, null);

        if (presentModeCount != 0)
        {
            var presentModes = new PresentModeKHR[presentModeCount];
            fixed (PresentModeKHR* pPresentModes = presentModes)
            {
                KhrSurface.GetPhysicalDeviceSurfacePresentModes(physicalDevice, Surface, ref presentModeCount, pPresentModes);
            }
            return presentModes.ToList();
        }

        return new List<PresentModeKHR>();
    }

    protected unsafe override void Dispose(bool disposing)
    {
        if (VulkanInstance.AllocationCallbacks != null)
        {
            var allocationCallbacks = VulkanInstance.AllocationCallbacks.Value;
            KhrSurface.DestroySurface(VulkanInstance.Instance, Surface, &allocationCallbacks);
        }
        else
        {
            KhrSurface.DestroySurface(VulkanInstance.Instance, Surface, null);
        }
        KhrSurface?.Dispose();
        base.Dispose(disposing);
    }

    public static implicit operator SurfaceKHR(VulkanSurface v) => v.Surface;
    public static implicit operator KhrSurface(VulkanSurface v) => v.KhrSurface;
}
