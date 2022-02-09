using Silk.NET.Maths;

namespace RLLib;

internal abstract class Camera
{
    private Vector3D<float> m_Position;
    public virtual Vector3D<float> Position
    {
        get => m_Position;
        set
        {
            m_Position = value;
            RecalculateViewMatrix();
        }
    }

    private Matrix4X4<float> m_ViewMatrix;
    internal virtual Matrix4X4<float> ViewMatrix => m_ViewMatrix;

    internal abstract Matrix4X4<float> ProjectionMatrix { get; set; }

    public Camera() : this(0, 0, 0)
    {
    }
    public Camera(float positionX, float positionY, float positionZ)
    {
        Position = new Vector3D<float>(positionX, positionY, positionZ);
    }

    private void RecalculateViewMatrix()
    {
        var translation = Matrix4X4.CreateTranslation(Position);
        Matrix4X4.Invert(translation, out m_ViewMatrix);
    }
}

internal class OrthographicCamera : Camera
{
    internal override Matrix4X4<float> ProjectionMatrix { get; set; }

    public OrthographicCamera(float width, float height) : this(0.0f, 0.0f, width, height)
    {
    }

    public OrthographicCamera(float x, float y, float width, float height) : base(x, y, 0)
    {
        ProjectionMatrix = Matrix4X4.CreateOrthographic<float>(width, height, -1.0f, 1.0f);
    }
}