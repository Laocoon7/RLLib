using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;

using Buffer = Silk.NET.Vulkan.Buffer;
using Semaphore = Silk.NET.Vulkan.Semaphore;

namespace RLLib.VulkanInfo;
/// <summary>
/// Vulkan Wrappers which do minimal boilerplate work.
/// </summary>
public static unsafe class VkFunc
{
    private static Vk VK = Vk.GetApi();

    #region Instance Properties
    public static Version32 EnumerateInstanceVersion()
    {
        uint version = 0;
        EnumerateInstanceVersion(ref version);
        return (Version32)version;
    }
    public static Result EnumerateInstanceVersion(ref uint version) =>
        VK.EnumerateInstanceVersion(ref version);

    public static LayerProperties[] EnumerateInstanceLayerProperties()
    {
        EnumerateInstanceLayerProperties(out var properties);
        return properties;
    }
    public static Result EnumerateInstanceLayerProperties(out LayerProperties[] layerProperties)
    {
        uint count = 0;
        VK.EnumerateInstanceLayerProperties(ref count, null);
        layerProperties = new LayerProperties[count];
        fixed (LayerProperties* pProps = layerProperties)
        {
            return VK.EnumerateInstanceLayerProperties(ref count, pProps);
        }
    }

    public static ExtensionProperties[] EnumerateInstanceExtensionProperties(string? layername)
    {
        EnumerateInstanceExtensionProperties(layername, out var properties);
        return properties;
    }
    public static Result EnumerateInstanceExtensionProperties(string? layerName, out ExtensionProperties[] extensionProperties)
    {
        uint count = 0;
        VK.EnumerateInstanceExtensionProperties(layerName, ref count, null);
        extensionProperties = new ExtensionProperties[count];
        fixed (ExtensionProperties* pProps = extensionProperties)
        {
            return VK.EnumerateInstanceExtensionProperties(layerName, ref count, pProps);
        }
    }
    #endregion

    #region PhysicalDevice Properties
    public static PhysicalDevice[] EnumeratePhysicalDevices(Instance instance)
    {
        EnumeratePhysicalDevices(instance, out var physicalDevices);
        return physicalDevices;
    }
    public static Result EnumeratePhysicalDevices(Instance instance, out PhysicalDevice[] physicalDevices)
    {
        uint count = 0;
        VK.EnumeratePhysicalDevices(instance, ref count, null);
        physicalDevices = new PhysicalDevice[count];
        fixed (PhysicalDevice* pDevices = physicalDevices)
        {
            return VK.EnumeratePhysicalDevices(instance, ref count, pDevices);
        }
    }

    public static PhysicalDeviceProperties GetPhysicalDeviceProperties(PhysicalDevice physicalDevice)
    {
        GetPhysicalDeviceProperties(physicalDevice, out var properties);
        return properties;
    }
    public static void GetPhysicalDeviceProperties(PhysicalDevice physicalDevice, out PhysicalDeviceProperties properties) =>
        VK.GetPhysicalDeviceProperties(physicalDevice, out properties);

    public static PhysicalDeviceProperties2 GetPhysicalDeviceProperties2(PhysicalDevice physicalDevice)
    {
        GetPhysicalDeviceProperties2(physicalDevice, out var properties);
        return properties;
    }
    public static void GetPhysicalDeviceProperties2(PhysicalDevice physicalDevice, out PhysicalDeviceProperties2 properties) =>
        VK.GetPhysicalDeviceProperties2(physicalDevice, out properties);

    public static PhysicalDeviceFeatures GetPhysicalDeviceFeatures(PhysicalDevice physicalDevice)
    {
        GetPhysicalDeviceFeatures(physicalDevice, out var features);
        return features;
    }
    public static void GetPhysicalDeviceFeatures(PhysicalDevice physicalDevice, out PhysicalDeviceFeatures properties) =>
        VK.GetPhysicalDeviceFeatures(physicalDevice, out properties);

    public static PhysicalDeviceFeatures2 GetPhysicalDeviceFeatures2(PhysicalDevice physicalDevice)
    {
        GetPhysicalDeviceFeatures2(physicalDevice, out var features);
        return features;
    }
    public static void GetPhysicalDeviceFeatures2(PhysicalDevice physicalDevice, out PhysicalDeviceFeatures2 properties) =>
        VK.GetPhysicalDeviceFeatures2(physicalDevice, out properties);

    public static PhysicalDeviceMemoryProperties GetPhysicalDeviceMemoryProperties(PhysicalDevice physicalDevice)
    {
        GetPhysicalDeviceMemoryProperties(physicalDevice, out var properties);
        return properties;
    }
    public static void GetPhysicalDeviceMemoryProperties(PhysicalDevice physicalDevice, out PhysicalDeviceMemoryProperties properties) =>
        VK.GetPhysicalDeviceMemoryProperties(physicalDevice, out properties);

    public static PhysicalDeviceMemoryProperties2 GetPhysicalDeviceMemoryProperties2(PhysicalDevice physicalDevice)
    {
        GetPhysicalDeviceMemoryProperties2(physicalDevice, out var properties);
        return properties;
    }
    public static void GetPhysicalDeviceMemoryProperties2(PhysicalDevice physicalDevice, out PhysicalDeviceMemoryProperties2 properties) =>
        VK.GetPhysicalDeviceMemoryProperties2(physicalDevice, out properties);

    public static QueueFamilyProperties[] GetPhysicalDeviceQueueFamilyProperties(PhysicalDevice physicalDevice)
    {
        GetPhysicalDeviceQueueFamilyProperties(physicalDevice, out var properties);
        return properties;
    }
    public static void GetPhysicalDeviceQueueFamilyProperties(PhysicalDevice physicalDevice, out QueueFamilyProperties[] properties)
    {
        uint count = 0;
        VK.GetPhysicalDeviceQueueFamilyProperties(physicalDevice, ref count, null);
        properties = new QueueFamilyProperties[count];
        fixed (QueueFamilyProperties* pProps = properties)
        {
            VK.GetPhysicalDeviceQueueFamilyProperties(physicalDevice, ref count, pProps);
        }
    }

