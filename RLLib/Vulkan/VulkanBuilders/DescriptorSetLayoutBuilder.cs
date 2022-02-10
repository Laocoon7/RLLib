using Silk.NET.Vulkan;

using RLLib.VulkanInfo;

namespace RLLib.VulkanBuilders;

public class DescriptorSetLayoutBuilder
{
    private BuilderSettings Settings;

    public DescriptorSetLayoutBuilder(VulkanLogicalDevice vulkanLogicalDevice)
    {
        Settings = new BuilderSettings(vulkanLogicalDevice);
    }

    public VulkanDescriptorSetLayout Build() => new VulkanDescriptorSetLayout(Settings);

    #region Builder
    public DescriptorSetLayoutBuilder AddDescriptor(uint binding, DescriptorType descriptorType, ShaderStageFlags shaderStageFlags, uint count)
    {
        if (!Settings.Descriptors.TryGetValue(binding, out var tuples))
        {
            tuples = new List<DescriptorSetLayoutDefinition>();
            Settings.Descriptors.Add(binding, tuples);
        }
        Settings.Descriptors[binding].Add(new DescriptorSetLayoutDefinition(descriptorType, shaderStageFlags, count));
        return this;
    }
    #endregion

    public DescriptorSetLayoutBuilder Reset(VulkanLogicalDevice vulkanLogicalDevice)
    {
        Settings.Reset(vulkanLogicalDevice);
        return this;
    }

    public DescriptorSetLayoutBuilder Reset()
    {
        Settings.Reset();
        return this;
    }

    internal class BuilderSettings
    {
        public VulkanLogicalDevice VulkanLogicalDevice { get; set; }

        public Dictionary<uint, List<DescriptorSetLayoutDefinition>> Descriptors { get; set; } = new Dictionary<uint, List<DescriptorSetLayoutDefinition>>();

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
            Descriptors.Clear();
        }
    }

    internal class DescriptorSetLayoutDefinition
    {
        public DescriptorType DescriptorType { get; set; }
        public ShaderStageFlags ShaderStageFlags { get; set; }
        public uint Count { get; set; }

        public DescriptorSetLayoutDefinition(DescriptorType descriptorType, ShaderStageFlags shaderStageFlags, uint count)
        {
            DescriptorType = descriptorType;
            ShaderStageFlags = shaderStageFlags;
            Count = count;
        }
    }
}

public class VulkanDescriptorSetLayout : VkObject
{
    internal VulkanInstance VulkanInstance => VulkanLogicalDevice.VulkanInstance;
    internal VulkanLogicalDevice VulkanLogicalDevice { get; init; }

    internal Dictionary<uint, List<DescriptorSetLayoutBuilder.DescriptorSetLayoutDefinition>> DescriptorDefinitions { get; init; }



    public DescriptorSetLayout DescriptorSetLayout { get; init; }

    internal unsafe VulkanDescriptorSetLayout(DescriptorSetLayoutBuilder.BuilderSettings settings)
    {
        VulkanLogicalDevice = settings.VulkanLogicalDevice;
        DescriptorDefinitions = new Dictionary<uint, List<DescriptorSetLayoutBuilder.DescriptorSetLayoutDefinition>>(settings.Descriptors);

        var descriptorSetLayoutBindings = new List<DescriptorSetLayoutBinding>();

        foreach (var binding in settings.Descriptors.Keys)
        {
            foreach (var descriptorSetLayoutDefinition in DescriptorDefinitions[binding])
            {
                descriptorSetLayoutBindings.Add(new DescriptorSetLayoutBinding()
                {
                    Binding = binding,
                    DescriptorCount = (uint)DescriptorDefinitions[binding].Count,
                    DescriptorType = descriptorSetLayoutDefinition.DescriptorType,
                    PImmutableSamplers = null,
                    StageFlags = descriptorSetLayoutDefinition.ShaderStageFlags,
                });
            }
        }

        var bindings = descriptorSetLayoutBindings.ToArray();
        var descriptorSetLayout = new DescriptorSetLayout();

        fixed (DescriptorSetLayoutBinding* pBindings = bindings)
        {
            var layoutInfo = new DescriptorSetLayoutCreateInfo()
            {
                SType = StructureType.DescriptorSetLayoutCreateInfo,
                BindingCount = (uint)bindings.Length,
                PBindings = pBindings,
            };

            if (VkFunc.CreateDescriptorSetLayout(VulkanLogicalDevice, layoutInfo, VulkanInstance.AllocationCallbacks, out descriptorSetLayout) != Result.Success)
            {
                throw new Exception("Failed to create descriptor set layout!");
            }
        }

        DescriptorSetLayout = descriptorSetLayout;
    }

    protected override void Dispose(bool disposing)
    {
        VkFunc.DestroyDescriptorSetLayout(VulkanLogicalDevice, DescriptorSetLayout, VulkanInstance.AllocationCallbacks);

        base.Dispose(disposing);
    }

    public static implicit operator DescriptorSetLayout(VulkanDescriptorSetLayout v) => v.DescriptorSetLayout;
}