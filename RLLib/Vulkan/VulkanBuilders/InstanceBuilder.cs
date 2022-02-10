using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;

using RLLib.VulkanInfo;

namespace RLLib.VulkanBuilders;

public class InstanceBuilder
{
    private BuilderSettings Settings;

    public InstanceBuilder(VulkanWindow? vulkanWindow = null)
    {
        Settings = new BuilderSettings();
        Settings.VulkanWindow = vulkanWindow;
    }

    public VulkanInstance Build() => new VulkanInstance(Settings);

    #region Builder
    /// <summary>
    /// Set the app name.
    /// </summary>
    /// <param name="appName"></param>
    /// <returns></returns>
    public InstanceBuilder SetAppName(string appName)
    {
        Settings.AppName = appName;
        return this;
    }

    /// <summary>
    /// Set the engine name.
    /// </summary>
    /// <param name="engineName"></param>
    /// <returns></returns>
    public InstanceBuilder SetEngineName(string engineName)
    {
        Settings.EngineName = engineName;
        return this;
    }

    /// <summary>
    /// Set the app version.
    /// </summary>
    /// <param name="major"></param>
    /// <param name="minor"></param>
    /// <param name="revision"></param>
    /// <returns></returns>
    public InstanceBuilder SetAppVersion(uint major, uint minor, uint revision)
    {
        Settings.AppVersion = new Version32(major, minor, revision);
        return this;
    }

    /// <summary>
    /// Set the engine version.
    /// </summary>
    /// <param name="major"></param>
    /// <param name="minor"></param>
    /// <param name="revision"></param>
    /// <returns></returns>
    public InstanceBuilder SetEngineVersion(uint major, uint minor, uint revision)
    {
        Settings.EngineVersion = new Version32(major, minor, revision);
        return this;
    }

    public InstanceBuilder SetWindow(VulkanWindow? window)
    {
        Settings.VulkanWindow = window;
        return this;
    }

    /// <summary>
    /// VkInstanceCreateFlags are reserved for future use.
    /// </summary>
    /// <param name="ReservedFlags"></param>
    /// <returns></returns>
    public InstanceBuilder SetInstanceCreateFlags(uint ReservedFlags)
    {
        Settings.InstanceCreateFlags = ReservedFlags;
        return this;
    }

    /// <summary>
    /// Set required Api Version.
    /// </summary>
    /// <param name="apiVersion"></param>
    /// <returns></returns>
    public InstanceBuilder RequiredApiVersion(uint apiVersion)
    {
        Settings.RequiredApiVersion = (Version32)apiVersion;
        return this;
    }

    /// <summary>
    /// Set desired Api Version.
    /// </summary>
    /// <param name="apiVersion"></param>
    /// <returns></returns>
    public InstanceBuilder DesiredApiVersion(uint apiVersion)
    {
        Settings.DesiredApiVersion = (Version32)apiVersion;
        return this;
    }

    /// <summary>
    /// Add a required extension.
    /// </summary>
    /// <param name="extensionName"></param>
    /// <returns></returns>
    public InstanceBuilder AddRequiredExtension(string extensionName)
    {
        if (string.IsNullOrWhiteSpace(extensionName)) return this;
        Settings.RequiredExtensions.Add(extensionName);
        return this;
    }

    /// <summary>
    /// Add a desired extension.
    /// </summary>
    /// <param name="extensionName"></param>
    /// <returns></returns>
    public InstanceBuilder AddDesiredExtension(string extensionName)
    {
        if (string.IsNullOrWhiteSpace(extensionName)) return this;
        Settings.DesiredExtensions.Add(extensionName);
        return this;
    }

    /// <summary>
    /// Add a required layer.
    /// </summary>
    /// <param name="layerName"></param>
    /// <returns></returns>
    public InstanceBuilder AddRequiredLayer(string layerName)
    {
        if (string.IsNullOrWhiteSpace(layerName)) return this;
        Settings.RequiredLayers.Add(layerName);
        return this;
    }

