using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.KHR;

using RLLib.VulkanInfo;

namespace RLLib.VulkanBuilders;

public class PhysicalDeviceBuilder
{
    private BuilderSettings Settings;

    public PhysicalDeviceBuilder(VulkanInstance vulkanInstance, VulkanSurface? vulkanSurface)
    {
        Settings = new BuilderSettings(vulkanInstance, vulkanSurface);
    }

    public VulkanPhysicalDevice Build() => new VulkanPhysicalDevice(Settings);

    #region Builder
    public PhysicalDeviceBuilder SetPerferredDeviceType(PhysicalDeviceType type)
    {
        Settings.PreferredDeviceType = type;
        return this;
    }

    public PhysicalDeviceBuilder DisallowDeviceType(PhysicalDeviceType type)
    {
        Settings.DisallowedDeviceTypes.Add(type);
        return this;
    }

    public PhysicalDeviceBuilder AddRequiredQueue(VulkanQueue queue)
    {
        Settings.RequiredQueues.Add(queue);
        return this;
    }

    public PhysicalDeviceBuilder RequireMemorySize(ulong size)
    {
        Settings.RequiredMemory = size;
        return this;
    }

    public PhysicalDeviceBuilder DesireMemorySize(ulong size)
    {
        Settings.DesiredMemory = size;
        return this;
    }

    public PhysicalDeviceBuilder AddRequiredExtension(string extensionName)
    {
        Settings.RequiredExtensions.Add(extensionName);
        return this;
    }

    public PhysicalDeviceBuilder AddDesiredExtension(string extensionName)
    {
        Settings.DesiredExtensions.Add(extensionName);
        return this;
    }

    public PhysicalDeviceBuilder SetRequiredFeatures(PhysicalDeviceFeatures features)
    {
        Settings.RequiredFeatures = features;
        return this;
    }

    /// <summary>
    /// TODO: Checks NotImplemented in <see cref="VulkanPhysicalDeviceDescription"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="features"></param>
    /// <returns></returns>
    //public PhysicalDeviceBuilder AddRequiredExtensionFeature<T>(T features) where T : IExtendsChain<DeviceCreateInfo>
    //{
    //    Settings.ExtendedFeatures.Add(features);
    //    return this;
    //}

    public PhysicalDeviceBuilder SelectFirstDeviceUnconditionally(bool unconditionally)
    {
        Settings.UseFirstDeviceUnconditionally = unconditionally;
        return this;
    }
    #endregion

    public PhysicalDeviceBuilder Reset(VulkanInstance vulkanInstance, VulkanSurface? vulkanSurface)
    {
        Settings.Reset(vulkanInstance, vulkanSurface);
        return this;
    }

    public PhysicalDeviceBuilder Reset()
    {
        Settings.Reset();
        return this;
    }

    internal class BuilderSettings
    {
        public VulkanInstance VulkanInstance { get; set; }
        public VulkanSurface? VulkanSurface { get; set; } = null;

        public List<VulkanQueue> RequiredQueues { get; set; } = new List<VulkanQueue>();

        public PhysicalDeviceType PreferredDeviceType { get; set; } = PhysicalDeviceType.DiscreteGpu;
        public List<PhysicalDeviceType> DisallowedDeviceTypes { get; set; } = new List<PhysicalDeviceType>();


        public ulong RequiredMemory { get; set; } = 0;
        public ulong DesiredMemory { get; set; } = 0;

        public List<string> RequiredExtensions { get; set; } = new List<string>();
        public List<string> DesiredExtensions { get; set; } = new List<string>();

        public PhysicalDeviceFeatures RequiredFeatures { get; set; } = new PhysicalDeviceFeatures();
        //public List<IExtendsChain<DeviceCreateInfo>> ExtendedFeatures { get; set; } = new List<IExtendsChain<DeviceCreateInfo>>();

        public bool UseFirstDeviceUnconditionally { get; set; } = false;

        public BuilderSettings(VulkanInstance vulkanInstance, VulkanSurface? vulkanSurface)
        {
            VulkanInstance = vulkanInstance;
            VulkanSurface = vulkanSurface;
        }

        public void Reset(VulkanInstance vulkanInstance, VulkanSurface? vulkanSurface)
        {
            VulkanInstance = vulkanInstance;
            VulkanSurface = vulkanSurface;

            Reset();
        }

        public void Reset()
        {
            RequiredQueues.Clear();
            PreferredDeviceType = PhysicalDeviceType.DiscreteGpu;
            DisallowedDeviceTypes.Clear();

            RequiredMemory = 0;
            DesiredMemory = 0;

            RequiredExtensions.Clear();

            UseFirstDeviceUnconditionally = false;
        }
    }
}

public class VulkanPhysicalDevice : VkObject
{
    internal VulkanInstance VulkanInstance { get; init; }
    internal VulkanSurface? VulkanSurface { get; init; }

    public PhysicalDevice PhysicalDevice { get; init; }

    private List<QueueFamilyProperties> m_QueueFamilyProperties { get; init; }
    public List<QueueFamilyProperties> QueueFamilyProperties => new List<QueueFamilyProperties>(m_QueueFamilyProperties);

    public PhysicalDeviceFeatures RequiredFeatures { get; init; }

    private List<string> m_ExtensionsToEnable { get; init; }
    public List<string> ExtensionsToEnable => new List<string>(m_ExtensionsToEnable);

