public class GameObject
{
    public Transform transform = new Transform();
    public List<Actor> components = new List<Actor>();
    public string name;
    
    public GameObject(String _name)
    {
        this.name = _name;
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
        component.Start();

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
}