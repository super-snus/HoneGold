public class TestComponent : Actor
{
    public override void Start()
    {
        Console.WriteLine("TestComponent Started!");
        GameObject.transform.position.Y = 200;
    }

    public override void Update()
    {
        GameObject.transform.position.X += 0.1f;
        //Console.WriteLine("TestComponent Update!");
        if (GameObject.transform.position.X > 500)
        {
            GameObject.transform.position.X = 0;
            Console.WriteLine(GameObject.name);
        }
        
    }
}