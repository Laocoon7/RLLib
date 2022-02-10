using Silk.NET.Core.Native;
using Silk.NET.Vulkan;

using RLLib.VulkanInfo;

namespace RLLib.VulkanBuilders;

public class GraphicsPipelineBuilder
{
    private BuilderSettings Settings;

    public GraphicsPipelineBuilder(VulkanRenderPass vulkanRenderPass, VulkanDescriptorSetLayout vulkanDescriptorSetLayout)
    {
        Settings = new BuilderSettings(vulkanRenderPass, vulkanDescriptorSetLayout);
    }

    public VulkanGraphicsPipeline Build() => new VulkanGraphicsPipeline(Settings);

    #region Builder
    public GraphicsPipelineBuilder AddShader(VulkanShader vulkanShader)
    {
        Settings.VulkanShaders.Add(vulkanShader);
        return this;
    }

    public GraphicsPipelineBuilder AddVertexInput(Func<VertexInputBindingDescription> getBinding, Func<VertexInputAttributeDescription[]> getAttribute)
    {
        Settings.VertexInputs.Add(new VertexInput(getBinding, getAttribute));
        return this;
    }

    // PipelineInputAssemblyStateCreateInfo
    public GraphicsPipelineBuilder SetPrimitiveTopology(PrimitiveTopology primitiveTopology)
    {
        Settings.PrimitiveTopology = primitiveTopology;
        return this;
    }
    public GraphicsPipelineBuilder SetPrimitiveRestartEnable(bool enable = true)
    {
        Settings.PrimitiveRestartEnable = enable;
        return this;
    }

    // PipelineRasterizationStateCreateInfo
    public GraphicsPipelineBuilder SetDepthClampEnable(bool enable = true)
    {
        Settings.DepthClampEnable = enable;
        return this;
    }
    public GraphicsPipelineBuilder SetRasterizerDiscardEnable(bool enable = true)
    {
        Settings.RasterizerDiscardEnable = enable;
        return this;
    }
    public GraphicsPipelineBuilder SetPolygonMode(PolygonMode polygonMode)
    {
        Settings.PolygonMode = polygonMode;
        return this;
    }
    public GraphicsPipelineBuilder SetLineWidth(uint width)
    {
        Settings.LineWidth = width;
        return this;
    }
    public GraphicsPipelineBuilder SetCullMode(CullModeFlags cullMode)
    {
        Settings.CullMode = cullMode;
        return this;
    }
    public GraphicsPipelineBuilder SetFrontFace(FrontFace frontFace)
    {
        Settings.FrontFace = frontFace;
        return this;
    }
    public GraphicsPipelineBuilder SetDepthBiasEnable(bool enable = true)
    {
        Settings.DepthBiasEnable = enable;
        return this;
    }

    // PipelineMultisampleStateCreateInfo
    public GraphicsPipelineBuilder SetSampleShadingEnable(bool enable = true)
    {
        Settings.SampleShadingEnable = enable;
        return this;
    }
    public GraphicsPipelineBuilder SetRasterizationSamples(SampleCountFlags sampleCount)
    {
        Settings.RasterizationSamples = sampleCount;
        return this;
    }

    // PipelineColorBlendAttachmentState
    public GraphicsPipelineBuilder SetColorWriteMask(ColorComponentFlags colorComponent)
    {
        Settings.ColorWriteMask = colorComponent;
        return this;
    }
    public GraphicsPipelineBuilder AddColorWriteMask(ColorComponentFlags colorComponent)
    {
        Settings.ColorWriteMask |= colorComponent;
        return this;
    }
    public GraphicsPipelineBuilder SetBlendEnable(bool enable = true)
    {
        Settings.BlendEnable = enable;
        return this;
    }
    public GraphicsPipelineBuilder SetSrcColorBlendFactor(BlendFactor blendFactor)
    {
        Settings.SrcColorBlendFactor = blendFactor;
        return this;
    }
    public GraphicsPipelineBuilder SetDstColorBlendFactor(BlendFactor blendFactor)
    {
        Settings.DstColorBlendFactor = blendFactor;
        return this;
    }
    public GraphicsPipelineBuilder SetColorBlendOp(BlendOp blendOp)
    {
        Settings.ColorBlendOp = blendOp;
        return this;
    }
    public GraphicsPipelineBuilder SetSrcAlphaBlendFactor(BlendFactor blendFactor)
    {
        Settings.SrcAlphaBlendFactor = blendFactor;
        return this;
    }
    public GraphicsPipelineBuilder SetDstAlphaBlendFactor(BlendFactor blendFactor)
    {
        Settings.DstAlphaBlendFactor = blendFactor;
        return this;
    }
    public GraphicsPipelineBuilder SetAlphaBlendOp(BlendOp blendOp)
    {
        Settings.AlphaBlendOp = blendOp;
        return this;
    }