    /// <summary>
    /// Add a desired layer.
    /// </summary>
    /// <param name="layerName"></param>
    /// <returns></returns>
    public InstanceBuilder AddDesiredLayer(string layerName)
    {
        if (string.IsNullOrWhiteSpace(layerName)) return this;
        Settings.DesiredLayers.Add(layerName);
        return this;
    }

    /// <summary>
    /// Require VK_LAYER_KHRONOS_validation.
    /// </summary>
    /// <param name="requireValidation"></param>
    /// <returns></returns>
    public InstanceBuilder RequireValidationLayers(bool requireValidation = true)
    {
        Settings.IsRequiredValidationLayers = requireValidation;
        return this;
    }

    /// <summary>
    /// Desire VK_LAYER_KHRONOS_validation.
    /// </summary>
    /// <param name="desireValidation"></param>
    /// <returns></returns>
    public InstanceBuilder DesireValidationLayers(bool desireValidation = true)
    {
        Settings.IsDesiredValidationLayers = desireValidation;
        return this;
    }

    /// <summary>
    /// Add a validation check.
    /// </summary>
    /// <param name="check"></param>
    /// <returns></returns>
    public InstanceBuilder AddValidationCheck(ValidationCheckEXT check)
    {
        Settings.DisabledValidationChecks.Add(check);
        return this;
    }

    /// <summary>
    /// Add a validation feature to enable.
    /// </summary>
    /// <param name="enable"></param>
    /// <returns></returns>
    public InstanceBuilder AddValidationFeatureEnable(ValidationFeatureEnableEXT enable)
    {
        Settings.EnabledValidationFeatures.Add(enable);
        return this;
    }

    /// <summary>
    /// Add a validation feature to disable.
    /// </summary>
    /// <param name="disable"></param>
    /// <returns></returns>
    public InstanceBuilder AddValidationFeatureDisable(ValidationFeatureDisableEXT disable)
    {
        Settings.DisabledValidationFeatures.Add(disable);
        return this;
    }

    /// <summary>
    /// Use the default DebugMessenger. Will write with Console.WriteLine.
    /// </summary>
    /// <returns></returns>
    public unsafe InstanceBuilder UseDefaultDebugMessenger() => SetDebugCallback(VulkanInstance.DefaultDebugCallback);

    /// <summary>
    /// Setup a custom DebugCallback.
    /// </summary>
    /// <param name="debugCallback"></param>
    /// <returns></returns>
    public InstanceBuilder SetDebugCallback(DebugUtilsMessengerCallbackFunctionEXT debugCallback)
    {
        Settings.DebugCallback = debugCallback;
        return this;
    }

    /// <summary>
    /// Set a pointer to UserData which will be sent to DebugCallback.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public unsafe InstanceBuilder SetDebugCallbackUserData(void* obj)
    {
        Settings.UserData = obj;
        return this;
    }

    /// <summary>
    /// Set interested DebugCallback severity level.
    /// </summary>
    /// <param name="severity"></param>
    /// <returns></returns>
    public InstanceBuilder SetDebugMessengerSeverity(DebugUtilsMessageSeverityFlagsEXT severity)
    {
        Settings.SeverityFlags = severity;
        return this;
    }

    /// <summary>
    /// Add interested DebugCallback severity level.
    /// </summary>
    /// <param name="severity"></param>
    /// <returns></returns>
    public InstanceBuilder AddDebugMessengerSeverity(DebugUtilsMessageSeverityFlagsEXT severity)
    {
        Settings.SeverityFlags |= severity;
        return this;
    }

    /// <summary>
    /// Set interested DebugCallback message type.
    /// </summary>
    /// <param name="types"></param>
    /// <returns></returns>
    public InstanceBuilder SetDebugMessengerType(DebugUtilsMessageTypeFlagsEXT types)
    {
        Settings.TypeFlags = types;
        return this;
    }

