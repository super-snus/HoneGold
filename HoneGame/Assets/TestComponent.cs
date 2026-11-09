using System.ComponentModel;
using System.Numerics;

public class TestComponent : Actor
{
    private RigitBody2D _rb;
    //private Camera cam;
    private SpriteRenderer _spriteRenderer;
    private Sprite _sprite;
    private Texture _texture;
    public override void Start()
    {
        Console.WriteLine("Test component started");
        _rb = GameObject.GetComponent<RigitBody2D>();
        //Console.WriteLine("Test component started2");
        
        //cam = GameObject.GetComponent<Camera>();
        //Render.SetCamera(cam);

        //Console.WriteLine();

        _texture = new Texture();
        _texture.Load("mem.png");

        _sprite = GameObject.GetComponent<Sprite>();

        _sprite.texture = _texture;

        if (_sprite != null) { Console.WriteLine("спрайт не равен null"); } else { Console.WriteLine("спрайт равен нулю"); }
        //Console.WriteLine("Test component started3");

        _spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
        //Console.WriteLine("Test component started4");
        //_spriteRenderer.sprite = _sprite; // вот если есть спрайт в спрайтрендерере то оно почему то неработает (а щос да)
        //Console.WriteLine("Test component started5!!!!");
    }

    public override void Update()
    {
        
        
        //Console.WriteLine("Test component update");
        if (Input.IsKeyPressed(87)) //W
        {
            _rb.AddVelocity(new System.Numerics.Vector2(0, 5f));
            Console.WriteLine("W");
        } else if (Input.IsKeyPressed(65)) //A
        {
            _rb.AddVelocity(new System.Numerics.Vector2(2f, 0));
            Console.WriteLine("A");
        } else if (Input.IsKeyPressed(68)) //D
        {
            _rb.AddVelocity(new System.Numerics.Vector2(-2f, 0));
            Console.WriteLine("D");
        } else if (Input.IsKeyPressed(82)) //R
        {
            GameObject.transform.position = new System.Numerics.Vector3(0, 0, 0);
            Console.WriteLine("R");
        }
        //Console.WriteLine("Test component update2");
    }
}