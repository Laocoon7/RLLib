using Silk.NET.Vulkan;

using RLLib.VulkanInfo;

namespace RLLib.VulkanBuilders;

public class RenderPassBuilder
{
    private BuilderSettings Settings;

    public RenderPassBuilder(VulkanSwapchain vulkanSwapchain)
    {
        Settings = new BuilderSettings(vulkanSwapchain);
    }
    public VulkanRenderPass Build() => new VulkanRenderPass(Settings);

    #region Builder
    #endregion

    public RenderPassBuilder Reset(VulkanSwapchain vulkanSwapchain)
    {
        Settings.Reset(vulkanSwapchain);
        return this;
    }

    public RenderPassBuilder Reset()
    {
        Settings.Reset();
        return this;
    }

	internal class BuilderSettings
    {
        public VulkanSwapchain VulkanSwapchain { get; set; }

        public BuilderSettings(VulkanSwapchain vulkanSwapchain)
        {
            VulkanSwapchain = vulkanSwapchain;
        }

        public void Reset(VulkanSwapchain vulkanSwapchain)
        {
            VulkanSwapchain = vulkanSwapchain;

            Reset();
        }

        public void Reset()
        {

        }
    }
}
public class VulkanRenderPass : VkObject
{
    internal VulkanInstance VulkanInstance => VulkanSwapchain.VulkanInstance;
    internal VulkanLogicalDevice VulkanLogicalDevice => VulkanSwapchain.VulkanLogicalDevice;
    internal VulkanSwapchain VulkanSwapchain { get; init; }

    public RenderPass RenderPass { get; init; }

    internal unsafe VulkanRenderPass(RenderPassBuilder.BuilderSettings settings)
    {
        VulkanSwapchain = settings.VulkanSwapchain;

        var colorAttachment = new AttachmentDescription()
        {
            Format = VulkanSwapchain.ImageFormat,
            Samples = SampleCountFlags.SampleCount1Bit,
            LoadOp = AttachmentLoadOp.Clear,
            StoreOp = AttachmentStoreOp.Store,
            StencilLoadOp = AttachmentLoadOp.DontCare,
            InitialLayout = ImageLayout.Undefined,
            FinalLayout = ImageLayout.PresentSrcKhr,
        };

        var colorAttachmentRef = new AttachmentReference()
        {
            Attachment = 0,
            Layout = ImageLayout.ColorAttachmentOptimal,
        };

        var subpass = new SubpassDescription()
        {
            PipelineBindPoint = PipelineBindPoint.Graphics,
            ColorAttachmentCount = 1,
            PColorAttachments = &colorAttachmentRef,
        };

        var dependency = new SubpassDependency()
        {
            SrcSubpass = Vk.SubpassExternal,
            DstSubpass = 0,
            SrcStageMask = PipelineStageFlags.PipelineStageColorAttachmentOutputBit,
            SrcAccessMask = 0,
            DstStageMask = PipelineStageFlags.PipelineStageColorAttachmentOutputBit,
            DstAccessMask = AccessFlags.AccessColorAttachmentWriteBit
        };

        var renderPassInfo = new RenderPassCreateInfo()
        {
            SType = StructureType.RenderPassCreateInfo,
            AttachmentCount = 1,
            PAttachments = &colorAttachment,
            SubpassCount = 1,
            PSubpasses = &subpass,
            DependencyCount = 1,
            PDependencies = &dependency,
};

        if (VkFunc.CreateRenderPass(VulkanLogicalDevice, renderPassInfo, VulkanInstance.AllocationCallbacks, out var renderPass) != Result.Success)
        {
            throw new Exception("Failed to create renderpass!");
        }

        RenderPass = renderPass;
    }

    protected override void Dispose(bool disposing)
    {
        VkFunc.DestroyRenderPass(VulkanLogicalDevice, RenderPass, VulkanInstance.AllocationCallbacks);

        base.Dispose(disposing);
    }

    public static implicit operator RenderPass(VulkanRenderPass v) => v.RenderPass;
}
