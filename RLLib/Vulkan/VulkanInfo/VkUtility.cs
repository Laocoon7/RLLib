using Silk.NET.Core.Native;
using Silk.NET.Vulkan;

namespace RLLib.VulkanInfo;

public static unsafe class VkUtility
{
    public static bool HasInstanceLayerProperty(params string[] properties)
    {
        var names = GetNames(VkFunc.EnumerateInstanceLayerProperties());
        foreach (var property in properties)
        {
            if (!names.Contains(property))
            {
                return false;
            }
        }
        return true;
    }

    public static List<string> GetNames(IEnumerable<LayerProperties> properties)
    {
        var propertyNames = new List<string>();
        foreach (var property in properties)
        {
            var propertyName = SilkMarshal.PtrToString((nint)property.LayerName);
            if (propertyName != null)
            {
                propertyNames.Add(propertyName);
            }
        }
        return propertyNames;
    }
    public static List<string> GetNames(IEnumerable<ExtensionProperties> properties)
    {
        var propertyNames = new List<string>();
        foreach (var property in properties)
        {
            var propertyName = SilkMarshal.PtrToString((nint)property.ExtensionName);
            if (propertyName != null)
            {
                propertyNames.Add(propertyName);
            }
        }
        return propertyNames;
    }

    public static Dictionary<string, LayerProperties> MatchNames(IEnumerable<LayerProperties> properties)
    {
        var propertyNames = new Dictionary<string, LayerProperties>();
        foreach (var property in properties)
        {
            var propertyName = SilkMarshal.PtrToString((nint)property.LayerName);
            if (propertyName != null)
            {
                propertyNames.Add(propertyName, property);
            }
        }
        return propertyNames;
    }
    public static Dictionary<string, ExtensionProperties> MatchNames(IEnumerable<ExtensionProperties> properties)
    {
        var propertyNames = new Dictionary<string, ExtensionProperties>();
        foreach (var property in properties)
        {
            var propertyName = SilkMarshal.PtrToString((nint)property.ExtensionName);
            if (propertyName != null)
            {
                propertyNames.Add(propertyName, property);
            }
        }
        return propertyNames;
    }

    public static List<ExtensionProperties> EnumerateAllInstanceExtensionProperties()
    {
        var properties = new List<ExtensionProperties>();

        properties.AddRange(VkFunc.EnumerateInstanceExtensionProperties(null));
        foreach (var layer in GetNames(VkFunc.EnumerateInstanceLayerProperties()))
        {
            properties.AddRange(VkFunc.EnumerateInstanceExtensionProperties(layer));
        }

        return properties;
    }
    public static List<ExtensionProperties> EnumerateAllPhysicalDeviceExtensionProperties(PhysicalDevice physicalDevice)
    {
        var properties = new List<ExtensionProperties>();

        properties.AddRange(VkFunc.EnumeratePhysicalDeviceExtensionProperties(physicalDevice, null));
        foreach (var layer in GetNames(VkFunc.EnumeratePhysicalDeviceLayerProperties(physicalDevice)))
        {
            properties.AddRange(VkFunc.EnumeratePhysicalDeviceExtensionProperties(physicalDevice, layer));
        }

        return properties;
    }

    public static unsafe ShaderModule CreateShaderModule(Device device, byte[] code, AllocationCallbacks? pAllocator = null)
    {
        var createInfo = new ShaderModuleCreateInfo()
        {
            SType = StructureType.ShaderModuleCreateInfo,
            CodeSize = (nuint)code.Length,
        };

        ShaderModule shaderModule;

        fixed (byte* pCode = code)
        {
            createInfo.PCode = (uint*)pCode;

            if (VkFunc.CreateShaderModule(device, createInfo, pAllocator, out shaderModule) != Result.Success)
            {
                throw new Exception("Failed to create shader module!");
            }
        }

        return shaderModule;

    }
    public static unsafe void DestroyShaderModule(Device device, ShaderModule shaderModule, AllocationCallbacks? pAllocator = null)
{
        VkFunc.DestroyShaderModule(device, shaderModule, pAllocator);
    }

    public static uint FindMemoryType(PhysicalDevice physicalDevice, uint typeFilter, MemoryPropertyFlags properties)
    {
        PhysicalDeviceMemoryProperties memProperties;
        VkFunc.GetPhysicalDeviceMemoryProperties(physicalDevice, out memProperties);

        for (int i = 0; i < memProperties.MemoryTypeCount; i++)
        {
            if ((typeFilter & (1 << i)) != 0 && (memProperties.MemoryTypes[i].PropertyFlags & properties) == properties)
            {
                return (uint)i;
            }
        }

        throw new Exception("Failed to find suitable memory type!");
    }

