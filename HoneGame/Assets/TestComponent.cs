using System.ComponentModel;
using System.Numerics;

public class TestComponent : Actor
{
    private RigitBody2D _rb;
    public override void Start()
    {
        _rb = GameObject.GetComponent<RigitBody2D>();
    }

    public override void Update()
    {
        if (Input.IsKeyPressed(87)) //W
        {
            _rb.AddVelocity(new System.Numerics.Vector2(0, 5f));
        } else if (Input.IsKeyPressed(65)) //A
        {
            _rb.AddVelocity(new System.Numerics.Vector2(2f, 0));
        } else if (Input.IsKeyPressed(68)) //D
        {
            _rb.AddVelocity(new System.Numerics.Vector2(-2f, 0));
        } else if (Input.IsKeyPressed(82)) //R
        {
            GameObject.transform.position = new System.Numerics.Vector3(0, 0, 0);
        }
    }
}