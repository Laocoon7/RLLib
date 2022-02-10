using Silk.NET.Core;
using Silk.NET.Vulkan;

using RLLib.VulkanInfo;

namespace RLLib.VulkanBuilders;

public class QueueBuilder
{
    private BuilderSettings Settings;

    public QueueBuilder()
    {
        Settings = new BuilderSettings();
    }

    public VulkanQueue Build() => new VulkanQueue(Settings);

    #region Builder
    public QueueBuilder RequirePresent(bool require = true)
    {
        Settings.RequirePresent = require;
        return this;
    }

    public QueueBuilder RequireGraphics(bool require = true)
    {
        Settings.RequireGraphics = require;
        return this;
    }

    public QueueBuilder RequireCompute(bool require = true)
    {
        Settings.RequireCompute = require;
        return this;
    }

    public QueueBuilder RequireTransfer(bool require = true)
    {
        Settings.RequireTransfer = require;
        return this;
    }

    public QueueBuilder RequireEncode(bool require = true)
    {
        Settings.RequireEncode = require;
        return this;
    }

    public QueueBuilder RequireDecode(bool require = true)
    {
        Settings.RequireDecode = require;
        return this;
    }

    public QueueBuilder RequireProtected(bool require = true)
    {
        Settings.RequireProtected = require;
        return this;
    }

    public QueueBuilder RequireSparseBinding(bool require = true)
    {
        Settings.RequireSparseBinding = require;
        return this;
    }

    public QueueBuilder NoPresent()
    {
        Settings.NoPresent = true;
        return this;
    }

    public QueueBuilder NoGraphics()
    {
        Settings.NoGraphics = true;
        return this;
    }

    public QueueBuilder NoCompute()
    {
        Settings.NoCompute = true;
        return this;
    }

    public QueueBuilder NoTransfer()
    {
        Settings.NoTransfer = true;
        return this;
    }

    public QueueBuilder NoEncode()
    {
        Settings.NoEncode = true;
        return this;
    }

    public QueueBuilder NoDecode()
    {
        Settings.NoDecode = true;
        return this;
    }

    public QueueBuilder NoProtected()
    {
        Settings.NoProtected = true;
        return this;
    }

    public QueueBuilder NoSparseBinding()
    {
        Settings.NoSparseBinding = true;
        return this;
    }

    public QueueBuilder SetPriority(float priority)
    {
        Settings.Priority = priority;
        return this;
    }
    #endregion

    public QueueBuilder Reset()
    {
        Settings.Reset();
        return this;
    }

    internal class BuilderSettings
    {
        public bool RequirePresent { get; set; } = false;
        public bool RequireGraphics { get; set; } = false;
        public bool RequireCompute { get; set; } = false;
        public bool RequireTransfer { get; set; } = false;
        public bool RequireEncode { get; set; } = false;
        public bool RequireDecode { get; set; } = false;
        public bool RequireProtected { get; set; } = false;
        public bool RequireSparseBinding { get; set; } = false;

        public bool NoPresent { get; set; } = false;
        public bool NoGraphics { get; set; } = false;
        public bool NoCompute { get; set; } = false;
        public bool NoTransfer { get; set; } = false;
        public bool NoEncode { get; set; } = false;
        public bool NoDecode { get; set; } = false;
        public bool NoProtected { get; set; } = false;
        public bool NoSparseBinding { get; set; } = false;

        public float Priority { get; set; } = 1.0f;

        public void Reset()
        {
            RequirePresent = false;
            RequireGraphics = false;
            RequireCompute = false;
            RequireTransfer = false;
            RequireEncode = false;
            RequireDecode = false;
            RequireProtected = false;
            RequireSparseBinding = false;

            NoPresent = false;
            NoGraphics = false;
            NoCompute = false;
            NoTransfer = false;
            NoEncode = false;
            NoDecode = false;
            NoProtected = false;
            NoSparseBinding = false;

            Priority = 1.0f;
        }
    }
}

public class VulkanQueue : VkObject
{
    public bool RequirePresent { get; init; }
    public bool RequireGraphics { get; init; }
    public bool RequireCompute { get; init; }
    public bool RequireTransfer { get; init; }
    public bool RequireEncode { get; init; }
    public bool RequireDecode { get; init; }
    public bool RequireProtected { get; init; }
    public bool RequireSparseBinding { get; init; }

    public bool NoPresent { get; init; }
    public bool NoGraphics { get; init; }
    public bool NoCompute { get; init; }
    public bool NoTransfer { get; init; }
    public bool NoEncode { get; init; }
    public bool NoDecode { get; init; }
    public bool NoProtected { get; init; }
    public bool NoSparseBinding { get; init; }

