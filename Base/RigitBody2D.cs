using System.Numerics;

class RigitBody2D : Actor
{
    public Vector2 velocity = new Vector2(0, 0);

    public Vector2 _pendingVelocityDelta = new Vector2(0, 0);
    public Vector2? _targetVelocity = null;
    public float mass = 1f;
    public int BodyType = 2; // 2 - Dynamic body

    public void AddVelocity( Vector2 vec )
    {
        _pendingVelocityDelta = vec;
    }

    public void SetVelocity( Vector2 vec )
    {
        _targetVelocity = vec;
    }
}