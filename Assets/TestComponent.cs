using System.ComponentModel;

public class TestComponent : Actor
{
    public override void Start()
    {
    }

    public override void Update()
    {
        if (Input.IsKeyPressed(87))
        {
            GameObject.GetComponent<RigitBody2D>().SetVelocity(new System.Numerics.Vector2(0, 5f));
        }
    }
}