    public float Priority { get; init; }



    internal VulkanLogicalDevice? VulkanLogicalDevice { get; set; } = null;
    internal uint QueueFamilyIndex { get; set; } = 0;
    internal uint QueueIndex { get; set; } = 0;
    private Queue? Queue { get; set; } = null;

    internal VulkanQueue(QueueBuilder.BuilderSettings settings)
    {
        RequirePresent = settings.RequirePresent;
        RequireGraphics = settings.RequireGraphics;
        RequireCompute = settings.RequireCompute;
        RequireTransfer = settings.RequireTransfer;
        RequireEncode = settings.RequireEncode;
        RequireDecode = settings.RequireDecode;
        RequireProtected = settings.RequireProtected;
        RequireSparseBinding = settings.RequireSparseBinding;

        NoPresent = settings.NoPresent;
        NoGraphics = settings.NoGraphics;
        NoCompute = settings.NoCompute;
        NoTransfer = settings.NoTransfer;
        NoEncode = settings.NoEncode;
        NoDecode = settings.NoDecode;
        NoProtected = settings.NoProtected;
        NoSparseBinding = settings.NoSparseBinding;

        Priority = settings.Priority;
    }

    public uint? GetQueueIndex(PhysicalDevice physicalDevice, IEnumerable<QueueFamilyProperties> queueFamilyProperties, VulkanSurface? vulkanSurface)
    {
        VulkanSurface? surface = vulkanSurface;

        if (RequirePresent && surface == null)
        {
            return null;
        }

        var queueFamilies = queueFamilyProperties.ToList();

        for (int i = 0; i < queueFamilies.Count; i++)
        {
            Bool32 presentSupport = false;
            if (surface != null)
            {
                surface.KhrSurface.GetPhysicalDeviceSurfaceSupport(physicalDevice, (uint)i, surface.Surface, out presentSupport);
            }

            if (RequirePresent && !presentSupport)
                continue;
            if (RequireGraphics && !queueFamilies[i].QueueFlags.HasFlag(QueueFlags.QueueGraphicsBit))
                continue;
            if (RequireCompute && !queueFamilies[i].QueueFlags.HasFlag(QueueFlags.QueueComputeBit))
                continue;
            if (RequireTransfer && !queueFamilies[i].QueueFlags.HasFlag(QueueFlags.QueueTransferBit))
                continue;
            if (RequireEncode && !queueFamilies[i].QueueFlags.HasFlag(QueueFlags.QueueVideoEncodeBitKhr))
                continue;
            if (RequireDecode && !queueFamilies[i].QueueFlags.HasFlag(QueueFlags.QueueVideoDecodeBitKhr))
                continue;
            if (RequireProtected && !queueFamilies[i].QueueFlags.HasFlag(QueueFlags.QueueProtectedBit))
                continue;
            if (RequireSparseBinding && !queueFamilies[i].QueueFlags.HasFlag(QueueFlags.QueueSparseBindingBit))
                continue;

            if (NoPresent && presentSupport)
                continue;
            if (NoGraphics && queueFamilies[i].QueueFlags.HasFlag(QueueFlags.QueueGraphicsBit))
                continue;
            if (NoCompute && queueFamilies[i].QueueFlags.HasFlag(QueueFlags.QueueComputeBit))
                continue;
            if (NoTransfer && queueFamilies[i].QueueFlags.HasFlag(QueueFlags.QueueTransferBit))
                continue;
            if (NoEncode && queueFamilies[i].QueueFlags.HasFlag(QueueFlags.QueueVideoEncodeBitKhr))
                continue;
            if (NoDecode && queueFamilies[i].QueueFlags.HasFlag(QueueFlags.QueueVideoDecodeBitKhr))
                continue;
            if (NoProtected && queueFamilies[i].QueueFlags.HasFlag(QueueFlags.QueueProtectedBit))
                continue;
            if (NoSparseBinding && queueFamilies[i].QueueFlags.HasFlag(QueueFlags.QueueSparseBindingBit))
                continue;

            return (uint)i;
        }
        return null;
    }

    public Queue GetQueue()
    {
        if (Queue != null)
        {
            return Queue.Value;
        }

        if (VulkanLogicalDevice == null)
        {
            throw new Exception($"Failed to get queue, logical device not created.");
        }

        VkFunc.GetDeviceQueue(VulkanLogicalDevice, QueueFamilyIndex, QueueIndex, out var queue);
        Queue = queue;

        return queue;
    }

    public static implicit operator Queue(VulkanQueue q) => q.GetQueue();
}
