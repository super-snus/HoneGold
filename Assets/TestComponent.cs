using System.ComponentModel;

public class TestComponent : Actor
{
    public override void Start()
    {
    }

    public override void Update()
    {
        if (Input.IsKeyDown(87))
        {
            GameObject.transform.position.Y += 0.2f;
        }
    }
}