    // PipelineColorBlendStateCreateInfo
    public GraphicsPipelineBuilder SetLogicOpEnable(bool enable = true)
    {
        Settings.LogicOpEnable = enable;
        return this;
    }
    public GraphicsPipelineBuilder SetLogicOp(LogicOp logicOp)
    {
        Settings.LogicOp = logicOp;
        return this;
    }

    // BlendConstants
    public GraphicsPipelineBuilder SetBlendConstant0(uint blendConstant)
    {
        Settings.BlendConstant0 = blendConstant;
        return this;
    }
    public GraphicsPipelineBuilder SetBlendConstant1(uint blendConstant)
    {
        Settings.BlendConstant1 = blendConstant;
        return this;
    }
    public GraphicsPipelineBuilder SetBlendConstant2(uint blendConstant)
    {
        Settings.BlendConstant2 = blendConstant;
        return this;
    }
    public GraphicsPipelineBuilder SetBlendConstant3(uint blendConstant)
    {
        Settings.BlendConstant3 = blendConstant;
        return this;
    }
    #endregion

    public GraphicsPipelineBuilder Reset(VulkanRenderPass vulkanRenderPass, VulkanDescriptorSetLayout vulkanDescriptorSetLayout)
    {
        Settings.Reset(vulkanRenderPass, vulkanDescriptorSetLayout);
        return this;
    }

    public GraphicsPipelineBuilder Reset()
    {
        Settings.Reset();
        return this;
    }

    internal class BuilderSettings
    {
        public VulkanRenderPass VulkanRenderPass { get; set; }
        public VulkanDescriptorSetLayout VulkanDescriptorSetLayout { get; set; }

        public List<VulkanShader> VulkanShaders { get; set; } = new List<VulkanShader>();

        public List<VertexInput> VertexInputs { get; set; } = new List<VertexInput>();

        // PipelineInputAssemblyStateCreateInfo
        public PrimitiveTopology PrimitiveTopology { get; set; } = PrimitiveTopology.TriangleList;
        public bool PrimitiveRestartEnable { get; set; } = false;

        // PipelineRasterizationStateCreateInfo
        public bool DepthClampEnable { get; set; } = false;
        public bool RasterizerDiscardEnable { get; set; } = false;
        public PolygonMode PolygonMode { get; set; } = PolygonMode.Fill;
        public uint LineWidth { get; set; } = 1;
        public CullModeFlags CullMode { get; set; } = CullModeFlags.CullModeBackBit;
        public FrontFace FrontFace { get; set; } = FrontFace.CounterClockwise;
        public bool DepthBiasEnable { get; set; } = false;

        // PipelineMultisampleStateCreateInfo
        public bool SampleShadingEnable { get; set; } = false;
        public SampleCountFlags RasterizationSamples { get; set; } = SampleCountFlags.SampleCount1Bit;