    private List<VulkanQueue> m_QueueDefinitions { get; init; }
    public List<VulkanQueue> QueueDefinitions => new List<VulkanQueue>(m_QueueDefinitions);


    internal VulkanPhysicalDevice(PhysicalDeviceBuilder.BuilderSettings settings)
    {
        VulkanInstance = settings.VulkanInstance;
        VulkanSurface = settings.VulkanSurface;

        if (VulkanSurface != null)
        {
            settings.RequiredExtensions.Add(KhrSwapchain.ExtensionName);
        }

        var physicalDevices = VkFunc.EnumeratePhysicalDevices(VulkanInstance);

        if (physicalDevices.Length == 0)
        {
            throw new Exception($"No devices found.");
        }

        PhysicalDevice? selectedDevice = null;

        if (settings.UseFirstDeviceUnconditionally)
        {
            selectedDevice = physicalDevices[0];
        }
        else
        {
            foreach (var device in physicalDevices)
            {
                var suitable = IsSuitable(device, settings);
                if (suitable == Suitable.Yes)
                {
                    selectedDevice = device;
                    break;
                }
                else if (suitable == Suitable.Partial)
                {
                    selectedDevice = device;
                }
            }
        }

        if (selectedDevice == null)
        {
            throw new Exception($"No suitable device found.");
        }


        PhysicalDevice = selectedDevice.Value;

        m_QueueFamilyProperties = VkFunc.GetPhysicalDeviceQueueFamilyProperties(PhysicalDevice).ToList();

        RequiredFeatures = settings.RequiredFeatures;

        // Extensions
        var deviceExtensions = VkUtility.GetNames(VkUtility.EnumerateAllPhysicalDeviceExtensionProperties(PhysicalDevice));
        var desiredExtensions = new List<string>(settings.DesiredExtensions);

        foreach (var extension in settings.DesiredExtensions)
        {
            if (!deviceExtensions.Contains(extension))
            {
                desiredExtensions.Remove(extension);
            }
        }

        m_ExtensionsToEnable = new List<string>(settings.RequiredExtensions);
        m_ExtensionsToEnable.AddRange(desiredExtensions);

        m_QueueDefinitions = new List<VulkanQueue>(settings.RequiredQueues);
    }

    private enum Suitable
    {
        Yes,
        Partial,
        No
    }
    private Suitable IsSuitable(PhysicalDevice device, PhysicalDeviceBuilder.BuilderSettings settings)
    {
        var suitable = Suitable.Yes;
        VulkanInstance VulkanInstance = settings.VulkanInstance!;
        VulkanSurface? VulkanSurface = settings.VulkanSurface;

        var Properties = VkFunc.GetPhysicalDeviceProperties(device);
        var ApiVersion = Properties.ApiVersion;

        if (VulkanInstance.RequiredApiVersion > ApiVersion) return Suitable.No;
        if (VulkanInstance.DesiredApiVersion > ApiVersion) suitable = Suitable.Partial;

        bool requirePresent = false;
        foreach (var definition in settings.RequiredQueues)
        {
            requirePresent |= definition.RequirePresent;
            if (!definition.GetQueueIndex(device, VkFunc.GetPhysicalDeviceQueueFamilyProperties(device), VulkanSurface).HasValue)
                return Suitable.No;
        }

        var deviceExtensions = VkUtility.GetNames(VkUtility.EnumerateAllPhysicalDeviceExtensionProperties(device));

        foreach (var extension in settings.RequiredExtensions)
        {
            if (!deviceExtensions.Contains(extension))
            {
                return Suitable.No;
            }
        }

        bool swapChainAdequate = false;
        if (VulkanSurface != null)
        {
            var formats = VulkanSurface.GetSurfaceFormats(device);
            var presentModes = VulkanSurface.GetPresentModes(device);

            swapChainAdequate = formats.Any() && presentModes.Any();
        }

        if (requirePresent && !swapChainAdequate) return Suitable.No;

        if (Properties.DeviceType != settings.PreferredDeviceType)
        {
            if (settings.DisallowedDeviceTypes.Contains(Properties.DeviceType))
            {
                return Suitable.No;
            }
            suitable = Suitable.Partial;
        }
        if (!VkUtility.SupportsFeatures(settings.RequiredFeatures, VkFunc.GetPhysicalDeviceFeatures(device)))
        {
            return Suitable.No;
        }

        // TODO: SupportsExtendedFeatures()
        /*
        
        if (!SupportsExtendedFeatures(settings.ExtendedFeatures))
        {
            return Suitable.No;
        }

        //*/

        bool hasRequiredMemory = false;
        bool hasDesiredMemory = false;
        var memoryProperties = VkFunc.GetPhysicalDeviceMemoryProperties(device);
        for (int i = 0; i < memoryProperties.MemoryHeapCount; i++)
        {
            if (memoryProperties.MemoryHeaps[i].Flags.HasFlag(MemoryHeapFlags.MemoryHeapDeviceLocalBit))
            {
                if (memoryProperties.MemoryHeaps[i].Size >= settings.RequiredMemory)
                {
                    hasRequiredMemory = true;
                }
                if (memoryProperties.MemoryHeaps[i].Size >= settings.DesiredMemory)
                {
                    hasDesiredMemory = true;
                }
            }
        }
        if (!hasRequiredMemory) return Suitable.No;
        if (!hasDesiredMemory) suitable = Suitable.Partial;

        return suitable;
    }


    public static implicit operator PhysicalDevice(VulkanPhysicalDevice v) => v.PhysicalDevice;
}