    public static QueueFamilyProperties2[] GetPhysicalDeviceQueueFamilyProperties2(PhysicalDevice physicalDevice)
    {
        GetPhysicalDeviceQueueFamilyProperties2(physicalDevice, out var properties);
        return properties;
    }
    public static void GetPhysicalDeviceQueueFamilyProperties2(PhysicalDevice physicalDevice, out QueueFamilyProperties2[] properties)
    {
        uint count = 0;
        VK.GetPhysicalDeviceQueueFamilyProperties2(physicalDevice, ref count, null);
        properties = new QueueFamilyProperties2[count];
        fixed (QueueFamilyProperties2* pProps = properties)
        {
            VK.GetPhysicalDeviceQueueFamilyProperties2(physicalDevice, ref count, pProps);
        }
    }

    /// <summary>
    ///  Vk.EnumeratePhysicalDeviceLayerProperties doesn't exist... so alias:
    ///  Vk.EnumerateDeviceLayerProperties
    /// </summary>
    /// <param name="physicalDevice"></param>
    /// <returns></returns>
    public static LayerProperties[] EnumeratePhysicalDeviceLayerProperties(PhysicalDevice physicalDevice)
    {
        EnumeratePhysicalDeviceLayerProperties(physicalDevice, out var properties);
        return properties;
    }
    /// <summary>
    /// Vk.EnumeratePhysicalDeviceLayerProperties doesn't exist... so alias:
    ///  Vk.EnumerateDeviceLayerProperties
    /// </summary>
    /// <param name="physicalDevice"></param>
    /// <param name="properties"></param>
    /// <returns></returns>
    public static Result EnumeratePhysicalDeviceLayerProperties(PhysicalDevice physicalDevice, out LayerProperties[] properties) =>
        EnumerateDeviceLayerProperties(physicalDevice, out properties);
    public static LayerProperties[] EnumerateDeviceLayerProperties(PhysicalDevice physicalDevice)
    {
        EnumerateDeviceLayerProperties(physicalDevice, out var properties);
        return properties;
    }
    public static Result EnumerateDeviceLayerProperties(PhysicalDevice physicalDevice, out LayerProperties[] properties)
    {
        uint count = 0;
        VK.EnumerateDeviceLayerProperties(physicalDevice, ref count, null);
        properties = new LayerProperties[count];
        fixed (LayerProperties* pProps = properties)
        {
            return VK.EnumerateDeviceLayerProperties(physicalDevice, ref count, pProps);
        }
    }

    /// <summary>
    /// Vk.EnumeratePhysicalDeviceExtensionProperties doesn't exist... so alias:
    /// Vk.EnumerateDeviceExtensionProperties
    /// </summary>
    /// <param name="physicalDevice"></param>
    /// <param name="layerName"></param>
    /// <returns></returns>
    public static ExtensionProperties[] EnumeratePhysicalDeviceExtensionProperties(PhysicalDevice physicalDevice, string? layerName)
    {
        EnumeratePhysicalDeviceExtensionProperties(physicalDevice, layerName, out var properties);
        return properties;
    }
    /// <summary>
    /// Vk.EnumeratePhysicalDeviceExtensionProperties doesn't exist... so alias:
    /// Vk.EnumerateDeviceExtensionProperties
    /// </summary>
    /// <param name="physicalDevice"></param>
    /// <param name="layerName"></param>
    /// <param name="properties"></param>
    /// <returns></returns>
    public static Result EnumeratePhysicalDeviceExtensionProperties(PhysicalDevice physicalDevice, string? layerName, out ExtensionProperties[] properties) =>
        EnumerateDeviceExtensionProperties(physicalDevice, layerName, out properties);
    public static ExtensionProperties[] EnumerateDeviceExtensionProperties(PhysicalDevice physicalDevice, string? layerName)
    {
        EnumerateDeviceExtensionProperties(physicalDevice, layerName, out var properties);
        return properties;
    }
    public static Result EnumerateDeviceExtensionProperties(PhysicalDevice physicalDevice, string? layerName, out ExtensionProperties[] properties)
    {
        uint count = 0;
        VK.EnumerateDeviceExtensionProperties(physicalDevice, layerName, ref count, null);
        properties = new ExtensionProperties[count];
        fixed (ExtensionProperties* pProps = properties)
        {
            return VK.EnumerateDeviceExtensionProperties(physicalDevice, layerName, ref count, pProps);
        }
    }

    public static ExternalBufferProperties GetPhysicalDeviceExternalBufferProperties(PhysicalDevice physicalDevice, PhysicalDeviceExternalBufferInfo physicalDeviceExternalBufferInfo)
    {
        GetPhysicalDeviceExternalBufferProperties(physicalDevice, physicalDeviceExternalBufferInfo, out var properties);
        return properties;
    }
    public static void GetPhysicalDeviceExternalBufferProperties(PhysicalDevice physicalDevice, PhysicalDeviceExternalBufferInfo physicalDeviceExternalBufferInfo, out ExternalBufferProperties properties) =>
        VK.GetPhysicalDeviceExternalBufferProperties(physicalDevice, physicalDeviceExternalBufferInfo, out properties);

    public static FormatProperties GetPhysicalDeviceFormatProperties(PhysicalDevice physicalDevice, Format format)
    {
        GetPhysicalDeviceFormatProperties(physicalDevice, format, out var properties);
        return properties;
    }
    public static void GetPhysicalDeviceFormatProperties(PhysicalDevice physicalDevice, Format format, out FormatProperties properties) =>
        VK.GetPhysicalDeviceFormatProperties(physicalDevice, format, out properties);