    /// <summary>
    /// Add interested DebugCallback message type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public InstanceBuilder AddDebugMessengerType(DebugUtilsMessageTypeFlagsEXT type)
    {
        Settings.TypeFlags |= type;
        return this;
    }

    /// <summary>
    /// VkDebugUtilsMessengerCreateFlagsEXT are reserved for future use.
    /// </summary>
    /// <param name="ReservedFlags"></param>
    /// <returns></returns>
    public InstanceBuilder SetDebugCreateFlags(uint ReservedFlags)
    {
        Settings.DebugCreateFlags = ReservedFlags;
        return this;
    }

    /// <summary>
    /// Set the AllocationCallbacks.
    /// </summary>
    /// <param name="allocationCallbacks"></param>
    /// <returns></returns>
    public InstanceBuilder SetAllocationCallbacks(AllocationCallbacks? allocationCallbacks)
    {
        Settings.AllocationCallbacks = allocationCallbacks;
        return this;
    }
    #endregion

    public InstanceBuilder Reset(VulkanWindow? vulkanWindow)
    {
        Settings.Reset(vulkanWindow);
        return this;
    }

    public InstanceBuilder Reset()
    {
        Settings.Reset();
        return this;
    }

    internal class BuilderSettings
    {
        // IWindow
        public VulkanWindow? VulkanWindow = null;

        // ApplicationInfo
        public string AppName { get; set; } = string.Empty;
        public string EngineName { get; set; } = "VulkanEngine";
        public Version32 AppVersion { get; set; } = new Version32(1, 0, 0);
        public Version32 EngineVersion { get; set; } = new Version32(1, 0, 0);
        public Version32 ApiVersion { get; set; } = Vk.Version10;

        // API Version
        public Version32 RequiredApiVersion { get; set; } = Vk.Version10;
        public Version32 DesiredApiVersion { get; set; } = Vk.Version12;

        // CreateInstanceInfo
        public List<string> RequiredLayers { get; set; } = new List<string>();
        public List<string> DesiredLayers { get; set; } = new List<string>();
        public List<string> RequiredExtensions { get; set; } = new List<string>();
        public List<string> DesiredExtensions { get; set; } = new List<string>();
        public uint InstanceCreateFlags { get; set; } = 0;

        // DebugInfo
        public DebugUtilsMessengerCallbackFunctionEXT? DebugCallback { get; set; } = null;
        public DebugUtilsMessageSeverityFlagsEXT SeverityFlags { get; set; } = DebugUtilsMessageSeverityFlagsEXT.DebugUtilsMessageSeverityWarningBitExt | DebugUtilsMessageSeverityFlagsEXT.DebugUtilsMessageSeverityErrorBitExt;
        public DebugUtilsMessageTypeFlagsEXT TypeFlags { get; set; } = DebugUtilsMessageTypeFlagsEXT.DebugUtilsMessageTypeValidationBitExt | DebugUtilsMessageTypeFlagsEXT.DebugUtilsMessageTypePerformanceBitExt | DebugUtilsMessageTypeFlagsEXT.DebugUtilsMessageTypeGeneralBitExt;
        public unsafe void* UserData { get; set; } = null;
        public uint DebugCreateFlags { get; set; } = 0;

        // ValidationLayersInfo
        public List<ValidationCheckEXT> DisabledValidationChecks { get; set; } = new List<ValidationCheckEXT>();
        public List<ValidationFeatureEnableEXT> EnabledValidationFeatures { get; set; } = new List<ValidationFeatureEnableEXT>();
        public List<ValidationFeatureDisableEXT> DisabledValidationFeatures { get; set; } = new List<ValidationFeatureDisableEXT>();

        // CustomAllocator
        public AllocationCallbacks? AllocationCallbacks { get; set; } = null;

        // Flags
        public bool IsRequiredValidationLayers { get; set; } = false;
        public bool IsDesiredValidationLayers { get; set; } = false;
        public bool DeferSurfaceCreation { get; set; } = false;
        public bool UseDebugMessenger => DebugCallback != null;

