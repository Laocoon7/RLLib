using Silk.NET.Vulkan;

using RLLib.VulkanInfo;

namespace RLLib.VulkanBuilders;

public class CommandPoolBuilder
{
    private BuilderSettings Settings;

    public CommandPoolBuilder(VulkanLogicalDevice vulkanLogicalDevice, VulkanQueue vulkanQueue)
    {
        Settings = new BuilderSettings(vulkanLogicalDevice, vulkanQueue);
    }

    public VulkanCommandPool Build() => new VulkanCommandPool(Settings);

    #region Builder
    public CommandPoolBuilder SetCommandPoolCreateFlags(CommandPoolCreateFlags flags)
    {
        Settings.CommandPoolCreateFlags = flags;
        return this;
    }

    public CommandPoolBuilder SetQueue(VulkanQueue vulkanQueue)
    {
        Settings.VulkanQueue = vulkanQueue;
        return this;
    }
    #endregion

    public CommandPoolBuilder Reset(VulkanLogicalDevice vulkanLogicalDevice, VulkanQueue vulkanQueue)
    {
        Settings.Reset(vulkanLogicalDevice, vulkanQueue);
        return this;
    }

    public CommandPoolBuilder Reset()
    {
        Settings.Reset();
        return this;
    }

    internal class BuilderSettings
    {
        public VulkanLogicalDevice VulkanLogicalDevice { get; set; }
        public VulkanQueue VulkanQueue { get; set; }

        public CommandPoolCreateFlags CommandPoolCreateFlags { get; set; } = CommandPoolCreateFlags.CommandPoolCreateResetCommandBufferBit;

        public BuilderSettings(VulkanLogicalDevice vulkanLogicalDevice, VulkanQueue vulkanQueue)
        {
            VulkanLogicalDevice = vulkanLogicalDevice;
            VulkanQueue = vulkanQueue;
        }

        public void Reset(VulkanLogicalDevice vulkanLogicalDevice, VulkanQueue vulkanQueue)
        {
            VulkanLogicalDevice = vulkanLogicalDevice;
            VulkanQueue = vulkanQueue;

            Reset();
        }

        public void Reset()
        {
            CommandPoolCreateFlags = CommandPoolCreateFlags.CommandPoolCreateResetCommandBufferBit;
        }
    }
}

public class VulkanCommandPool : VkObject
{
    internal VulkanInstance VulkanInstance => VulkanLogicalDevice.VulkanInstance;
    internal VulkanLogicalDevice VulkanLogicalDevice { get; init; }
    internal VulkanQueue VulkanQueue { get; init; }

    public CommandPool CommandPool { get; init; }

    internal VulkanCommandPool(CommandPoolBuilder.BuilderSettings settings)
    {
        VulkanLogicalDevice = settings.VulkanLogicalDevice;
        VulkanQueue = settings.VulkanQueue;

        var poolInfo = new CommandPoolCreateInfo()
        {
            SType = StructureType.CommandPoolCreateInfo,
            QueueFamilyIndex = VulkanQueue.QueueFamilyIndex,
            Flags = settings.CommandPoolCreateFlags,
        };

        if (VkFunc.CreateCommandPool(VulkanLogicalDevice, poolInfo, VulkanInstance.AllocationCallbacks, out var commandPool) != Result.Success)
        {
            throw new Exception("Failed to create command pool!");
        }
        CommandPool = commandPool;
    }

    protected override void Dispose(bool disposing)
    {
        VkFunc.DestroyCommandPool(VulkanLogicalDevice, CommandPool, VulkanInstance.AllocationCallbacks);

        base.Dispose(disposing);
    }

    public static implicit operator CommandPool(VulkanCommandPool v) => v.CommandPool;
}