    public static FormatProperties2 GetPhysicalDeviceFormatProperties2(PhysicalDevice physicalDevice, Format format)
    {
        GetPhysicalDeviceFormatProperties2(physicalDevice, format, out var properties);
        return properties;
    }
    public static void GetPhysicalDeviceFormatProperties2(PhysicalDevice physicalDevice, Format format, out FormatProperties2 properties) =>
        VK.GetPhysicalDeviceFormatProperties2(physicalDevice, format, out properties);

    public static ImageFormatProperties GetPhysicalDeviceImageFormatProperties(PhysicalDevice physicalDevice, Format format, ImageType type, ImageTiling tiling, ImageUsageFlags usage, ImageCreateFlags flags)
    {
        GetPhysicalDeviceImageFormatProperties(physicalDevice, format, type, tiling, usage, flags, out var properties);
        return properties;
    }
    public static Result GetPhysicalDeviceImageFormatProperties(PhysicalDevice physicalDevice, Format format, ImageType type, ImageTiling tiling, ImageUsageFlags usage, ImageCreateFlags flags, out ImageFormatProperties properties) =>
        VK.GetPhysicalDeviceImageFormatProperties(physicalDevice, format, type, tiling, usage, flags, out properties);

    public static ImageFormatProperties2 GetPhysicalDeviceImageFormatProperties(PhysicalDevice physicalDevice, PhysicalDeviceImageFormatInfo2 imageFormatInfo)
    {
        GetPhysicalDeviceImageFormatProperties(physicalDevice, imageFormatInfo, out var properties);
        return properties;
    }
    public static Result GetPhysicalDeviceImageFormatProperties(PhysicalDevice physicalDevice, PhysicalDeviceImageFormatInfo2 imageFormatInfo, out ImageFormatProperties2 properties) =>
        VK.GetPhysicalDeviceImageFormatProperties2(physicalDevice, imageFormatInfo, out properties);

    public static SparseImageFormatProperties[] GetPhysicalDeviceSparseImageFormatProperties(PhysicalDevice physicalDevice, Format format, ImageType type, SampleCountFlags sampleCountFlags, ImageUsageFlags usage, ImageTiling tiling)
    {
        GetPhysicalDeviceSparseImageFormatProperties(physicalDevice, format, type, sampleCountFlags, usage, tiling, out var properties);
        return properties;
    }
    public static void GetPhysicalDeviceSparseImageFormatProperties(PhysicalDevice physicalDevice, Format format, ImageType type, SampleCountFlags sampleCountFlags, ImageUsageFlags usage, ImageTiling tiling, out SparseImageFormatProperties[] properties)
    {
        uint count = 0;
        VK.GetPhysicalDeviceSparseImageFormatProperties(physicalDevice, format, type, sampleCountFlags, usage, tiling, ref count, null);
        properties = new SparseImageFormatProperties[count];
        fixed (SparseImageFormatProperties* pProps = properties)
        {
            VK.GetPhysicalDeviceSparseImageFormatProperties(physicalDevice, format, type, sampleCountFlags, usage, tiling, ref count, pProps);
        }
    }

    public static SparseImageFormatProperties2[] GetPhysicalDeviceSparseImageFormatProperties2(PhysicalDevice physicalDevice, PhysicalDeviceSparseImageFormatInfo2 sparseImageFormatInfo)
    {
        GetPhysicalDeviceSparseImageFormatProperties2(physicalDevice, sparseImageFormatInfo, out var properties);
        return properties;
    }
    public static void GetPhysicalDeviceSparseImageFormatProperties2(PhysicalDevice physicalDevice, PhysicalDeviceSparseImageFormatInfo2 sparseImageFormatInfo, out SparseImageFormatProperties2[] properties)
    {
        uint count = 0;
        VK.GetPhysicalDeviceSparseImageFormatProperties2(physicalDevice, sparseImageFormatInfo, ref count, null);
        properties = new SparseImageFormatProperties2[count];
        fixed (SparseImageFormatProperties2* pProps = properties)
        {
            VK.GetPhysicalDeviceSparseImageFormatProperties2(physicalDevice, sparseImageFormatInfo, ref count, pProps);
        }
    }

    public static ExternalFenceProperties GetPhysicalDeviceExternalFenceProperties(PhysicalDevice physicalDevice, PhysicalDeviceExternalFenceInfo externalFenceInfo)
    {
        GetPhysicalDeviceExternalFenceProperties(physicalDevice, externalFenceInfo, out var properties);
        return properties;
    }
    public static void GetPhysicalDeviceExternalFenceProperties(PhysicalDevice physicalDevice, PhysicalDeviceExternalFenceInfo externalFenceInfo, out ExternalFenceProperties properties) =>
        VK.GetPhysicalDeviceExternalFenceProperties(physicalDevice, externalFenceInfo, out properties);

    public static ExternalSemaphoreProperties GetPhysicalDeviceExternalSemaphoreProperties(PhysicalDevice physicalDevice, PhysicalDeviceExternalSemaphoreInfo externalSemaphoreInfo)
    {
        GetPhysicalDeviceExternalSemaphoreProperties(physicalDevice, externalSemaphoreInfo, out var properties);
        return properties;
    }
    public static void GetPhysicalDeviceExternalSemaphoreProperties(PhysicalDevice physicalDevice, PhysicalDeviceExternalSemaphoreInfo externalSemaphoreInfo, out ExternalSemaphoreProperties properties) =>
        VK.GetPhysicalDeviceExternalSemaphoreProperties(physicalDevice, externalSemaphoreInfo, out properties);
    #endregion

    #region Extensions
    public static T? GetInstanceExtension<T>(Instance instance) where T : NativeExtension<Vk>
    {
        if (GetInstanceExtension<T>(instance, out var extension))
        {
            return extension;
        }
        return null;
    }
    public static bool GetInstanceExtension<T>(Instance instance, out T extension) where T : NativeExtension<Vk> =>
        VK.TryGetInstanceExtension<T>(instance, out extension);

