using Silk.NET.Core.Native;
using Silk.NET.Vulkan;

using System.Runtime.CompilerServices;

using RLLib.VulkanInfo;

namespace RLLib.VulkanBuilders;

public class LogicalDeviceBuilder
{
    private BuilderSettings Settings;

    public LogicalDeviceBuilder(VulkanPhysicalDevice vulkanPhysicalDevice)
    {
        Settings = new BuilderSettings(vulkanPhysicalDevice);
    }

    public VulkanLogicalDevice Build() => new VulkanLogicalDevice(Settings);

    #region Builder
    #endregion

    public LogicalDeviceBuilder Reset(VulkanPhysicalDevice vulkanPhysicalDevice)
    {
        Settings.Reset(vulkanPhysicalDevice);
        return this;
    }

    public LogicalDeviceBuilder Reset()
    {
        Settings.Reset();
        return this;
    }

    internal class BuilderSettings
    {
        public VulkanPhysicalDevice VulkanPhysicalDevice { get; set; }

        public BuilderSettings(VulkanPhysicalDevice vulkanPhysicalDevice)
        {
            VulkanPhysicalDevice = vulkanPhysicalDevice;
        }

        public void Reset(VulkanPhysicalDevice vulkanPhysicalDevice)
        {
            VulkanPhysicalDevice = vulkanPhysicalDevice;

            Reset();
        }

        public void Reset()
        {

        }
    }
}

public class VulkanLogicalDevice : VkObject
{
    internal VulkanInstance VulkanInstance => VulkanPhysicalDevice.VulkanInstance;
    internal VulkanSurface? VulkanSurface => VulkanPhysicalDevice.VulkanSurface;
    internal VulkanPhysicalDevice VulkanPhysicalDevice { get; init; }

    public Device Device;

    internal unsafe VulkanLogicalDevice(LogicalDeviceBuilder.BuilderSettings settings)
    {
        VulkanPhysicalDevice = settings.VulkanPhysicalDevice!;

        var familyDescriptions = new Dictionary<uint, FamilyDescription>();
        foreach (var definition in VulkanPhysicalDevice.QueueDefinitions)
        {
            var family = definition.GetQueueIndex(VulkanPhysicalDevice, VulkanPhysicalDevice.QueueFamilyProperties, VulkanSurface);
            if (family.HasValue)
            {
                if (!familyDescriptions.TryGetValue(family.Value, out var description))
                {
                    description = new FamilyDescription()
                    {
                        Family = family.Value,
                        MaxIndices = VulkanPhysicalDevice.QueueFamilyProperties[(int)family.Value].QueueCount,
                        CurrentIndex = 0,
                    };
                    familyDescriptions.Add(family.Value, description);
                }

                if (description.CurrentIndex >= description.MaxIndices)
                {
                    throw new Exception($"Too many queue's are being requested from the same family.");
                }

                definition.VulkanLogicalDevice = this;
                definition.QueueFamilyIndex = family.Value;
                definition.QueueIndex = description.CurrentIndex;
                description.Priorities.Add(Math.Min(1, Math.Max(0, definition.Priority)));

                description.CurrentIndex++;
            }
        }


        using var mem = GlobalMemory.Allocate(familyDescriptions.Keys.Count * sizeof(DeviceQueueCreateInfo));
        var queueCreateInfos = (DeviceQueueCreateInfo*)Unsafe.AsPointer(ref mem.GetPinnableReference());

        uint i = 0;
        foreach (var key in familyDescriptions.Keys)
        {
            var priorities = familyDescriptions[key].Priorities.ToArray();
            fixed (float* pPriorities = priorities)
            {

                queueCreateInfos[i] = new DeviceQueueCreateInfo()
                {
                    SType = StructureType.DeviceQueueCreateInfo,
                    Flags = 0,
                    QueueFamilyIndex = key,
                    QueueCount = familyDescriptions[key].CurrentIndex,
                    PQueuePriorities = pPriorities,
                    PNext = null
                };
            }
        }
        var deviceFeatures = VulkanPhysicalDevice.RequiredFeatures;

        var deviceCreateInfo = new DeviceCreateInfo()
        {
            SType = StructureType.DeviceCreateInfo,
            Flags = 0,
            QueueCreateInfoCount = (uint)familyDescriptions.Count,
            PQueueCreateInfos = queueCreateInfos,
            EnabledExtensionCount = (uint)VulkanPhysicalDevice.ExtensionsToEnable.Count,
            PpEnabledExtensionNames = (byte**)SilkMarshal.StringArrayToPtr(VulkanPhysicalDevice.ExtensionsToEnable),
            EnabledLayerCount = 0, // Deprecated
            PpEnabledLayerNames = null, // Deprecated
            PEnabledFeatures = &deviceFeatures,
            PNext = null,
        };

        // TODO:
        /*
        foreach (var extendedFeature in VulkanPhysicalDevice.SelectorSettings.ExtendedFeatures)
        {
            IExtendsChain<DeviceCreateInfo> feature = extendedFeature;
            deviceCreateInfo.SetNext(ref feature);
        }
        //*/

        if (VkFunc.CreateDevice(VulkanPhysicalDevice, deviceCreateInfo, VulkanInstance.AllocationCallbacks, out var device) != Result.Success)
        {
            throw new Exception("Failed to create logical device!");
        }

        Device = device;

        SilkMarshal.Free((nint)deviceCreateInfo.PpEnabledExtensionNames);
    }

    protected override void Dispose(bool disposing)
    {
        VkFunc.DestroyDevice(Device, VulkanInstance.AllocationCallbacks);

        base.Dispose(disposing);
    }

    public static implicit operator Device(VulkanLogicalDevice d) => d.Device;

    private class FamilyDescription
    {
        public uint Family { get; init; }
        public uint MaxIndices { get; init; }
        public uint CurrentIndex { get; set; }
        public List<float> Priorities { get; init; } = new List<float>();
    }
}
