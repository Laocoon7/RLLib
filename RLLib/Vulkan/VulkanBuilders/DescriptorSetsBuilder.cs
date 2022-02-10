using Silk.NET.Vulkan;

using RLLib.VulkanInfo;

namespace RLLib.VulkanBuilders;

public class DescriptorSetsBuilder
{
    private BuilderSettings Settings;

    public DescriptorSetsBuilder(VulkanDescriptorPool vulkanDescriptorPool)
    {
        Settings = new BuilderSettings(vulkanDescriptorPool);
    }

    public VulkanDescriptorSets Build() => new VulkanDescriptorSets(Settings);

    #region Builder

    #endregion

    public DescriptorSetsBuilder Reset(VulkanDescriptorPool vulkanDescriptorPool)
    {
        Settings.Reset(vulkanDescriptorPool);
        return this;
    }

    public DescriptorSetsBuilder Reset()
    {
        Settings.Reset();
        return this;
    }

    internal class BuilderSettings
    {
        public VulkanDescriptorPool VulkanDescriptorPool { get; set; }

        public BuilderSettings(VulkanDescriptorPool vulkanDescriptorPool)
        {
            VulkanDescriptorPool = vulkanDescriptorPool;
        }

        public void Reset(VulkanDescriptorPool vulkanDescriptorPool)
        {
            VulkanDescriptorPool = vulkanDescriptorPool;

            Reset();
        }

        public void Reset()
        {

        }
    }
}

public class VulkanDescriptorSets : VkObject
{
    internal VulkanInstance VulkanInstance => VulkanDescriptorPool.VulkanInstance;
    internal VulkanLogicalDevice VulkanLogicalDevice => VulkanDescriptorPool.VulkanLogicalDevice;
    internal VulkanDescriptorSetLayout VulkanDescriptorSetLayout => VulkanDescriptorPool.VulkanDescriptorSetLayout;
    internal VulkanDescriptorPool VulkanDescriptorPool { get; init; }

    public DescriptorSet[] DescriptorSets { get; init; }

    internal unsafe VulkanDescriptorSets(DescriptorSetsBuilder.BuilderSettings settings)
    {
        VulkanDescriptorPool = settings.VulkanDescriptorPool;

        var descriptorSets = new DescriptorSet[VulkanDescriptorPool.MaxSets];

        var layouts = new DescriptorSetLayout[VulkanDescriptorPool.MaxSets];
        Array.Fill(layouts, VulkanDescriptorSetLayout);

        fixed (DescriptorSetLayout* pLayouts = layouts)
        {
            var allocateInfo = new DescriptorSetAllocateInfo()
            {
                SType = StructureType.DescriptorSetAllocateInfo,
                DescriptorPool = VulkanDescriptorPool,
                DescriptorSetCount = VulkanDescriptorPool.MaxSets,
                PSetLayouts = pLayouts,
            };

            if (VkFunc.AllocateDescriptorSets(VulkanLogicalDevice, allocateInfo, out descriptorSets) != Result.Success)
            {
                throw new Exception("Failed to allocate descriptor sets!");
            }
        }

        DescriptorSets = descriptorSets;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            VkFunc.FreeDescriptorSets(VulkanLogicalDevice, VulkanDescriptorPool, DescriptorSets);
        }

        base.Dispose(disposing);
    }

    public static implicit operator DescriptorSet[](VulkanDescriptorSets v) => v.DescriptorSets;
}