        public void Reset(VulkanWindow? vulkanWindow = null)
        {
            VulkanWindow = vulkanWindow;

            Reset();
        }

        public unsafe void Reset()
        {
            AppName = string.Empty;
            EngineName = string.Empty;
            AppVersion = new Version32(1, 0, 0);
            EngineVersion = new Version32(1, 0, 0);
            ApiVersion = Vk.Version10;

            RequiredApiVersion = Vk.Version10;
            DesiredApiVersion = Vk.Version12;

            RequiredLayers.Clear();
            DesiredLayers.Clear();
            RequiredExtensions.Clear();
            DesiredExtensions.Clear();
            InstanceCreateFlags = 0;

            DebugCallback = null;
            SeverityFlags = 0;
            TypeFlags = 0;
            UserData = null;
            DebugCreateFlags = 0;

            DisabledValidationChecks.Clear();
            EnabledValidationFeatures.Clear();
            DisabledValidationFeatures.Clear();

            AllocationCallbacks = null;

            IsRequiredValidationLayers = false;
            IsDesiredValidationLayers = false;
            DeferSurfaceCreation = false;
        }
    }
}

public class VulkanInstance : VkObject
{
    internal VulkanWindow? VulkanWindow { get; init; }

    public Instance Instance { get; init; }

    // ApplicationInfo
    public string AppName { get; init; }
    public string EngineName { get; init; }
    public Version32 AppVersion { get; init; }
    public Version32 EngineVersion { get; init; }

    // API Version
    public Version32 RequiredApiVersion { get; init; }
    public Version32 DesiredApiVersion { get; init; }
    public Version32 ApiVersion { get; init; }

    // Extensions
    private List<string> m_RequiredExtensions { get; init; }
    public List<string> RequiredExtensions => new List<string>(m_RequiredExtensions);
    private List<string> m_DesiredExtensions { get; init; }
    public List<string> DesiredExtensions => new List<string>(m_DesiredExtensions);

    // Layers
    private List<string> m_RequiredLayers { get; init; }
    public List<string> RequiredLayers => new List<string>(m_RequiredLayers);
    private List<string> m_DesiredLayers { get; init; }
    public List<string> DesiredLayers => new List<string>(m_DesiredLayers);

    // ValidationLayersInfo
    private List<ValidationCheckEXT> m_DisabledValidationChecks { get; init; }
    public List<ValidationCheckEXT> DisabledValidationChecks => new List<ValidationCheckEXT>(m_DisabledValidationChecks);
    private List<ValidationFeatureEnableEXT> m_EnabledValidationFeatures { get; init; }
    public List<ValidationFeatureEnableEXT> EnabledValidationFeatures => new List<ValidationFeatureEnableEXT>(m_EnabledValidationFeatures);
    private List<ValidationFeatureDisableEXT> m_DisabledValidationFeatures { get; init; }
    public List<ValidationFeatureDisableEXT> DisabledValidationFeatures => new List<ValidationFeatureDisableEXT>(m_DisabledValidationFeatures);

    // Debug
    public ExtDebugUtils? DebugUtils { get; init; }
    public DebugUtilsMessengerEXT? DebugMessenger { get; init; }
    public DebugUtilsMessengerCallbackFunctionEXT? DebugCallback { get; init; }
    public DebugUtilsMessageSeverityFlagsEXT SeverityFlags { get; init; }
    public DebugUtilsMessageTypeFlagsEXT TypeFlags { get; init; }
    public unsafe void* UserData { get; init; }
    public uint DebugCreateFlags { get; init; }

    // CustomAllocator
    public AllocationCallbacks? AllocationCallbacks { get; init; }

    // Flags
    public bool UseDebugMessenger { get; init; }

