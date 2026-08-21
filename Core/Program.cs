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

        // пока нету редактора тут мы создаём мемемебъекты и типа делаем всяокеамп
        GameObject test = new GameObject("TestObj");
        test.transform.position.Y = 0f;
        test.transform.position.X = 0f;
        //test.AddComponent<TestComponent>();

        Texture mem_texture = new Texture();
        mem_texture.Load("/mnt/data/HoneCS/HoneGold/Core/mem.png");

        Sprite spirt = test.AddComponent<Sprite>();
        spirt.texture = mem_texture;
        
        test.AddComponent<SpriteRenderer>().sprite = spirt;

        RigitBody2D testrb = test.AddComponent<RigitBody2D>();
        testrb.mass = 0f;

        //тупо крутая платформа для теста мемного
        GameObject mememe = new GameObject("blyatforma");
        RigitBody2D rbmem = mememe.AddComponent<RigitBody2D>();
        rbmem.BodyType = 0;
        mememe.transform.position.Y = -3;
        Sprite lol = mememe.AddComponent<Sprite>();
        lol.texture = mem_texture;
        mememe.AddComponent<SpriteRenderer>().sprite = lol;
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
            physics.Step(AllObjects, renderer.deltaTime());


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

            renderer.FrameEnd();
        }
    }
}
