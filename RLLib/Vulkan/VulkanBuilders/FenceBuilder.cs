using Silk.NET.Vulkan;

using RLLib.VulkanInfo;

namespace RLLib.VulkanBuilders;

public class FenceBuilder
{
    private BuilderSettings Settings;

    public FenceBuilder(VulkanLogicalDevice vulkanLogicalDevice)
    {
        Settings = new BuilderSettings(vulkanLogicalDevice);
    }

    public VulkanFence Build() => new VulkanFence(Settings);

    #region Builder
    public FenceBuilder SetFenceCreateFlags(FenceCreateFlags flags)
    {
        Settings.FenceCreateFlags = flags;
        return this;
    }
    #endregion

    public FenceBuilder Reset(VulkanLogicalDevice vulkanLogicalDevice)
    {
        Settings.Reset(vulkanLogicalDevice);
        return this;
    }

    public FenceBuilder Reset()
    {
        Settings.Reset();
        return this;
    }

    internal class BuilderSettings
    {
        public VulkanLogicalDevice VulkanLogicalDevice { get; set; }

        public FenceCreateFlags FenceCreateFlags { get; set; } = FenceCreateFlags.FenceCreateSignaledBit;

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
            FenceCreateFlags = FenceCreateFlags.FenceCreateSignaledBit;
        }
    }
}

public class VulkanFence : VkObject
{
    internal VulkanLogicalDevice VulkanLogicalDevice;


    public Fence Fence { get; init; }

    internal VulkanFence(FenceBuilder.BuilderSettings settings)
    {
        VulkanLogicalDevice = settings.VulkanLogicalDevice;

        var fenceCreateInfo = new FenceCreateInfo()
        {
            SType = StructureType.FenceCreateInfo,
            Flags = settings.FenceCreateFlags,
        };

        Fence = VkFunc.CreateFence(VulkanLogicalDevice, fenceCreateInfo, null);
    }

    protected override void Dispose(bool disposing)
    {
        VkFunc.DestroyFence(VulkanLogicalDevice, Fence, null);
        base.Dispose(disposing);
    }

    public static implicit operator Fence(VulkanFence v) => v.Fence;
}