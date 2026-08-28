using System;
class Program
{
    public static List<GameObject> AllObjects = new List<GameObject>();

    public static List<GameObject> RenderObjects = new List<GameObject>();
    public static void Main(string[] args)
    {
        IRenderer renderer = new RayLibRender(); // RayLibRender on
        renderer.InitWindow(640, 480, "Hone Engine Test");

        IPhysics physics = new Physics();
        physics.Init();

        IInput input = new RaylibInput();
        Input.Initialize(input);

        // пока нету редактора тут мы создаём мемемебъекты и типа делаем всяокеамп
        GameObject test = new GameObject("TestObj");
        test.transform.position = new System.Numerics.Vector3(0, 0, 0);
        test.AddComponent<TestComponent>();

        Texture mem_texture = new Texture();
        mem_texture.Load("/mnt/data/HoneCS/HoneGold/Core/mem.png");

        Sprite spirt = test.AddComponent<Sprite>();
        spirt.texture = mem_texture;
        
        test.AddComponent<SpriteRenderer>().sprite = spirt;

        RigitBody2D testrb = test.AddComponent<RigitBody2D>();
        testrb.mass = 0f;

        //тупо крутая платформа для теста мемного
        GameObject mememe = new GameObject("blyatforma");
        mememe.transform.scale = new System.Numerics.Vector3(100, 1, 1);
        RigitBody2D rbmem = mememe.AddComponent<RigitBody2D>();
        rbmem.BodyType = 0;
        mememe.transform.position = new System.Numerics.Vector3(0, -3, 0);
        Sprite lol = mememe.AddComponent<Sprite>();
        lol.texture = mem_texture;
        mememe.AddComponent<SpriteRenderer>().sprite = lol;


        GameObject mememe2 = new GameObject("blyatforma");
        mememe2.transform.scale = new System.Numerics.Vector3(1, 3, 1);
        RigitBody2D rbmem2 = mememe2.AddComponent<RigitBody2D>();
        rbmem2.BodyType = 0;
        mememe2.transform.position = new System.Numerics.Vector3(0.6f, -3, 0);
        Sprite lol2 = mememe2.AddComponent<Sprite>();
        lol2.texture = mem_texture;
        mememe2.AddComponent<SpriteRenderer>().sprite = lol2;
        // ah~


        // это старт движка, здесь у каждого объекта, в каждом компоненте, мы вызываем Start();
        foreach (var Object in AllObjects)
            {
            foreach(var component in Object.components)
            {
                // RenderObjects.Add(Object);
                //добавляем мемный объект в ренер
                 if (component is SpriteRenderer)
                 {
                     RenderObjects.Add(Object);
                     Console.WriteLine("mem: " + Object.name);
                 }
                component.Start();
            }
        }
        
        // это бесконечный цикл движка, здесь у каждого объекта, в каждом компоненте, мы вызываем Update();
        while (!renderer.WindowShouldClose())
        {
            //Raylib.BeginDrawing();
            renderer.FrameStart(); 


            foreach (var Object in AllObjects)
            {
                foreach(var component in Object.components)
                {
                    component.Update();
                }
            }
            //Raylib.DrawText("Hello, World!", 10, 10, 20, Color.Blue);
            //Raylib.EndDrawing();
            if (RenderObjects != null)
            {
                renderer.Draw(RenderObjects);   
            }
            physics.Step(AllObjects, renderer.deltaTime());
            renderer.FrameEnd();
        }
    }
}