    public static bool SupportsFeatures(PhysicalDeviceFeatures requiredFeatures, PhysicalDeviceFeatures availableFeatures)
    {
        if (requiredFeatures.RobustBufferAccess && !availableFeatures.RobustBufferAccess) return false;
        if (requiredFeatures.FullDrawIndexUint32 && !availableFeatures.FullDrawIndexUint32) return false;
        if (requiredFeatures.ImageCubeArray && !availableFeatures.ImageCubeArray) return false;
        if (requiredFeatures.IndependentBlend && !availableFeatures.IndependentBlend) return false;
        if (requiredFeatures.GeometryShader && !availableFeatures.GeometryShader) return false;
        if (requiredFeatures.TessellationShader && !availableFeatures.TessellationShader) return false;
        if (requiredFeatures.SampleRateShading && !availableFeatures.SampleRateShading) return false;
        if (requiredFeatures.DualSrcBlend && !availableFeatures.DualSrcBlend) return false;
        if (requiredFeatures.LogicOp && !availableFeatures.LogicOp) return false;
        if (requiredFeatures.MultiDrawIndirect && !availableFeatures.MultiDrawIndirect) return false;
        if (requiredFeatures.DrawIndirectFirstInstance && !availableFeatures.DrawIndirectFirstInstance) return false;
        if (requiredFeatures.DepthClamp && !availableFeatures.DepthClamp) return false;
        if (requiredFeatures.DepthBiasClamp && !availableFeatures.DepthBiasClamp) return false;
        if (requiredFeatures.FillModeNonSolid && !availableFeatures.FillModeNonSolid) return false;
        if (requiredFeatures.DepthBounds && !availableFeatures.DepthBounds) return false;
        if (requiredFeatures.WideLines && !availableFeatures.WideLines) return false;
        if (requiredFeatures.LargePoints && !availableFeatures.LargePoints) return false;
        if (requiredFeatures.AlphaToOne && !availableFeatures.AlphaToOne) return false;
        if (requiredFeatures.MultiViewport && !availableFeatures.MultiViewport) return false;
        if (requiredFeatures.SamplerAnisotropy && !availableFeatures.SamplerAnisotropy) return false;
        if (requiredFeatures.TextureCompressionEtc2 && !availableFeatures.TextureCompressionEtc2) return false;
        if (requiredFeatures.TextureCompressionAstcLdr && !availableFeatures.TextureCompressionAstcLdr) return false;
        if (requiredFeatures.TextureCompressionBC && !availableFeatures.TextureCompressionBC) return false;
        if (requiredFeatures.OcclusionQueryPrecise && !availableFeatures.OcclusionQueryPrecise) return false;
        if (requiredFeatures.PipelineStatisticsQuery && !availableFeatures.PipelineStatisticsQuery) return false;
        if (requiredFeatures.VertexPipelineStoresAndAtomics && !availableFeatures.VertexPipelineStoresAndAtomics) return false;
        if (requiredFeatures.FragmentStoresAndAtomics && !availableFeatures.FragmentStoresAndAtomics) return false;
        if (requiredFeatures.ShaderTessellationAndGeometryPointSize && !availableFeatures.ShaderTessellationAndGeometryPointSize) return false;
        if (requiredFeatures.ShaderImageGatherExtended && !availableFeatures.ShaderImageGatherExtended) return false;
        if (requiredFeatures.ShaderStorageImageExtendedFormats && !availableFeatures.ShaderStorageImageExtendedFormats) return false;
        if (requiredFeatures.ShaderStorageImageMultisample && !availableFeatures.ShaderStorageImageMultisample) return false;
        if (requiredFeatures.ShaderStorageImageReadWithoutFormat && !availableFeatures.ShaderStorageImageReadWithoutFormat) return false;
        if (requiredFeatures.ShaderStorageImageWriteWithoutFormat && !availableFeatures.ShaderStorageImageWriteWithoutFormat) return false;
        if (requiredFeatures.ShaderUniformBufferArrayDynamicIndexing && !availableFeatures.ShaderUniformBufferArrayDynamicIndexing) return false;
        if (requiredFeatures.ShaderSampledImageArrayDynamicIndexing && !availableFeatures.ShaderSampledImageArrayDynamicIndexing) return false;
        if (requiredFeatures.ShaderStorageBufferArrayDynamicIndexing && !availableFeatures.ShaderStorageBufferArrayDynamicIndexing) return false;
        if (requiredFeatures.ShaderStorageImageArrayDynamicIndexing && !availableFeatures.ShaderStorageImageArrayDynamicIndexing) return false;
        if (requiredFeatures.ShaderClipDistance && !availableFeatures.ShaderClipDistance) return false;
        if (requiredFeatures.ShaderCullDistance && !availableFeatures.ShaderCullDistance) return false;
        if (requiredFeatures.ShaderFloat64 && !availableFeatures.ShaderFloat64) return false;
        if (requiredFeatures.ShaderInt64 && !availableFeatures.ShaderInt64) return false;
        if (requiredFeatures.ShaderInt16 && !availableFeatures.ShaderInt16) return false;
        if (requiredFeatures.ShaderResourceResidency && !availableFeatures.ShaderResourceResidency) return false;
        if (requiredFeatures.ShaderResourceMinLod && !availableFeatures.ShaderResourceMinLod) return false;
        if (requiredFeatures.SparseBinding && !availableFeatures.SparseBinding) return false;
        if (requiredFeatures.SparseResidencyBuffer && !availableFeatures.SparseResidencyBuffer) return false;
        if (requiredFeatures.SparseResidencyImage2D && !availableFeatures.SparseResidencyImage2D) return false;
        if (requiredFeatures.SparseResidencyImage3D && !availableFeatures.SparseResidencyImage3D) return false;
        if (requiredFeatures.SparseResidency2Samples && !availableFeatures.SparseResidency2Samples) return false;
        if (requiredFeatures.SparseResidency4Samples && !availableFeatures.SparseResidency4Samples) return false;
        if (requiredFeatures.SparseResidency8Samples && !availableFeatures.SparseResidency8Samples) return false;
        if (requiredFeatures.SparseResidency16Samples && !availableFeatures.SparseResidency16Samples) return false;
        if (requiredFeatures.SparseResidencyAliased && !availableFeatures.SparseResidencyAliased) return false;
        if (requiredFeatures.VariableMultisampleRate && !availableFeatures.VariableMultisampleRate) return false;
        if (requiredFeatures.InheritedQueries && !availableFeatures.InheritedQueries) return false;
        return true;
    }
}