    internal unsafe VulkanInstance(InstanceBuilder.BuilderSettings settings)
    {
        AppName = settings.AppName;
        EngineName = settings.EngineName;
        AppVersion = settings.AppVersion;
        EngineVersion = settings.EngineVersion;

        RequiredApiVersion = settings.RequiredApiVersion;
        DesiredApiVersion = settings.DesiredApiVersion;
        ApiVersion = SelectApiVersion(RequiredApiVersion, DesiredApiVersion);

        m_RequiredExtensions = new List<string>(settings.RequiredExtensions);
        m_DesiredExtensions = new List<string>(settings.DesiredExtensions);

        m_RequiredLayers = new List<string>(settings.RequiredLayers);
        m_DesiredLayers = new List<string>(settings.DesiredLayers);

        m_DisabledValidationChecks = new List<ValidationCheckEXT>(settings.DisabledValidationChecks);
        m_EnabledValidationFeatures = new List<ValidationFeatureEnableEXT>(settings.EnabledValidationFeatures);
        m_DisabledValidationFeatures = new List<ValidationFeatureDisableEXT>(settings.DisabledValidationFeatures);

        AllocationCallbacks = settings.AllocationCallbacks;

        if (settings.VulkanWindow != null)
        {
            VulkanWindow = settings.VulkanWindow;
            var glfwExtensions = VulkanWindow.Window.VkSurface!.GetRequiredExtensions(out var glfwExtensionCount);
            var extensions = SilkMarshal.PtrToStringArray((nint)glfwExtensions, (int)glfwExtensionCount).ToList();
            m_RequiredExtensions.AddRange(extensions);
        }

        var allExtensionNames = VkUtility.GetNames(VkUtility.EnumerateAllInstanceExtensionProperties());
        var allLayerNames = VkUtility.GetNames(VkFunc.EnumerateInstanceLayerProperties());

        UseDebugMessenger = settings.UseDebugMessenger && allExtensionNames.Contains(ExtDebugUtils.ExtensionName);
        if (UseDebugMessenger)
        {
            DebugCallback = settings.DebugCallback;
            SeverityFlags = settings.SeverityFlags;
            TypeFlags = settings.TypeFlags;
            UserData = settings.UserData;
            DebugCreateFlags = settings.DebugCreateFlags;

            m_RequiredExtensions.Add(ExtDebugUtils.ExtensionName);
        }


        if (allExtensionNames.Contains(KhrGetPhysicalDeviceProperties2.ExtensionName))
        {
            m_RequiredExtensions.Add(KhrGetPhysicalDeviceProperties2.ExtensionName);
        }

        #region Validate Extensions
        foreach (var extensionName in RequiredExtensions)
        {
            if (!allExtensionNames.Contains(extensionName))
            {
                throw new Exception($"Not all required extensions are available.");
            }
        }

        var desiredExtensions = DesiredExtensions;
        foreach (var extension in m_DesiredExtensions)
        {
            if (!allExtensionNames.Contains(extension))
            {
                desiredExtensions.Remove(extension);
            }
        }

        var allExtensions = RequiredExtensions;
        allExtensions.AddRange(desiredExtensions);
        #endregion

        #region Validate Layers
        if (settings.IsRequiredValidationLayers)
        {
            m_RequiredLayers.Add("VK_LAYER_KHRONOS_validation");
        }
        else if (settings.IsDesiredValidationLayers)
        {
            m_DesiredLayers.Add("VK_LAYER_KHRONOS_validation");
        }

        foreach (var layerName in RequiredLayers)
        {
            if (!allLayerNames.Contains(layerName))
            {
                throw new Exception($"Not all required layers are available.");
            }
        }

        var desiredLayers = DesiredLayers;
        foreach (var layer in m_DesiredLayers)
        {
            if (!allLayerNames.Contains(layer))
            {
                desiredLayers.Remove(layer);
            }
        }

        var allLayers = RequiredLayers;
        allLayers.AddRange(desiredLayers);
        #endregion

        var appInfo = new ApplicationInfo()
        {
            SType = StructureType.ApplicationInfo,
            PApplicationName = (byte*)Marshal.StringToHGlobalAnsi(AppName),
            ApplicationVersion = AppVersion,
            PEngineName = (byte*)Marshal.StringToHGlobalAnsi(EngineName),
            EngineVersion = EngineVersion,
            ApiVersion = ApiVersion,
            PNext = null,
        };

        var createInfo = new InstanceCreateInfo()
        {
            SType = StructureType.InstanceCreateInfo,
            PApplicationInfo = &appInfo,
            EnabledExtensionCount = (uint)(allExtensions.Count),
            PpEnabledExtensionNames = (byte**)SilkMarshal.StringArrayToPtr(allExtensions.ToArray()),
            EnabledLayerCount = (uint)(allLayers.Count),
            PpEnabledLayerNames = allLayers.Count > 0 ? (byte**)SilkMarshal.StringArrayToPtr(allLayers.ToArray()) : null,
            Flags = settings.InstanceCreateFlags,
            PNext = null,
        };

        if (UseDebugMessenger)
        {
            createInfo.AddNext(out DebugUtilsMessengerCreateInfoEXT messengerCreateInfo);
            messengerCreateInfo = messengerCreateInfo with
            {
                SType = StructureType.DebugUtilsMessengerCreateInfoExt,
                MessageSeverity = SeverityFlags,
                MessageType = TypeFlags,
                PfnUserCallback = DebugCallback,
                PUserData = UserData,
                PNext = null,
            };
        }

        GlobalMemory? memEnabled = null;
        GlobalMemory? memDisabled = null;
        if (EnabledValidationFeatures.Count > 0 || DisabledValidationFeatures.Count > 0)
        {
            ValidationFeatureEnableEXT* enabledFeatures = null;
            if (EnabledValidationFeatures.Count > 0)
            {
                memEnabled = GlobalMemory.Allocate(EnabledValidationFeatures.Count * sizeof(ValidationFeatureEnableEXT));
                enabledFeatures = (ValidationFeatureEnableEXT*)Unsafe.AsPointer(ref memEnabled.GetPinnableReference());
                for (int i = 0; i < EnabledValidationFeatures.Count; i++)
                    enabledFeatures[i] = EnabledValidationFeatures[i];
            }

            ValidationFeatureDisableEXT* disabledFeatures = null;
            if (DisabledValidationFeatures.Count > 0)
            {
                memDisabled = GlobalMemory.Allocate(DisabledValidationFeatures.Count * sizeof(ValidationFeatureDisableEXT));
                disabledFeatures = (ValidationFeatureDisableEXT*)Unsafe.AsPointer(ref memDisabled.GetPinnableReference());
                for (int i = 0; i < DisabledValidationFeatures.Count; i++)
                    disabledFeatures[i] = DisabledValidationFeatures[i];
            }

            createInfo.AddNext(out ValidationFeaturesEXT features);
            features = features with
            {
                SType = StructureType.ValidationFeaturesExt,
                EnabledValidationFeatureCount = (uint)EnabledValidationFeatures.Count,
                PEnabledValidationFeatures = enabledFeatures,
                DisabledValidationFeatureCount = (uint)DisabledValidationFeatures.Count,
                PDisabledValidationFeatures = disabledFeatures,
                PNext = null,
            };
        }

        if (DisabledValidationChecks.Count > 0)
        {
            using var memChecks = GlobalMemory.Allocate(DisabledValidationChecks.Count * sizeof(ValidationCheckEXT));
            var disabledChecks = (ValidationCheckEXT*)Unsafe.AsPointer(ref memChecks.GetPinnableReference());
            for (int i = 0; i < DisabledValidationChecks.Count; i++)
                disabledChecks[i] = DisabledValidationChecks[i];

            createInfo.AddNext(out ValidationFlagsEXT checks);
            checks = checks with
            {
                SType = StructureType.ValidationFlagsExt,
                DisabledValidationCheckCount = (uint)DisabledValidationChecks.Count,
                PDisabledValidationChecks = disabledChecks,
                PNext = null,
            };
        }

        if (VkFunc.CreateInstance(createInfo, AllocationCallbacks, out var instance) != Result.Success)
        {
            throw new Exception($"Failed to create an instance.");
        }
        Instance = instance;

        if (UseDebugMessenger)
        {
            if (!VkFunc.GetInstanceExtension<ExtDebugUtils>(Instance, out var debugUtils))
            {
                throw new Exception($"Could not get extension: ExtDebugUtils");
            }
            DebugUtils = debugUtils;

            var debugInfo = new DebugUtilsMessengerCreateInfoEXT()
            {
                SType = StructureType.DebugUtilsMessengerCreateInfoExt,
                MessageSeverity = SeverityFlags,
                MessageType = TypeFlags,
                PfnUserCallback = DebugCallback,
                PUserData = UserData,
                Flags = DebugCreateFlags,
                PNext = null,
            };

            DebugUtilsMessengerEXT debugMessenger;
            if (AllocationCallbacks != null)
            {
                var allocationCallbacks = AllocationCallbacks.Value;
                if (DebugUtils.CreateDebugUtilsMessenger(Instance, in debugInfo, &allocationCallbacks, out debugMessenger) != Result.Success)
                {
                    throw new Exception($"Failed to create a debug messenger.");
                }
            }
            else
            {
                if (DebugUtils.CreateDebugUtilsMessenger(Instance, in debugInfo, null, out debugMessenger) != Result.Success)
                {
                    throw new Exception($"Failed to create a debug messenger.");
                }
            }
            DebugMessenger = debugMessenger;
        }

        memEnabled?.Dispose();
        memDisabled?.Dispose();

        Marshal.FreeHGlobal((IntPtr)appInfo.PApplicationName);
        Marshal.FreeHGlobal((IntPtr)appInfo.PEngineName);

        if (allLayers.Count > 0)
        {
            SilkMarshal.Free((nint)createInfo.PpEnabledLayerNames);
        }

        SilkMarshal.Free((nint)createInfo.PpEnabledExtensionNames);
    }

