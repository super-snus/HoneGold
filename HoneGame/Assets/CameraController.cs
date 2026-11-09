using System.Numerics;

public class CameraController : Actor
{
    public GameObject target;
    public Camera cam;
    public override void Start()
    {
        Render.SetCamera(cam);
    }
    public override void Update()
    {
        //Console.WriteLine("JJJJJJ");
        //GameObject.transform.position = Vector3.Zero;
        if (target == null) { return; }

        GameObject.transform.position = target.transform.position;
    }
}