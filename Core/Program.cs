using System;
class Program
{
    public static List<GameObject> AllObjects = new List<GameObject>();
    public static void Main(string[] args)
    {
    
        GameObject test = new GameObject("TestObj");
        test.AddComponent<TestComponent>();
        // это старт движка, здесь у каждого объекта, в каждом компоненте, мы вызываем Start();
        foreach (var Object in AllObjects)
            {
            foreach(var component in Object.components)
            {
                component.Start();
            }
        }
        
        // это бесконечный цикл движка, здесь у каждого объекта, в каждом компоненте, мы вызываем Update();
        while (true)
        {
            foreach (var Object in AllObjects)
            {
                foreach(var component in Object.components)
                {
                    component.Update();
                }
            }
        }
    }
}
