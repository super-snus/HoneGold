using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;

public class SceneManager
{
    static List<SceneMeta> _regScenesMeta = new List<SceneMeta>();

    static public List<Scene> LoadedScenes = new List<Scene>();
    static public Scene ActiveScene;
    static public void Init(List<SceneMeta> _scenesMeta)
    {
        _regScenesMeta.AddRange(_scenesMeta);
        deb("Инициализация прошла");
    }
    static public Scene LoadScene(int index)
    {
        deb("--- Начало загрузки сцены");
        // Как-то получаем сцены из конфига игрового

        // После находки идём читаем файл сцены
        // Делаем сцену в ActiveScene
        // Грузим вот это: Scene
        string sceneFile = File.ReadAllText(_regScenesMeta[index].Path);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        SceneDataDto dto = JsonSerializer.Deserialize<SceneDataDto>(sceneFile, options);

        Scene scene = new Scene(dto.Name);
        ActiveScene = scene;

        //жеперь сценка есть, всо мем
        // тут проходимся по объектам в dto
        foreach (var objDto in dto.Objects)
        {
            deb("--- Начало загрузки объекта");
            // загружаем трансформ
            GameObject gameObject = new GameObject(objDto.Name, scene);
            //gameObject.transform.position.X = objDto.Transform.Position.X;
            gameObject.transform.position = ConvertVector2Dto(objDto.Transform.Position);
            gameObject.transform.rotation = ConvertVector2Dto(objDto.Transform.Rotation);
            gameObject.transform.scale = ConvertVector2Dto(objDto.Transform.Scale);

            deb("Позиция объекта выставлена");

            //компоненты мема
            // проходимся по компонентам внутри объекта которой мы достали из dto (а dto мы взяли из файла сцены мем)
            foreach (var componentDto in objDto.Components)
            {
                deb("-- начало загрузки компонентов объекта");
                //загружаем тип компонента
                Type componentType = FindComponentType(componentDto.Type);
                if (componentType == null)
                {
                    deb($"componentType == null компонент пропускаеться {componentDto.Type}");
                    continue;
                }

                // создаём наш компонент в мемас движухе
                Actor component = (Actor)Activator.CreateInstance(componentType);
                component.GameObject = gameObject; // указываем объект к которому привязан компонент

                gameObject.components.Add(component); // добавляем компонент на объект

                //тут применить свойства компонентов типа проперти и свойства
                BindingFlags flags = BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance; // тут это флаги что бы искать только публик что бы ещё что то я хз короче какашка
                Dictionary<string, JsonElement> component_props = componentDto.Properties; // пихаем все поля компонента в Dictionary что бы пройтись по нему и каждому добавить поле
                foreach (var prop in component_props)
                {
                    // проходимся по значениям и задаём их
                    deb("-- Начало загрузки значений и свойств компонента");
                    string propName = prop.Key;
                    string propValue = prop.Value.GetRawText(); // просто в переменную запихиваем мемасию

                    FieldInfo field = componentType.GetField(propName, flags); // получаем FieldInfo (какаша которая типа о поле инфу даёт какой тип там типа публик или не и тд тп лалка кек хз не шарю полностью но догадываюсь)
                    if (field != null)
                    {
                        object parsedValue = JsonSerializer.Deserialize(propValue, field.FieldType); // получаем значение которое мы будем ложить в мемы корорчваоп

                        field.SetValue(component, parsedValue); // а тут вот мы уже задаём

                        deb($"{field.Name} компонента загружен");
                        continue;
                    }
                    
                    PropertyInfo property = componentType.GetProperty(propName, flags); // получаем проперти по флагам которые раньше были выше и по названию проперти
                    if (property != null && property.CanWrite)
                    {
                        object parsedValue = JsonSerializer.Deserialize(propValue, property.PropertyType); // тут берём то что находиться в файле типа лол в значении

                        deb($"{property.Name} компонента загружен");
                        property.SetValue(component, parsedValue); // ну и задаём короче
                    }
                }
            }
        }

        // а вот и нет!!!!
        // тут второй проход по объектам для так скажем "догрузки" ссылок, что бы например в SpriteRenderer поле sprite было заполнено и мы видили объект
        foreach (var objectDto in dto.Objects)
        {
            //objectDto
        }
        

        LoadedScenes.Add(scene);
        return scene;
    }

    static Vector3 ConvertVector2Dto(Vector3Dto vector3Dto)
    {
        return new Vector3(vector3Dto.X, vector3Dto.Y, vector3Dto.Z);
    }

    private static Type FindComponentType(string typeName)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var type = assembly.GetTypes()
                .FirstOrDefault(t => t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase) 
                                  && typeof(Actor).IsAssignableFrom(t));
            if (type != null) return type;
        }
        return null;
    }

    static void deb(string text)
    {
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine(text);
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
    }
}