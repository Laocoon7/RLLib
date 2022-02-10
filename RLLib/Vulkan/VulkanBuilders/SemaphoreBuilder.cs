using Silk.NET.Vulkan;

using RLLib.VulkanInfo;

using Semaphore = Silk.NET.Vulkan.Semaphore;

namespace RLLib.VulkanBuilders;

public class SemaphoreBuilder
{
    private BuilderSettings Settings;

    public SemaphoreBuilder(VulkanLogicalDevice vulkanLogicalDevice)
    {
        Settings = new BuilderSettings(vulkanLogicalDevice);
    }

    public VulkanSemaphore Build() => new VulkanSemaphore(Settings);

    #region Builder
    public SemaphoreBuilder SetSemaphoreCreateFlags(SemaphoreCreateFlags flags)
    {
        Settings.SemaphoreCreateFlags = flags;
        return this;
    }
    #endregion

    public SemaphoreBuilder Reset(VulkanLogicalDevice vulkanLogicalDevice)
    {
        Settings.Reset(vulkanLogicalDevice);
        return this;
    }

    public SemaphoreBuilder Reset()
    {
        Settings.Reset();
        return this;
    }

    internal class BuilderSettings
    {
        public VulkanLogicalDevice VulkanLogicalDevice { get; set; }

        public SemaphoreCreateFlags SemaphoreCreateFlags { get; set; } = 0;

        public BuilderSettings(VulkanLogicalDevice vulkanLogicalDevice)
        {
            VulkanLogicalDevice = vulkanLogicalDevice;
        }

        public void Reset(VulkanLogicalDevice vulkanLogicalDevice)
        {
            VulkanLogicalDevice = vulkanLogicalDevice;

            Reset();
        }

        public void Reset()
        {
            SemaphoreCreateFlags = 0;
        }
    }
}

public class VulkanSemaphore : VkObject
{
    internal VulkanLogicalDevice VulkanLogicalDevice;


    public Semaphore Semaphore { get; init; }

    internal VulkanSemaphore(SemaphoreBuilder.BuilderSettings settings)
    {
        VulkanLogicalDevice = settings.VulkanLogicalDevice;

        var semaphoreCreateInfo = new SemaphoreCreateInfo()
        {
            SType = StructureType.SemaphoreCreateInfo,
            Flags = (uint)settings.SemaphoreCreateFlags,
        };

        Semaphore = VkFunc.CreateSemaphore(VulkanLogicalDevice, semaphoreCreateInfo, null);
    }

    protected override void Dispose(bool disposing)
    {
        VkFunc.DestroySemaphore(VulkanLogicalDevice, Semaphore, null);
        base.Dispose(disposing);
    }

    public static implicit operator Semaphore(VulkanSemaphore v) => v.Semaphore;
}