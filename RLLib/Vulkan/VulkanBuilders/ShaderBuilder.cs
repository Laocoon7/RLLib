using Silk.NET.Vulkan;

using RLLib.VulkanInfo;

namespace RLLib.VulkanBuilders;

public class ShaderBuilder
{
	private BuilderSettings Settings;

	public ShaderBuilder(VulkanLogicalDevice vulkanLogicalDevice)
    {
		Settings = new BuilderSettings(vulkanLogicalDevice);
    }

    public VulkanShader Build() => new VulkanShader(Settings);

    #region Builder
    public ShaderBuilder SetFile(string filename)
    {
        Settings.ShaderFile = new FileInfo(filename);
        return this;
    }

    public ShaderBuilder SetFile(FileInfo file)
    {
        Settings.ShaderFile = file;
        return this;
    }

    public ShaderBuilder SetEntryPoint (string entryPoint)
    {
        Settings.EntryPoint = entryPoint;
        return this;
    }

    public ShaderBuilder SetShaderStageFlags (ShaderStageFlags flags)
    {
        Settings.ShaderStageFlags = flags;
        return this;
    }

    #endregion

    public ShaderBuilder Reset(VulkanLogicalDevice vulkanLogicalDevice)
    {
        Settings.Reset(vulkanLogicalDevice);
        return this;
    }

    public ShaderBuilder Reset()
    {
        Settings.Reset();
        return this;
    }

    internal class BuilderSettings
    {
        public VulkanLogicalDevice VulkanLogicalDevice { get; set; }

        public FileInfo? ShaderFile { get; set; } = null;
        public string EntryPoint { get; set; } = "main";
        public ShaderStageFlags ShaderStageFlags { get; set; } = 0;


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
            ShaderFile = null;
            EntryPoint = "main";
            ShaderStageFlags = 0;
        }
    }
}

public class VulkanShader : VkObject
{
    internal VulkanLogicalDevice VulkanLogicalDevice { get; init; }

    public FileInfo ShaderFile { get; init; }
    public string EntryPoint { get; init; }
    public ShaderStageFlags ShaderStageFlags { get; init; }

    public ShaderModule ShaderModule { get; init; }

    internal unsafe VulkanShader(ShaderBuilder.BuilderSettings settings)
    {
        VulkanLogicalDevice = settings.VulkanLogicalDevice;
        if (settings.ShaderFile == null)
        {
            throw new Exception("Failed to create shader. No file provided.");
        }
        else if (!File.Exists(settings.ShaderFile.FullName))
        {
            throw new FileNotFoundException(settings.ShaderFile.FullName);
        }

        ShaderFile = settings.ShaderFile;
        EntryPoint = settings.EntryPoint;
        ShaderStageFlags = settings.ShaderStageFlags;

        var code = File.ReadAllBytes(ShaderFile.FullName);

        var shaderModuleCreateInfo = new ShaderModuleCreateInfo()
        {
            SType = StructureType.ShaderModuleCreateInfo,
            CodeSize = (nuint)code.Length,
        };

        fixed (byte* pCode = code)
        {
            shaderModuleCreateInfo.PCode = (uint*)pCode;

            if (VkFunc.CreateShaderModule(VulkanLogicalDevice, shaderModuleCreateInfo, VulkanLogicalDevice.VulkanInstance.AllocationCallbacks, out var shaderModule) != Result.Success)
            {
                throw new Exception("Failed to create shader module!");
            }
            ShaderModule = shaderModule;
        }
    }

    protected override void Dispose(bool disposing)
    {
        VkFunc.DestroyShaderModule(VulkanLogicalDevice, ShaderModule, VulkanLogicalDevice.VulkanInstance.AllocationCallbacks);

        base.Dispose(disposing);
    }

    public static implicit operator ShaderModule (VulkanShader v) => v.ShaderModule;
}