    public static T? GetDeviceExtension<T>(Instance instance, Device device) where T : NativeExtension<Vk>
    {
        if (GetDeviceExtension<T>(instance, device, out var extension))
        {
            return extension;
        }
        return null;
    }
    public static bool GetDeviceExtension<T>(Instance instance, Device device, out T extension) where T : NativeExtension<Vk> =>
        VK.TryGetDeviceExtension<T>(instance, device, out extension);
    #endregion

    #region Object Creation
    public static Instance CreateInstance(in InstanceCreateInfo instanceCreateInfo, AllocationCallbacks? pAllocator)
    {
        CreateInstance(instanceCreateInfo, pAllocator, out var instance);
        return instance;
    }
    public static Result CreateInstance(in InstanceCreateInfo instanceCreateInfo, AllocationCallbacks? pAllocator, out Instance instance)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.CreateInstance(instanceCreateInfo, &allocationCallbacks, out instance);
}
        return VK.CreateInstance(instanceCreateInfo, null, out instance);
    }
    public static void DestroyInstance(Instance instance, AllocationCallbacks? pAllocator)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            VK.DestroyInstance(instance, &allocationCallbacks);
        }
else
{
            VK.DestroyInstance(instance, null);
        }
    }

    public static Queue GetDeviceQueue(Device logicalDevice, uint queueFamilyIndex, uint queueIndex)
    {
        GetDeviceQueue(logicalDevice, queueFamilyIndex, queueIndex, out var queue);
        return queue;
    }
    public static void GetDeviceQueue(Device logicalDevice, uint queueFamilyIndex, uint queueIndex, out Queue queue) =>
        VK.GetDeviceQueue(logicalDevice, queueFamilyIndex, queueIndex, out queue);

    public static Device CreateDevice(PhysicalDevice physicalDevice, in DeviceCreateInfo deviceCreateInfo, AllocationCallbacks? pAllocator)
    {
        CreateDevice(physicalDevice, deviceCreateInfo, pAllocator, out var device);
        return device;
    }
    public static Result CreateDevice(PhysicalDevice physicalDevice, in DeviceCreateInfo deviceCreateInfo, AllocationCallbacks? pAllocator, out Device logicalDevice)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.CreateDevice(physicalDevice, deviceCreateInfo, &allocationCallbacks, out logicalDevice);
        }
        return VK.CreateDevice(physicalDevice, deviceCreateInfo, null, out logicalDevice);
    }
    public static void DestroyDevice(Device logicalDevice, AllocationCallbacks? pAllocator)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            VK.DestroyDevice(logicalDevice, &allocationCallbacks);
        }
        else
        {
            VK.DestroyDevice(logicalDevice, null);
        }
    }

    public static void DeviceWaitIdle(Device logicalDevice) =>
        VK.DeviceWaitIdle(logicalDevice);

    public static ShaderModule CreateShaderModule(Device logicalDevice, ShaderModuleCreateInfo shaderModuleCreateInfo, AllocationCallbacks? pAllocator)
    {
        CreateShaderModule(logicalDevice, shaderModuleCreateInfo, pAllocator, out var shaderModule);
        return shaderModule;
    }
    public static Result CreateShaderModule(Device logicalDevice, ShaderModuleCreateInfo shaderModuleCreateInfo, AllocationCallbacks? pAllocator, out ShaderModule shaderModule)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.CreateShaderModule(logicalDevice, shaderModuleCreateInfo, &allocationCallbacks, out shaderModule);
        }
        return VK.CreateShaderModule(logicalDevice, shaderModuleCreateInfo, null, out shaderModule);
    }
    public static void DestroyShaderModule(Device logicalDevice, ShaderModule shaderModule, AllocationCallbacks? pAllocator)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            VK.DestroyShaderModule(logicalDevice, shaderModule, &allocationCallbacks);
        }
        else
        {
            VK.DestroyShaderModule(logicalDevice, shaderModule, null);
        }
    }

    public static Image CreateImage(Device logicalDevice, ImageCreateInfo imageCreateInfo, AllocationCallbacks? pAllocator)
    {
        CreateImage(logicalDevice, imageCreateInfo, pAllocator, out var image);
        return image;
    }
    public static Result CreateImage(Device logicalDevice, ImageCreateInfo imageCreateInfo, AllocationCallbacks? pAllocator, out Image image)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.CreateImage(logicalDevice, imageCreateInfo, &allocationCallbacks, out image);
        }
        return VK.CreateImage(logicalDevice, imageCreateInfo, null, out image);
    }
    public static void DestroyImage(Device logicalDevice, Image image, AllocationCallbacks? pAllocator)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            VK.DestroyImage(logicalDevice, image, &allocationCallbacks);
        }
        else
        {
            VK.DestroyImage(logicalDevice, image, null);
        }
    }

    public static void GetImageMemoryRequirements(Device logicalDevice, Image image, out MemoryRequirements memory) =>
        VK.GetImageMemoryRequirements(logicalDevice, image, out memory);

    public static Result BindImageMemory(Device logicalDevice, Image image, DeviceMemory memory, uint memoryOffset) =>
        VK.BindImageMemory(logicalDevice, image, memory, memoryOffset);

    public static ImageView CreateImageView(Device logicalDevice, ImageViewCreateInfo createInfo, AllocationCallbacks? pAllocator)
    {
        CreateImageView(logicalDevice, createInfo, pAllocator, out var imageView);
        return imageView;
    }
    public static Result CreateImageView(Device logicalDevice, ImageViewCreateInfo createInfo, AllocationCallbacks? pAllocator, out ImageView imageView)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.CreateImageView(logicalDevice, createInfo, &allocationCallbacks, out imageView);
        }
        return VK.CreateImageView(logicalDevice, createInfo, null, out imageView);
    }
    public static void DestroyImageView(Device logicalDevice, ImageView imageView, AllocationCallbacks? pAllocator)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            VK.DestroyImageView(logicalDevice, imageView, &allocationCallbacks);
        }
        else
        {
            VK.DestroyImageView(logicalDevice, imageView, null);
        }
    }

    public static Sampler CreateSampler(Device logicalDevice, SamplerCreateInfo samplerCreateInfo, AllocationCallbacks? pAllocator)
    {
        CreateSampler(logicalDevice, samplerCreateInfo, pAllocator, out var sampler);
        return sampler;
    }
    public static Result CreateSampler(Device logicalDevice, SamplerCreateInfo samplerCreateInfo, AllocationCallbacks? pAllocator, out Sampler sampler)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.CreateSampler(logicalDevice, samplerCreateInfo, &allocationCallbacks, out sampler);
        }
        return VK.CreateSampler(logicalDevice, samplerCreateInfo, null, out sampler);
    }
    public static void DestroySampler(Device logicalDevice, Sampler sampler, AllocationCallbacks? pAllocator)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            VK.DestroySampler(logicalDevice, sampler, &allocationCallbacks);
        }
        else
        {
            VK.DestroySampler(logicalDevice, sampler, null);
        }
    }

    public static RenderPass CreateRenderPass(Device logicalDevice, in RenderPassCreateInfo renderPassCreateInfo, AllocationCallbacks? pAllocator)
    {
        CreateRenderPass(logicalDevice, renderPassCreateInfo, pAllocator, out var renderPass);
        return renderPass;
    }
    public static Result CreateRenderPass(Device logicalDevice, in RenderPassCreateInfo renderPassCreateInfo, AllocationCallbacks? pAllocator, out RenderPass pRenderPass)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.CreateRenderPass(logicalDevice, renderPassCreateInfo, &allocationCallbacks, out pRenderPass);
        }
        return VK.CreateRenderPass(logicalDevice, renderPassCreateInfo, null, out pRenderPass);
    }
    public static void DestroyRenderPass(Device logicalDevice, RenderPass renderPass, AllocationCallbacks? pAllocator)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            VK.DestroyRenderPass(logicalDevice, renderPass, &allocationCallbacks);
        }
        else
        {
            VK.DestroyRenderPass(logicalDevice, renderPass, null);
        }
    }

    public static Framebuffer CreateFramebuffer(Device logicalDevice, FramebufferCreateInfo framebufferCreateInfo, AllocationCallbacks? pAllocator)
    {
        CreateFramebuffer(logicalDevice, framebufferCreateInfo, pAllocator, out var framebuffer);
        return framebuffer;
    }
    public static Result CreateFramebuffer(Device logicalDevice, FramebufferCreateInfo framebufferCreateInfo, AllocationCallbacks? pAllocator, out Framebuffer framebuffer)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.CreateFramebuffer(logicalDevice, framebufferCreateInfo, &allocationCallbacks, out framebuffer);
        }
        return VK.CreateFramebuffer(logicalDevice, framebufferCreateInfo, null, out framebuffer);
    }
    public static void DestroyFramebuffer(Device logicalDevice, Framebuffer framebuffer, AllocationCallbacks? pAllocator)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            VK.DestroyFramebuffer(logicalDevice, framebuffer, &allocationCallbacks);
        }
        else
        {
            VK.DestroyFramebuffer(logicalDevice, framebuffer, null);
        }
    }

    public static DescriptorSetLayout CreateDescriptorSetLayout(Device logicalDevice, DescriptorSetLayoutCreateInfo descriptorSetLayoutCreateInfo, AllocationCallbacks? pAllocator)
    {
        CreateDescriptorSetLayout(logicalDevice, descriptorSetLayoutCreateInfo, pAllocator, out var descriptorSetLayout);
        return descriptorSetLayout;
    }
    public static Result CreateDescriptorSetLayout(Device logicalDevice, DescriptorSetLayoutCreateInfo descriptorSetLayoutCreateInfo, AllocationCallbacks? pAllocator, out DescriptorSetLayout descriptorSetLayout)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.CreateDescriptorSetLayout(logicalDevice, descriptorSetLayoutCreateInfo, &allocationCallbacks, out descriptorSetLayout);
        }
        return VK.CreateDescriptorSetLayout(logicalDevice, descriptorSetLayoutCreateInfo, null, out descriptorSetLayout);
    }
    public static void DestroyDescriptorSetLayout(Device logicalDevice, DescriptorSetLayout descriptorSetLayout, AllocationCallbacks? pAllocator)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            VK.DestroyDescriptorSetLayout(logicalDevice, descriptorSetLayout, &allocationCallbacks);
        }
        else
        {
            VK.DestroyDescriptorSetLayout(logicalDevice, descriptorSetLayout, null);
        }
    }

    public static DescriptorPool CreateDescriptorPool(Device logicalDevice, DescriptorPoolCreateInfo descriptorPoolCreateInfo, AllocationCallbacks? pAllocator)
    {
        CreateDescriptorPool(logicalDevice, descriptorPoolCreateInfo, pAllocator, out var descriptorPool);
        return descriptorPool;
    }
    public static Result CreateDescriptorPool(Device logicalDevice, DescriptorPoolCreateInfo descriptorPoolCreateInfo, AllocationCallbacks? pAllocator, out DescriptorPool descriptorPool)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.CreateDescriptorPool(logicalDevice, descriptorPoolCreateInfo, &allocationCallbacks, out descriptorPool);
        }
        return VK.CreateDescriptorPool(logicalDevice, descriptorPoolCreateInfo, null, out descriptorPool);
    }
    public static void DestroyDescriptorPool(Device logicalDevice, DescriptorPool descriptorPool, AllocationCallbacks? pAllocator)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            VK.DestroyDescriptorPool(logicalDevice, descriptorPool, &allocationCallbacks);
        }
        else
        {
            VK.DestroyDescriptorPool(logicalDevice, descriptorPool, null);
        }
    }

    public static Result ResetDescriptorPool(Device logicalDevice, DescriptorPool descriptorPool, uint flags) =>
        VK.ResetDescriptorPool(logicalDevice, descriptorPool, flags);

    public static DescriptorSet[] AllocateDescriptorSets(Device logicalDevice, DescriptorSetAllocateInfo descriptorSetAllocateInfo)
    {
        AllocateDescriptorSets(logicalDevice, descriptorSetAllocateInfo, out var descriptorSets);
        return descriptorSets;
    }
    public static Result AllocateDescriptorSets(Device logicalDevice, DescriptorSetAllocateInfo descriptorSetAllocateInfo, out DescriptorSet[] descriptorSets)
    {
        fixed (DescriptorSet* pDescriptorSets = descriptorSets)
        {
            return VK.AllocateDescriptorSets(logicalDevice, descriptorSetAllocateInfo, pDescriptorSets);
        }
    }
    public static void FreeDescriptorSets(Device logicalDevice, DescriptorPool descriptorPool, DescriptorSet[] descriptorSets)
    {
        VK.FreeDescriptorSets(logicalDevice, descriptorPool, descriptorSets);
    }

    public static void UpdateDescriptorSets(Device logicalDevice, WriteDescriptorSet[]? writeDescriptorSets, CopyDescriptorSet[]? copyDescriptorSets) =>
        VK.UpdateDescriptorSets(logicalDevice, writeDescriptorSets ?? null, copyDescriptorSets ?? null);

    public static PipelineLayout CreatePipelineLayout(Device logicalDevice, PipelineLayoutCreateInfo pipelineLayoutCreateInfo, AllocationCallbacks? pAllocator)
    {
        CreatePipelineLayout(logicalDevice, pipelineLayoutCreateInfo, pAllocator, out var pipelineLayout);
        return pipelineLayout;
    }
    public static Result CreatePipelineLayout(Device logicalDevice, PipelineLayoutCreateInfo pipelineLayoutCreateInfo, AllocationCallbacks? pAllocator, out PipelineLayout pipelineLayout)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.CreatePipelineLayout(logicalDevice, pipelineLayoutCreateInfo, &allocationCallbacks, out pipelineLayout);
        }
        return VK.CreatePipelineLayout(logicalDevice, pipelineLayoutCreateInfo, null, out pipelineLayout);
    }
    public static void DestroyPipelineLayout(Device logicalDevice, PipelineLayout pipelineLayout, AllocationCallbacks? pAllocator)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            VK.DestroyPipelineLayout(logicalDevice, pipelineLayout, &allocationCallbacks);
        }
        else
        {
            VK.DestroyPipelineLayout(logicalDevice, pipelineLayout, null);
        }
    }

    public static PipelineCache CreatePipelineCache(Device logicalDevice, PipelineCacheCreateInfo pipelineCacheCreateInfo, AllocationCallbacks? pAllocator)
    {
        CreatePipelineCache(logicalDevice, pipelineCacheCreateInfo, pAllocator, out var pipelineCache);
        return pipelineCache;
    }
    public static Result CreatePipelineCache(Device logicalDevice, PipelineCacheCreateInfo pipelineCacheCreateInfo, AllocationCallbacks? pAllocator, out PipelineCache pipelineCache)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.CreatePipelineCache(logicalDevice, pipelineCacheCreateInfo, &allocationCallbacks, out pipelineCache);
        }
        return VK.CreatePipelineCache(logicalDevice, pipelineCacheCreateInfo, null, out pipelineCache);
    }
    public static void DestroyPipelineCache(Device logicalDevice, PipelineCache pipelineCache, AllocationCallbacks? pAllocator)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            VK.DestroyPipelineCache(logicalDevice, pipelineCache, &allocationCallbacks);
        }
        else
        {
            VK.DestroyPipelineCache(logicalDevice, pipelineCache, null);
        }
    }

    public static Result CreateGraphicsPipelines(Device logicalDevice, GraphicsPipelineCreateInfo graphicsPipelineCreateInfo, AllocationCallbacks? pAllocator, out Pipeline pipeline) =>
        CreateGraphicsPipelines(logicalDevice, default, 1, graphicsPipelineCreateInfo, pAllocator, out pipeline);
    public static Result CreateGraphicsPipelines(Device logicalDevice, PipelineCache pipelineCache, uint count, GraphicsPipelineCreateInfo graphicsPipelineCreateInfo, AllocationCallbacks? pAllocator, out Pipeline pipeline)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.CreateGraphicsPipelines(logicalDevice, pipelineCache, count, graphicsPipelineCreateInfo, &allocationCallbacks, out pipeline);
        }
        return VK.CreateGraphicsPipelines(logicalDevice, pipelineCache, count, graphicsPipelineCreateInfo, null, out pipeline);
    }
    public static Result CreateComputePipelines(Device logicalDevice, ComputePipelineCreateInfo computePipelineCreateInfo, AllocationCallbacks? pAllocator, out Pipeline pipeline) =>
        CreateComputePipelines(logicalDevice, default, 1, computePipelineCreateInfo, pAllocator, out pipeline);
    public static Result CreateComputePipelines(Device logicalDevice, PipelineCache pipelineCache, uint count, ComputePipelineCreateInfo computePipelineCreateInfo, AllocationCallbacks? pAllocator, out Pipeline pipeline)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.CreateComputePipelines(logicalDevice, pipelineCache, count, computePipelineCreateInfo, &allocationCallbacks, out pipeline);
        }
        return VK.CreateComputePipelines(logicalDevice, pipelineCache, count, computePipelineCreateInfo, null, out pipeline);
    }
    public static void DestroyPipeline(Device logicalDevice, Pipeline pipeline, AllocationCallbacks? pAllocator)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            VK.DestroyPipeline(logicalDevice, pipeline, &allocationCallbacks);
        }
        else
        {
            VK.DestroyPipeline(logicalDevice, pipeline, null);
        }
    }

    public static Result CreateCommandPool(Device logicalDevice, CommandPoolCreateInfo commandPoolCreateInfo, AllocationCallbacks? pAllocator, out CommandPool commandPool)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.CreateCommandPool(logicalDevice, commandPoolCreateInfo, &allocationCallbacks, out commandPool);
        }
        return VK.CreateCommandPool(logicalDevice, commandPoolCreateInfo, null, out commandPool);
    }
    public static void DestroyCommandPool(Device logicalDevice, CommandPool commandPool, AllocationCallbacks? pAllocator)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            VK.DestroyCommandPool(logicalDevice, commandPool, &allocationCallbacks);
        }
        else
        {
            VK.DestroyCommandPool(logicalDevice, commandPool, null);
        }
    }

    public static CommandBuffer[] AllocateCommandBuffers(Device logicalDevice, CommandBufferAllocateInfo commandBufferAllocateInfo)
    {
        AllocateCommandBuffers(logicalDevice, commandBufferAllocateInfo, out CommandBuffer[] commandBuffer);
        return commandBuffer;
    }
    public static Result AllocateCommandBuffers(Device logicalDevice, CommandBufferAllocateInfo commandBufferAllocateInfo, out CommandBuffer[] commandBuffers)
    {
        commandBuffers = new CommandBuffer[commandBufferAllocateInfo.CommandBufferCount];
        fixed (CommandBuffer* pCommandBuffers = commandBuffers)
        {
            return VK.AllocateCommandBuffers(logicalDevice, commandBufferAllocateInfo, pCommandBuffers);
        }
    }
    public static void FreeCommandBuffers(Device logicalDevice, CommandPool commandPool, params CommandBuffer[] commandBuffers)
    {
        VK.FreeCommandBuffers(logicalDevice, commandPool, commandBuffers);
    }

    public static Result ResetCommandBuffer(CommandBuffer commandBuffer, CommandBufferResetFlags flags) =>
        VK.ResetCommandBuffer(commandBuffer, flags);

    public static Fence CreateFence(Device logicalDevice, FenceCreateInfo fenceCreateInfo, AllocationCallbacks? pAllocator)
    {
        CreateFence(logicalDevice, fenceCreateInfo, pAllocator, out var fence);
        return fence;
    }
    public static Result CreateFence(Device logicalDevice, FenceCreateInfo fenceCreateInfo, AllocationCallbacks? pAllocator, out Fence fence)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.CreateFence(logicalDevice, fenceCreateInfo, &allocationCallbacks, out fence);
        }
        return VK.CreateFence(logicalDevice, fenceCreateInfo, null, out fence);
    }
    public static void DestroyFence(Device logicalDevice, Fence fence, AllocationCallbacks? pAllocator)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            VK.DestroyFence(logicalDevice, fence, &allocationCallbacks);
        }
        else
        {
            VK.DestroyFence(logicalDevice, fence, null);
        }
    }

    public static Result WaitForFences(Device logicalDevice, Fence[] fences, bool waitAll, ulong timeout) =>
        VK.WaitForFences(logicalDevice, fences, waitAll, timeout);
    public static Result ResetFences(Device logicalDevice, params Fence[] fences) =>
        VK.ResetFences(logicalDevice, fences);

    public static Semaphore CreateSemaphore(Device logicalDevice, SemaphoreCreateInfo semaphoreCreateInfo, AllocationCallbacks? pAllocator)
    {
        CreateSemaphore(logicalDevice, semaphoreCreateInfo, pAllocator, out var semaphore);
        return semaphore;
    }
    public static Result CreateSemaphore(Device logicalDevice, SemaphoreCreateInfo semaphoreCreateInfo, AllocationCallbacks? pAllocator, out Semaphore semaphore)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.CreateSemaphore(logicalDevice, semaphoreCreateInfo, &allocationCallbacks, out semaphore);
        }
        return VK.CreateSemaphore(logicalDevice, semaphoreCreateInfo, null, out semaphore);
    }
    public static void DestroySemaphore(Device logicalDevice, Semaphore semaphore, AllocationCallbacks? pAllocator)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            VK.DestroySemaphore(logicalDevice, semaphore, &allocationCallbacks);
        }
        else
        {
            VK.DestroySemaphore(logicalDevice, semaphore, null);
        }
    }

    public static Result WaitSemaphores(Device logicalDevice, SemaphoreWaitInfo semaphoreWaitInfo, ulong timeout) =>
        VK.WaitSemaphores(logicalDevice, semaphoreWaitInfo, timeout);

    public static Result BeginCommandBuffer(CommandBuffer commandBuffer, CommandBufferBeginInfo commandBufferBeginInfo) =>
        VK.BeginCommandBuffer(commandBuffer, commandBufferBeginInfo);
    public static Result EndCommandBuffer(CommandBuffer commandBuffer) =>
        VK.EndCommandBuffer(commandBuffer);

    public static Result QueueSubmit(Queue queue, Fence fence, params SubmitInfo[] submitInfo) =>
        VK.QueueSubmit(queue, submitInfo, fence);
    public static Result QueueWaitIdle(Queue queue) =>
        VK.QueueWaitIdle(queue);

    public static Buffer CreateBuffer(Device logicalDevice, BufferCreateInfo bufferCreateInfo, AllocationCallbacks? pAllocator)
    {
        CreateBuffer(logicalDevice, bufferCreateInfo, pAllocator, out var buffer);
        return buffer;
    }
    public static Result CreateBuffer(Device logicalDevice, BufferCreateInfo bufferCreateInfo, AllocationCallbacks? pAllocator, out Buffer buffer)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.CreateBuffer(logicalDevice, bufferCreateInfo, &allocationCallbacks, out buffer);
        }
        return VK.CreateBuffer(logicalDevice, bufferCreateInfo, null, out buffer);
    }
    public static void DestroyBuffer(Device logicalDevice, Buffer buffer, AllocationCallbacks? pAllocator)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            VK.DestroyBuffer(logicalDevice, buffer, &allocationCallbacks);
        }
        else
        {
            VK.DestroyBuffer(logicalDevice, buffer, null);
        }
    }

    public static void GetBufferMemoryRequirements(Device logicalDevice, Buffer buffer, out MemoryRequirements memoryRequirements) =>
        VK.GetBufferMemoryRequirements(logicalDevice, buffer, out memoryRequirements);

    public static DeviceMemory AllocateMemory(Device logicalDevice, MemoryAllocateInfo memoryAllocateInfo, AllocationCallbacks? pAllocator)
    {
        AllocateMemory(logicalDevice, memoryAllocateInfo, pAllocator, out var memory);
        return memory;
    }
    public static Result AllocateMemory(Device logicalDevice, MemoryAllocateInfo memoryAllocateInfo, AllocationCallbacks? pAllocator, out DeviceMemory deviceMemory)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            return VK.AllocateMemory(logicalDevice, memoryAllocateInfo, &allocationCallbacks, out deviceMemory);
        }
        return VK.AllocateMemory(logicalDevice, memoryAllocateInfo, null, out deviceMemory);
    }
    public static void FreeMemory(Device logicalDevice, DeviceMemory deviceMemory, AllocationCallbacks? pAllocator)
    {
        if (pAllocator != null)
        {
            var allocationCallbacks = pAllocator.Value;
            VK.FreeMemory(logicalDevice, deviceMemory, &allocationCallbacks);
        }
        else
        {
            VK.FreeMemory(logicalDevice, deviceMemory, null);
        }
    }

    public static void BindBufferMemory(Device logicalDevice, Buffer buffer, DeviceMemory memory, uint memoryOffset) =>
        VK.BindBufferMemory(logicalDevice, buffer, memory, memoryOffset);

    public static Result MapMemory(Device logicalDevice, DeviceMemory deviceMemory, ulong offset, ulong size, uint flags, void** data) =>
        VK.MapMemory(logicalDevice, deviceMemory, offset, size, flags, data);
    public static void UnmapMemory(Device logicalDevice, DeviceMemory deviceMemory) =>
        VK.UnmapMemory(logicalDevice, deviceMemory);
    #endregion


    #region Cmd
    public static void CmdCopyBuffer(CommandBuffer commandBuffer, Buffer source, Buffer destination, params BufferCopy[] regions) =>
        VK.CmdCopyBuffer(commandBuffer, source, destination, regions);

    public static void CmdCopyBufferToImage(CommandBuffer commandBuffer, Buffer source, Image destination, ImageLayout imageLayout, uint regionCount, BufferImageCopy region) =>
        VK.CmdCopyBufferToImage(commandBuffer, source, destination, imageLayout, regionCount, region);

    public static void CmdPipelineBarrier(CommandBuffer commandBuffer, PipelineStageFlags sourceStage, PipelineStageFlags destinationStage, DependencyFlags dependencyFlags, MemoryBarrier[]? memoryBarriers, BufferMemoryBarrier[]? bufferMemoryBarriers, ImageMemoryBarrier[]? imageMemoryBarriers) =>
        VK.CmdPipelineBarrier(commandBuffer, sourceStage, destinationStage, dependencyFlags, memoryBarriers == null ? 0 : (uint)memoryBarriers.Length, memoryBarriers, bufferMemoryBarriers == null ? 0 : (uint)bufferMemoryBarriers.Length, bufferMemoryBarriers, imageMemoryBarriers == null ? 0 : (uint)imageMemoryBarriers.Length, imageMemoryBarriers);

    public static void CmdBeginRenderPass(CommandBuffer commandBuffer, RenderPassBeginInfo renderPassBeginInfo, SubpassContents subpassContents)=>
        VK.CmdBeginRenderPass(commandBuffer, renderPassBeginInfo, subpassContents);

    public static void CmdEndRenderPass(CommandBuffer commandBuffer) =>
        VK.CmdEndRenderPass(commandBuffer);

    public static void CmdBindPipeline(CommandBuffer commandBuffer, PipelineBindPoint pipelineBindPoint, Pipeline pipeline) =>
        VK.CmdBindPipeline(commandBuffer, pipelineBindPoint, pipeline);

    public static void CmdBindVertexBuffers(CommandBuffer commandBuffer, uint binding, Buffer[] vertexBuffer, ulong[] offsets) =>
        VK.CmdBindVertexBuffers(commandBuffer, binding, (uint)vertexBuffer.Length, vertexBuffer, offsets);

    public static void CmdBindIndexBuffer(CommandBuffer commandBuffer, Buffer indexBuffer, uint offset, IndexType indexType) =>
        VK.CmdBindIndexBuffer(commandBuffer, indexBuffer, offset, indexType);

    public static void CmdBindDescriptorSets(CommandBuffer commandBuffer, PipelineBindPoint pipelineBindPoint, PipelineLayout pipelineLayout, uint firstSet, DescriptorSet[] descriptorSets, uint[]? dynamicOffsets) =>
        VK.CmdBindDescriptorSets(commandBuffer, pipelineBindPoint, pipelineLayout, firstSet, descriptorSets, dynamicOffsets);

    public static void CmdDrawIndexed(CommandBuffer commandBuffer, uint numberOfIndices, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance) =>
        VK.CmdDrawIndexed(commandBuffer, numberOfIndices, instanceCount, firstIndex, vertexOffset, firstInstance);

    public static void CmdBeginRendering(CommandBuffer commandBuffer, RenderingInfo renderingInfo)
    {
        VK.CmdBeginRendering(commandBuffer, renderingInfo);
    }
    #endregion
}