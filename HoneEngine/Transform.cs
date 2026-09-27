using System.Numerics;

public class Transform
{
    public bool SizeChanged = false;
    public bool PositionChanged = false;
    public bool RotationChanged = false;
    private Vector3 _position = new Vector3(0, 0, 0);
    private Vector3 _scale = new Vector3(1, 1, 1);
    private Vector3 _rotation = new Vector3(0, 0, 0);

    public Vector3 position
    {
        get => _position;
        set
        {
            if (_position != value)
            {
                _position = value;
                PositionChanged = true;
            }
        }
    }

        public Vector3 scale
    {
        get => _scale;
        set
        {
            if (_scale != value)
            {
                _scale = value;
                SizeChanged = true;
            }
        }
    }

    public Vector3 rotation
    {
        get => _rotation;
        set
        {
            if (_rotation != value)
            {
                _rotation = value;
                RotationChanged = true;
            }
        }
    }
}