    private Version32 SelectApiVersion(Version32 required, Version32 desired)
    {
        var version = Vk.Version10;

        var apiVersion = VkFunc.EnumerateInstanceVersion();

        if (required > apiVersion)
        {
            throw new Exception($"Version not available. Required: {required.Major}.{required.Minor}.{required.Patch}, Available: {apiVersion.Major}.{apiVersion.Minor}.{apiVersion.Patch}");
        }

        if (desired > required)
        {
            if (desired > apiVersion)
            {
                version = apiVersion;
            }
            else
            {
                version = desired;
            }
        }
        else
        {
            version = required;
        }


        return version;
    }

    protected unsafe override void Dispose(bool disposing)
    {
        if (UseDebugMessenger)
        {
            if (AllocationCallbacks != null)
            {
                var allocationCallbacks = AllocationCallbacks.Value;
                DebugUtils!.DestroyDebugUtilsMessenger(Instance, DebugMessenger!.Value, &allocationCallbacks);
            }
            else
            {
                DebugUtils!.DestroyDebugUtilsMessenger(Instance, DebugMessenger!.Value, null);
            }
            DebugUtils.Dispose();
        }

        VkFunc.DestroyInstance(Instance, AllocationCallbacks);

        base.Dispose(disposing);
    }

    // DefaultDebugCallback
    internal unsafe static uint DefaultDebugCallback(DebugUtilsMessageSeverityFlagsEXT messageSeverity, DebugUtilsMessageTypeFlagsEXT messageTypes, DebugUtilsMessengerCallbackDataEXT* pCallbackData, void* pUserData)
    {
        var message = Marshal.PtrToStringAnsi((nint)pCallbackData->PMessage);
        if (message != null)
        {
            Console.WriteLine($"Validation Layer:" + message);
        }

        return Vk.False;
    }

    public static implicit operator Instance(VulkanInstance i) => i.Instance;
}
