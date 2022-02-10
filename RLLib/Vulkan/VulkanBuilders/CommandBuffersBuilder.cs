using Silk.NET.OpenAL;
using Silk.NET.Vulkan;

using RLLib.VulkanInfo;

using Buffer = Silk.NET.Vulkan.Buffer;

namespace RLLib.VulkanBuilders;

public class CommandBuffersBuilder
{
    private BuilderSettings Settings;

    public CommandBuffersBuilder(VulkanCommandPool vulkanCommandPool)
    {
        Settings = new BuilderSettings(vulkanCommandPool);
    }

    public VulkanCommandBuffers Build() => new VulkanCommandBuffers(Settings);

    #region Builder
    public CommandBuffersBuilder SetCount(uint count)
    {
        Settings.Count = count;
        return this;
    }
    #endregion

    public CommandBuffersBuilder Reset(VulkanCommandPool vulkanCommandPool)
    {
        Settings.Reset(vulkanCommandPool);
        return this;
    }

    public CommandBuffersBuilder Reset()
    {
        Settings.Reset();
        return this;
    }

    internal class BuilderSettings
    {
        public VulkanCommandPool VulkanCommandPool { get; set; }

        public uint Count { get; set; } = 1;


        public BuilderSettings(VulkanCommandPool vulkanCommandPool)
        {
            VulkanCommandPool = vulkanCommandPool;
        }

        public void Reset(VulkanCommandPool vulkanCommandPool)
        {
            VulkanCommandPool = vulkanCommandPool;

            Reset();
        }

        public void Reset()
        {
            Count = 1;
        }
    }
}

public class VulkanCommandBuffers : VkObject
{
    internal VulkanLogicalDevice VulkanLogicalDevice => VulkanCommandPool.VulkanLogicalDevice;
    internal VulkanQueue VulkanQueue => VulkanCommandPool.VulkanQueue;
    internal VulkanCommandPool VulkanCommandPool { get; init; }


    public CommandBuffer[] CommandBuffers { get; init; }

    internal unsafe VulkanCommandBuffers(CommandBuffersBuilder.BuilderSettings settings)
    {
        VulkanCommandPool = settings.VulkanCommandPool;

        var allocInfo = new CommandBufferAllocateInfo()
        {
            SType = StructureType.CommandBufferAllocateInfo,
            CommandPool = VulkanCommandPool,
            Level = CommandBufferLevel.Primary,
            CommandBufferCount = settings.Count,
        };

        if (VkFunc.AllocateCommandBuffers(VulkanLogicalDevice, allocInfo, out var commandBuffers) != Result.Success)
        {
            throw new Exception("Failed to allocate command buffers!");
        }
        CommandBuffers = commandBuffers;
    }

    public VulkanCommandBuffer this[int index] => new VulkanCommandBuffer(CommandBuffers[index]);
    public VulkanCommandBuffer this[uint index] => new VulkanCommandBuffer(CommandBuffers[index]);

    protected override void Dispose(bool disposing)
    {
        VkFunc.FreeCommandBuffers(VulkanLogicalDevice, VulkanCommandPool, CommandBuffers);

        base.Dispose(disposing);
    }

    public static implicit operator CommandBuffer[](VulkanCommandBuffers v) => v.CommandBuffers;
}

public abstract class CommandBufferBase
{
    public abstract CommandBuffer CommandBuffer { get; init; }

    #region Cmd
    public void CmdCopyBuffer(Buffer source, Buffer destination, params BufferCopy[] regions) =>
        VkFunc.CmdCopyBuffer(CommandBuffer, source, destination, regions);

    public void CmdCopyBufferToImage(Buffer source, Image destination, ImageLayout imageLayout, uint regionCount, BufferImageCopy region) =>
        VkFunc.CmdCopyBufferToImage(CommandBuffer, source, destination, imageLayout, regionCount, region);

    public void CmdPipelineBarrier(PipelineStageFlags sourceStage, PipelineStageFlags destinationStage, DependencyFlags dependencyFlags, MemoryBarrier[]? memoryBarriers, BufferMemoryBarrier[]? bufferMemoryBarriers, ImageMemoryBarrier[]? imageMemoryBarriers) =>
        VkFunc.CmdPipelineBarrier(CommandBuffer, sourceStage, destinationStage, dependencyFlags, memoryBarriers, bufferMemoryBarriers, imageMemoryBarriers);

