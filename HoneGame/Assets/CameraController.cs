using System.Numerics;

public class CameraController : Actor
{
    public GameObject target;
    public Camera cam;
    public override void Start()
    {
        Render.SetCamera(cam);
    }
    public override void FixedUpdate()
    {
        //Console.WriteLine("JJJJJJ");
        //GameObject.transform.position = Vector3.Zero;
        if (target == null) { return; }

        //GameObject.transform.position = target.transform.position;
        GameObject.transform.position = Vector3.Lerp(GameObject.transform.position, target.transform.position, 0.02f);
    }
}