        // PipelineColorBlendAttachmentState
        public ColorComponentFlags ColorWriteMask { get; set; } = ColorComponentFlags.ColorComponentRBit | ColorComponentFlags.ColorComponentGBit | ColorComponentFlags.ColorComponentBBit | ColorComponentFlags.ColorComponentABit;
        public bool BlendEnable { get; set; } = true;
        public BlendFactor SrcColorBlendFactor { get; set; } = BlendFactor.SrcAlpha;
        public BlendFactor DstColorBlendFactor { get; set; } = BlendFactor.OneMinusSrcAlpha;
        public BlendOp ColorBlendOp { get; set; } = BlendOp.Add;
        public BlendFactor SrcAlphaBlendFactor { get; set; } = BlendFactor.SrcAlpha;
        public BlendFactor DstAlphaBlendFactor { get; set; } = BlendFactor.OneMinusSrcAlpha;
        public BlendOp AlphaBlendOp { get; set; } = BlendOp.Add;

        // PipelineColorBlendStateCreateInfo
        public bool LogicOpEnable { get; set; } = false;
        public LogicOp LogicOp { get; set; } = LogicOp.Copy;

        // BlendConstants
        public uint BlendConstant0 { get; set; }
        public uint BlendConstant1 { get; set; }
        public uint BlendConstant2 { get; set; }
        public uint BlendConstant3 { get; set; }

        public BuilderSettings(VulkanRenderPass vulkanRenderPass, VulkanDescriptorSetLayout vulkanDescriptorSetLayout)
        {
            VulkanRenderPass = vulkanRenderPass;
            VulkanDescriptorSetLayout = vulkanDescriptorSetLayout;
        }

        public void Reset(VulkanRenderPass vulkanRenderPass, VulkanDescriptorSetLayout vulkanDescriptorSetLayout)
        {
            VulkanRenderPass = vulkanRenderPass;
            VulkanDescriptorSetLayout = vulkanDescriptorSetLayout;

            Reset();
        }

        public void Reset()
        {
            VulkanShaders.Clear();

            VertexInputs.Clear();

            PrimitiveTopology = PrimitiveTopology.TriangleList;
            PrimitiveRestartEnable = false;

            DepthClampEnable = false;
            RasterizerDiscardEnable = false;
            PolygonMode = PolygonMode.Fill;
            LineWidth = 1;
            CullMode = CullModeFlags.CullModeBackBit;
            FrontFace = FrontFace.CounterClockwise;
            DepthBiasEnable = false;

            SampleShadingEnable = false;
            RasterizationSamples = SampleCountFlags.SampleCount1Bit;

            ColorWriteMask = ColorComponentFlags.ColorComponentRBit | ColorComponentFlags.ColorComponentGBit | ColorComponentFlags.ColorComponentBBit | ColorComponentFlags.ColorComponentABit;
            BlendEnable = true;
            SrcColorBlendFactor = BlendFactor.SrcAlpha;
            DstColorBlendFactor = BlendFactor.OneMinusSrcAlpha;
            ColorBlendOp = BlendOp.Add;
            SrcAlphaBlendFactor = BlendFactor.SrcAlpha;
            DstAlphaBlendFactor = BlendFactor.OneMinusSrcAlpha;
            AlphaBlendOp = BlendOp.Add;

            LogicOpEnable = false;
            LogicOp = LogicOp.Copy;

            BlendConstant0 = 0;
            BlendConstant1 = 0;
            BlendConstant2 = 0;
            BlendConstant3 = 0;
        }
    }

    internal class VertexInput
    {
        public Func<VertexInputBindingDescription> GetBinding;
        public Func<VertexInputAttributeDescription[]> GetAttributes;

        public VertexInput(Func<VertexInputBindingDescription> getBinding, Func<VertexInputAttributeDescription[]> getAttribute)
        {
            GetBinding = getBinding;
            GetAttributes = getAttribute;
        }
    }
}

public class VulkanGraphicsPipeline : VkObject
{
    internal VulkanInstance VulkanInstance => VulkanRenderPass.VulkanInstance;
    internal VulkanLogicalDevice VulkanLogicalDevice => VulkanRenderPass.VulkanLogicalDevice;
    internal VulkanSwapchain VulkanSwapchain => VulkanRenderPass.VulkanSwapchain;
    internal VulkanRenderPass VulkanRenderPass { get; init; }
    internal VulkanDescriptorSetLayout VulkanDescriptorSetLayout { get; init; }