    public void CmdBeginRenderPass(RenderPassBeginInfo renderPassBeginInfo, SubpassContents subpassContents) =>
        VkFunc.CmdBeginRenderPass(CommandBuffer, renderPassBeginInfo, subpassContents);

    public void CmdEndRenderPass() =>
        VkFunc.CmdEndRenderPass(CommandBuffer);

    public void CmdBindPipeline(PipelineBindPoint pipelineBindPoint, Pipeline pipeline) =>
        VkFunc.CmdBindPipeline(CommandBuffer, pipelineBindPoint, pipeline);

    public void CmdBindVertexBuffers(uint binding, Buffer[] vertexBuffer, ulong[] offsets) =>
        VkFunc.CmdBindVertexBuffers(CommandBuffer, binding, vertexBuffer, offsets);

    public void CmdBindIndexBuffer(Buffer indexBuffer, uint offset, IndexType indexType) =>
        VkFunc.CmdBindIndexBuffer(CommandBuffer, indexBuffer, offset, indexType);

    public void CmdBindDescriptorSets(PipelineBindPoint pipelineBindPoint, PipelineLayout pipelineLayout, uint firstSet, DescriptorSet[] descriptorSets, uint[]? dynamicOffsets) =>
        VkFunc.CmdBindDescriptorSets(CommandBuffer, pipelineBindPoint, pipelineLayout, firstSet, descriptorSets, dynamicOffsets);

    public void CmdDrawIndexed(uint numberOfIndices, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance) =>
        VkFunc.CmdDrawIndexed(CommandBuffer, numberOfIndices, instanceCount, firstIndex, vertexOffset, firstInstance);

    #endregion

}

public class VulkanCommandBuffer : CommandBufferBase
{
    public override CommandBuffer CommandBuffer { get; init; }

    internal VulkanCommandBuffer(CommandBuffer commandBuffer)
    {
        CommandBuffer = commandBuffer;
    }

    public Result ResetCommandBuffer(CommandBufferResetFlags flags) =>
        VkFunc.ResetCommandBuffer(CommandBuffer, flags);

    public Result BeginCommandBuffer(CommandBufferBeginInfo commandBufferBeginInfo) =>
        VkFunc.BeginCommandBuffer(CommandBuffer, commandBufferBeginInfo);

    public Result EndCommandBuffer() =>
        VkFunc.EndCommandBuffer(CommandBuffer);

    public static implicit operator CommandBuffer(VulkanCommandBuffer v) => v.CommandBuffer;
}

public class VulkanSingleTimeCommandBuffer : CommandBufferBase
{
    internal VulkanLogicalDevice VulkanLogicalDevice => VulkanCommandPool.VulkanLogicalDevice;
    internal VulkanQueue VulkanQueue => VulkanCommandPool.VulkanQueue;
    internal VulkanCommandPool VulkanCommandPool { get; init; }

    public override CommandBuffer CommandBuffer { get; init; }

    public VulkanSingleTimeCommandBuffer(VulkanCommandPool vulkanCommandPool)
    {
        VulkanCommandPool = vulkanCommandPool;

        var allocateInfo = new CommandBufferAllocateInfo()
        {
            SType = StructureType.CommandBufferAllocateInfo,
            Level = CommandBufferLevel.Primary,
            CommandPool = vulkanCommandPool,
            CommandBufferCount = 1,
        };

        VkFunc.AllocateCommandBuffers(vulkanCommandPool.VulkanLogicalDevice, allocateInfo, out var commandBuffer);

        var beginInfo = new CommandBufferBeginInfo()
        {
            SType = StructureType.CommandBufferBeginInfo,
            Flags = CommandBufferUsageFlags.CommandBufferUsageOneTimeSubmitBit,
        };

        VkFunc.BeginCommandBuffer(commandBuffer[0], beginInfo);

        CommandBuffer = commandBuffer[0];

    }
    public unsafe void End()
    {
        VkFunc.EndCommandBuffer(CommandBuffer);

        var commandBuffer = CommandBuffer;

        var submitInfo = new SubmitInfo()
        {
            SType = StructureType.SubmitInfo,
            CommandBufferCount = 1,
            PCommandBuffers = &commandBuffer,
        };

        VkFunc.QueueSubmit(VulkanQueue, default, submitInfo);
        VkFunc.QueueWaitIdle(VulkanQueue);

        VkFunc.FreeCommandBuffers(VulkanLogicalDevice, VulkanCommandPool, commandBuffer);
    }
}
