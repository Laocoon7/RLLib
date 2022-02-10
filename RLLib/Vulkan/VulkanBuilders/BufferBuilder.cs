using System.Runtime.CompilerServices;

using Silk.NET.Vulkan;

using RLLib.VulkanInfo;

using Buffer = Silk.NET.Vulkan.Buffer;

namespace RLLib.VulkanBuilders;

public class BufferBuilder
{
    private BuilderSettings Settings;

    public BufferBuilder(VulkanLogicalDevice vulkanLogicalDevice)
    {
        Settings = new BuilderSettings(vulkanLogicalDevice);
    }

    public VulkanBuffer Build() => new VulkanBuffer(Settings);

    #region Builder
    public BufferBuilder SetBufferUsageFlags(BufferUsageFlags flags)
    {
        Settings.BufferUsageFlags = flags;
        return this;
    }

    public BufferBuilder SetMemoryPropertyFlags(MemoryPropertyFlags flags)
    {
        Settings.MemoryPropertyFlags = flags;
        return this;
    }

    public BufferBuilder SetSharingMode(SharingMode mode)
    {
        Settings.SharingMode = mode;
        return this;
    }

    public BufferBuilder SetSize(ulong size)
    {
        Settings.Size = size;
        return this;
    }
    #endregion

    public BufferBuilder Reset(VulkanLogicalDevice vulkanLogicalDevice)
    {
        Settings.Reset(vulkanLogicalDevice);
        return this;
    }

    public BufferBuilder Reset()
    {
        Settings.Reset();
        return this;
    }

    internal class BuilderSettings
    {
        public VulkanLogicalDevice VulkanLogicalDevice { get; set; }

        public BufferUsageFlags BufferUsageFlags { get; set; }
        public MemoryPropertyFlags MemoryPropertyFlags { get; set; }
        public SharingMode SharingMode { get; set; } = SharingMode.Exclusive;

        public ulong Size { get; set; } = 0;


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
            BufferUsageFlags = 0;
            MemoryPropertyFlags = 0;
            SharingMode = SharingMode.Exclusive;
            Size = 0;
        }
    }
}

public class VulkanBuffer : VkObject
{
    internal VulkanInstance VulkanInstance => VulkanLogicalDevice.VulkanInstance;
    internal VulkanPhysicalDevice VulkanPhysicalDevice => VulkanLogicalDevice.VulkanPhysicalDevice;
    internal VulkanLogicalDevice VulkanLogicalDevice { get; init; }

    public Buffer Buffer;
    public DeviceMemory BufferMemory;

    internal VulkanBuffer(BufferBuilder.BuilderSettings settings)
    {
        VulkanLogicalDevice = settings.VulkanLogicalDevice;

        var bufferInfo = new BufferCreateInfo()
        {
            SType = StructureType.BufferCreateInfo,
            Size = settings.Size,
            Usage = settings.BufferUsageFlags,
            SharingMode = settings.SharingMode,
        };

        if (VkFunc.CreateBuffer(VulkanLogicalDevice, bufferInfo, VulkanInstance.AllocationCallbacks, out var buffer) != Result.Success)
        {
            throw new Exception("Failed to create buffer!");
        }
        Buffer = buffer;

        var memRequirements = new MemoryRequirements();
        VkFunc.GetBufferMemoryRequirements(VulkanLogicalDevice, Buffer, out memRequirements);

        var allocateInfo = new MemoryAllocateInfo()
        {
            SType = StructureType.MemoryAllocateInfo,
            AllocationSize = memRequirements.Size,
            MemoryTypeIndex = VkUtility.FindMemoryType(VulkanPhysicalDevice, memRequirements.MemoryTypeBits, settings.MemoryPropertyFlags),
        };

        if (VkFunc.AllocateMemory(VulkanLogicalDevice, allocateInfo, VulkanInstance.AllocationCallbacks, out var bufferMemory) != Result.Success)
        {
            throw new Exception("Failed to allocate buffer memory!");
        }
        BufferMemory = bufferMemory;

        VkFunc.BindBufferMemory(VulkanLogicalDevice, Buffer, BufferMemory, 0);
    }

    public unsafe void* Map(ulong offset = 0, ulong size = Vk.WholeSize, uint flags = 0)
    {
        void* data;
        var result = VkFunc.MapMemory(VulkanLogicalDevice, BufferMemory, offset, size, flags, &data);
        if (result != Result.Success)
        {
            throw new Exception("Failed to map memory.");
        }
        return data;
    }

    public void Unmap()
    {
        VkFunc.UnmapMemory(VulkanLogicalDevice, BufferMemory);
    }

    public unsafe void Write<T>(Span<T> data, ulong offset = 0, ulong length = Vk.WholeSize, uint flags = 0)
    {
        var pData = Map(offset, length, flags);
        data.CopyTo(new Span<T>(pData, data.Length));
        Unmap();
    }

    public void CopyTo(VulkanBuffer destination, VulkanCommandPool vulkanCommandPool, ulong sourceOffset = 0, ulong destinationOffset = 0, ulong size = Vk.WholeSize)
    {
        var commandBuffer = new VulkanSingleTimeCommandBuffer(vulkanCommandPool);

        var copyRegion = new BufferCopy()
        {
            SrcOffset = sourceOffset,
            DstOffset = destinationOffset,
            Size = size,
        };

        commandBuffer.CmdCopyBuffer(Buffer, destination, copyRegion);

        commandBuffer.End();
    }

    protected override void Dispose(bool disposing)
    {
        VkFunc.FreeMemory(VulkanLogicalDevice, BufferMemory, VulkanInstance.AllocationCallbacks);
        VkFunc.DestroyBuffer(VulkanLogicalDevice, Buffer, VulkanInstance.AllocationCallbacks);
        base.Dispose(disposing);
    }

    public static implicit operator Buffer(VulkanBuffer v) => v.Buffer;
    public static implicit operator DeviceMemory(VulkanBuffer v) => v.BufferMemory;
}