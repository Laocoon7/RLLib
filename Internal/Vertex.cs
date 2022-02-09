using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Silk.NET.Maths;
using Silk.NET.Vulkan;

namespace RLLib;

internal struct Vertex
{
    public Vector3D<float> Position;
    public Vector2D<float> TextureCoord;
    public Vector4D<float> Color;
    public Vector4D<float> BackColor;

    public static VertexInputBindingDescription GetBindingDescription()
    {
        VertexInputBindingDescription bindingDescription = new()
        {
            Binding = 0,
            Stride = (uint)Unsafe.SizeOf<Vertex>(),
            InputRate = VertexInputRate.Vertex,
        };

        return bindingDescription;
    }

    public static VertexInputAttributeDescription[] GetAttributeDescriptions()
    {
        var attributeDescriptions = new[]
        {
            new VertexInputAttributeDescription()
            {
                Binding = 0,
                Location = 0,
                Format = Format.R32G32B32Sfloat,
                Offset = (uint)Marshal.OffsetOf<Vertex>(nameof(Position)),
            },
            new VertexInputAttributeDescription()
            {
                Binding = 0,
                Location = 1,
                Format = Format.R32G32Sfloat,
                Offset = (uint)Marshal.OffsetOf<Vertex>(nameof(TextureCoord)),
            },
            new VertexInputAttributeDescription()
            {
                Binding = 0,
                Location = 2,
                Format = Format.R32G32B32A32Sfloat,
                Offset = (uint)Marshal.OffsetOf<Vertex>(nameof(Color)),
            },
            new VertexInputAttributeDescription()
            {
                Binding = 0,
                Location = 3,
                Format = Format.R32G32B32A32Sfloat,
                Offset = (uint)Marshal.OffsetOf<Vertex>(nameof(BackColor)),
            }
        };

        return attributeDescriptions;
    }
}
