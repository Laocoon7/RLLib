using Silk.NET.Vulkan;

using RLLib.VulkanInfo;

namespace RLLib.VulkanBuilders;

public class DescriptorPoolBuilder
{
    private BuilderSettings Settings;

    public DescriptorPoolBuilder(VulkanDescriptorSetLayout vulkanDescriptorSetLayout)
    {
        Settings = new BuilderSettings(vulkanDescriptorSetLayout);
    }

    public VulkanDescriptorPool Build() => new VulkanDescriptorPool(Settings);

    #region Builder

    public DescriptorPoolBuilder SetDescriptorPoolCreateFlags(DescriptorPoolCreateFlags flags)
    {
        Settings.DescriptorPoolCreateFlags = flags;
        return this;
    }

    public DescriptorPoolBuilder SetMaxSets(uint maxSets)
    {
        Settings.MaxSets = maxSets;
        return this;
    }
    #endregion

    public DescriptorPoolBuilder Reset(VulkanDescriptorSetLayout vulkanDescriptorSetLayout)
    {
        Settings.Reset(vulkanDescriptorSetLayout);
        return this;
    }

    public DescriptorPoolBuilder Reset()
    {
        Settings.Reset();
        return this;
    }

    internal class BuilderSettings
    {
        public VulkanDescriptorSetLayout VulkanDescriptorSetLayout { get; set; }

        public DescriptorPoolCreateFlags DescriptorPoolCreateFlags { get; set; }
        public uint MaxSets { get; set; }

        public BuilderSettings(VulkanDescriptorSetLayout vulkanDescriptorSetLayout)
        {
            VulkanDescriptorSetLayout = vulkanDescriptorSetLayout;
        }

        public void Reset(VulkanDescriptorSetLayout vulkanDescriptorSetLayout)
        {
            VulkanDescriptorSetLayout = vulkanDescriptorSetLayout;

            Reset();
        }

        public void Reset()
        {
            DescriptorPoolCreateFlags = 0;
            MaxSets = 0;
        }
    }
}

public class VulkanDescriptorPool : VkObject
{
    internal VulkanInstance VulkanInstance => VulkanDescriptorSetLayout.VulkanInstance;
    internal VulkanLogicalDevice VulkanLogicalDevice => VulkanDescriptorSetLayout.VulkanLogicalDevice;
    internal VulkanDescriptorSetLayout VulkanDescriptorSetLayout { get; init; }

    internal DescriptorPoolCreateFlags DescriptorPoolCreateFlags { get; set; }

    public DescriptorPool DescriptorPool { get; init; }
    public uint MaxSets { get; init; }

    internal unsafe VulkanDescriptorPool(DescriptorPoolBuilder.BuilderSettings settings)
    {
        VulkanDescriptorSetLayout = settings.VulkanDescriptorSetLayout;
        DescriptorPoolCreateFlags = settings.DescriptorPoolCreateFlags;

        MaxSets = settings.MaxSets;

        var descriptorPoolSizes = new List<DescriptorPoolSize>();

        foreach (var binding in VulkanDescriptorSetLayout.DescriptorDefinitions.Keys)
        {
            foreach (var definition in VulkanDescriptorSetLayout.DescriptorDefinitions[binding])
            {
                descriptorPoolSizes.Add(new DescriptorPoolSize()
                {
                    Type = definition.DescriptorType,
                    DescriptorCount = definition.Count,
                });
            }
        }

        var poolSizes = descriptorPoolSizes.ToArray();

        fixed (DescriptorPoolSize* pPoolSizes = poolSizes)
        {
            DescriptorPoolCreateInfo poolInfo = new()
            {
                SType = StructureType.DescriptorPoolCreateInfo,
                Flags = settings.DescriptorPoolCreateFlags,
                PoolSizeCount = (uint)poolSizes.Length,
                PPoolSizes = pPoolSizes,
                MaxSets = MaxSets,
            };

            if (VkFunc.CreateDescriptorPool(VulkanLogicalDevice, poolInfo, VulkanInstance.AllocationCallbacks, out var descriptorPool) != Result.Success)
            {
                throw new Exception("Failed to create descriptor pool!");
            }

            DescriptorPool = descriptorPool;
        }
    }

    protected override void Dispose(bool disposing)
    {
        VkFunc.DestroyDescriptorPool(VulkanLogicalDevice, DescriptorPool, VulkanInstance.AllocationCallbacks);

        base.Dispose(disposing);
    }

    public static implicit operator DescriptorPool(VulkanDescriptorPool v) => v.DescriptorPool;
}
