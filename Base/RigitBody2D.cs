using System.Numerics;

class RigitBody2D : Actor
{
    public Vector2 velocity = new Vector2(0, 0);
    public float mass = 1f;
    public int BodyType = 2; // 2 - Dynamic body
}