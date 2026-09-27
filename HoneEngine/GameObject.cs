using System.Dynamic;

public class GameObject
{
    public Transform transform = new Transform();
    public List<Actor> components = new List<Actor>();
    public string name { get; set; }
    public string ID { get; set; } = string.Empty;
    public GameObject(string _name) : this(_name, SceneManager.ActiveScene)
    {
    }
    public GameObject(String _name, Scene _scene)
    {
        this.name = _name;
        //Program.AllObjects.Add(this);

        //добавляемся в сцену текущую
        if (_scene != null)
        {
            _scene.Objects.Add(this);
        }
    }

    public T AddComponent<T>() where T : Actor, new()
    {
        // Создаем сам скрипт
        T component = new T();

        // Говорим скрипту: "Твой хозяин — этот GameObject"
        component.GameObject = this;

        // Клеим в список компонентов этого объекта
        components.Add(component);

        // Сразу запускаем Start()
        //component.Start();

        return component;
    }

    public T? GetComponent<T>() where T : Actor
    {
        // Проходимся по всем компонентам этого объекта
        foreach (var component in components)
        {
            // Если компонент является типом T (или его наследником)
            if (component is T target)
            {
                return target;
            }
        }

        // Если ничего не нашли — возвращаем null
        return null;
    }
    public void Destroy()
    {
        //удаляемся из списка сцены
    }
}