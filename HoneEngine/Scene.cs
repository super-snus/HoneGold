public class Scene
{
    public string Name { get; }
    public List<GameObject> Objects { get; } = new();

    public Scene(string name)
    {
        Name = name;
    }

    public void Destroy()
    {
        //типао удол лаем
        //for (int i = 0; i < Objects.Count; i++)
        //{
        //    Objects[i].Destroy();
        //}
        //Objects.Clear();
    }
}