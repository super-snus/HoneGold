using System.ComponentModel;

public class TestComponent : Actor
{
    public override void Start()
    {
    }

    public override void Update()
    {
        if (Input.IsKeyPressed(87)) //W
        {
            GameObject.GetComponent<RigitBody2D>().AddVelocity(new System.Numerics.Vector2(0, 5f));
        } else if (Input.IsKeyPressed(65)) //A
        {
            GameObject.GetComponent<RigitBody2D>().AddVelocity(new System.Numerics.Vector2(2f, 0));
        } else if (Input.IsKeyPressed(68)) //D
        {
            GameObject.GetComponent<RigitBody2D>().AddVelocity(new System.Numerics.Vector2(-2f, 0));
        } else if (Input.IsKeyPressed(82)) //R
        {
            GameObject.transform.position = new System.Numerics.Vector3(0, 0, 0);
        }
    }
}