    internal List<VulkanShader> VulkanShaders { get; init; }

    public PipelineLayout PipelineLayout { get; init; }
    public Pipeline Pipeline { get; init; }

    internal unsafe VulkanGraphicsPipeline(GraphicsPipelineBuilder.BuilderSettings settings)
    {
        VulkanRenderPass = settings.VulkanRenderPass;
        VulkanDescriptorSetLayout = settings.VulkanDescriptorSetLayout;

        VulkanShaders = new List<VulkanShader>(settings.VulkanShaders);

        var freeables = new List<nint>();

        var piplineShaderStageCreateInfos = new List<PipelineShaderStageCreateInfo>();
        foreach (var shader in VulkanShaders)
        {
            var name = (byte*)SilkMarshal.StringToPtr(shader.EntryPoint);
            freeables.Add((nint)name);
            piplineShaderStageCreateInfos.Add(new PipelineShaderStageCreateInfo()
            {
                SType = StructureType.PipelineShaderStageCreateInfo,
                Stage = shader.ShaderStageFlags,
                Module = shader,
                PName = name,
            });
        }

        var shaderStages = piplineShaderStageCreateInfos.ToArray();
        fixed(PipelineShaderStageCreateInfo* pShaderStages = shaderStages)
        {
            var bindingDescriptions = new List<VertexInputBindingDescription>();
            var attributeDescriptions = new List<VertexInputAttributeDescription>();
            foreach (var vertexInput in settings.VertexInputs)
            {
                bindingDescriptions.Add(vertexInput.GetBinding());
                attributeDescriptions.AddRange(vertexInput.GetAttributes());
            }

            var bDescriptions = bindingDescriptions.ToArray();
            var aDescriptions = attributeDescriptions.ToArray();
            fixed (VertexInputBindingDescription* pBindingDescriptions = bDescriptions)
            fixed (VertexInputAttributeDescription* pAttributeDescriptions = aDescriptions)
            {

                var pDescriptorSetLayout = VulkanDescriptorSetLayout.DescriptorSetLayout;

                var vertexInputInfo = new PipelineVertexInputStateCreateInfo()
                {
                    SType = StructureType.PipelineVertexInputStateCreateInfo,

                    VertexBindingDescriptionCount = (uint)bDescriptions.Length,
                    PVertexBindingDescriptions = pBindingDescriptions,

                    VertexAttributeDescriptionCount = (uint)aDescriptions.Length,
                    PVertexAttributeDescriptions = pAttributeDescriptions,
                };

                var inputAssembly = new PipelineInputAssemblyStateCreateInfo()
                {
                    SType = StructureType.PipelineInputAssemblyStateCreateInfo,
                    Topology = settings.PrimitiveTopology,
                    PrimitiveRestartEnable = settings.PrimitiveRestartEnable,
                };

                var viewport = new Viewport()
                {
                    X = 0,
                    Y = 0,
                    Width = VulkanSwapchain.Extent.Width,
                    Height = VulkanSwapchain.Extent.Height,
                    MinDepth = 0,
                    MaxDepth = 1,
                };

                var scissor = new Rect2D()
                {
                    Offset = { X = 0, Y = 0 },
                    Extent = VulkanSwapchain.Extent,
                };

                var viewportState = new PipelineViewportStateCreateInfo()
                {
                    SType = StructureType.PipelineViewportStateCreateInfo,
                    ViewportCount = 1,
                    PViewports = &viewport,
                    ScissorCount = 1,
                    PScissors = &scissor,
                };

                var rasterizer = new PipelineRasterizationStateCreateInfo()
                {
                    SType = StructureType.PipelineRasterizationStateCreateInfo,
                    DepthClampEnable = settings.DepthClampEnable,
                    RasterizerDiscardEnable = settings.RasterizerDiscardEnable,
                    PolygonMode = settings.PolygonMode,
                    LineWidth = settings.LineWidth,
                    CullMode = settings.CullMode,
                    FrontFace = settings.FrontFace,
                    DepthBiasEnable = settings.DepthBiasEnable,
                };

                var multisampling = new PipelineMultisampleStateCreateInfo()
                {
                    SType = StructureType.PipelineMultisampleStateCreateInfo,
                    SampleShadingEnable = settings.SampleShadingEnable,
                    RasterizationSamples = settings.RasterizationSamples,
                };

                var colorBlendAttachment = new PipelineColorBlendAttachmentState()
                {
                    ColorWriteMask = settings.ColorWriteMask,
                    BlendEnable = settings.BlendEnable,
                    SrcColorBlendFactor = settings.SrcColorBlendFactor,
                    DstColorBlendFactor = settings.DstColorBlendFactor,
                    ColorBlendOp = settings.ColorBlendOp,
                    SrcAlphaBlendFactor = settings.SrcAlphaBlendFactor,
                    DstAlphaBlendFactor = settings.DstAlphaBlendFactor,
                    AlphaBlendOp = settings.AlphaBlendOp,
                };

                var colorBlending = new PipelineColorBlendStateCreateInfo()
                {
                    SType = StructureType.PipelineColorBlendStateCreateInfo,
                    LogicOpEnable = settings.LogicOpEnable,
                    LogicOp = settings.LogicOp,
                    AttachmentCount = 1,
                    PAttachments = &colorBlendAttachment,
                };

                colorBlending.BlendConstants[0] = settings.BlendConstant0;
                colorBlending.BlendConstants[1] = settings.BlendConstant1;
                colorBlending.BlendConstants[2] = settings.BlendConstant2;
                colorBlending.BlendConstants[3] = settings.BlendConstant3;

                var pipelineLayoutInfo = new PipelineLayoutCreateInfo()
                {
                    SType = StructureType.PipelineLayoutCreateInfo,
                    PushConstantRangeCount = 0,
                    SetLayoutCount = 1,
                    PSetLayouts = &pDescriptorSetLayout
                };

                if (VkFunc.CreatePipelineLayout(VulkanLogicalDevice, pipelineLayoutInfo, VulkanInstance.AllocationCallbacks, out var pipelineLayout) != Result.Success)
                {
                    throw new Exception("Failed to create pipeline layout!");
                }

                PipelineLayout = pipelineLayout;

                var pipelineInfo = new GraphicsPipelineCreateInfo()
                {
                    SType = StructureType.GraphicsPipelineCreateInfo,
                    StageCount = 2,
                    PStages = pShaderStages,
                    PVertexInputState = &vertexInputInfo,
                    PInputAssemblyState = &inputAssembly,
                    PViewportState = &viewportState,
                    PRasterizationState = &rasterizer,
                    PMultisampleState = &multisampling,
                    PColorBlendState = &colorBlending,
                    Layout = pipelineLayout,
                    RenderPass = VulkanRenderPass,
                    Subpass = 0,
                    BasePipelineHandle = default
                };

                if (VkFunc.CreateGraphicsPipelines(VulkanLogicalDevice, pipelineInfo, VulkanInstance.AllocationCallbacks, out var pipeline) != Result.Success)
                {
                    throw new Exception("Failed to create graphics pipeline!");
                }

                Pipeline = pipeline;
            }
        }


        foreach (var freeable in freeables)
        {
            SilkMarshal.Free(freeable);
        }
    }

    protected override void Dispose(bool disposing)
    {
        VkFunc.DestroyPipelineLayout(VulkanLogicalDevice, PipelineLayout, VulkanInstance.AllocationCallbacks);
        VkFunc.DestroyPipeline(VulkanLogicalDevice, Pipeline, VulkanInstance.AllocationCallbacks);

        base.Dispose(disposing);
    }

    public static implicit operator PipelineLayout(VulkanGraphicsPipeline v) => v.PipelineLayout;
    public static implicit operator Pipeline(VulkanGraphicsPipeline v) => v.Pipeline;
}