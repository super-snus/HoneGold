using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using HoneEngine;

public class SceneManager
{
    static List<SceneMeta> _regScenesMeta = new List<SceneMeta>();

    static public List<Scene> LoadedScenes = new List<Scene>();
    static public Scene ActiveScene;
    static public Dictionary<string, Actor> ActorDictionary = new Dictionary<string, Actor>();
    static public Dictionary<string, GameObject> ObjectDictionary = new Dictionary<string, GameObject>();
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
        
        string sceneFile = File.ReadAllText(Path.Combine(Application.AssetsPath, _regScenesMeta[index].Path)); // тут теперь короче ээээ ну относительный путь типа лол кек (то есть корень теперь в assets папке)
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        SceneDataDto dto = JsonSerializer.Deserialize<SceneDataDto>(sceneFile, options);

        Scene scene = new Scene(dto.Name);
        ActiveScene = scene;

        // тут короче проходимся и записываем всё в книжечку
        foreach (var objDto in dto.Objects)
        {
            deb("Первый проно где мы крочч чч просто вфывфывфывфывфывфывфыв");
            // загружаемобъект
            GameObject gameObject = new GameObject(objDto.Name, scene);

            gameObject.ID = objDto.ID; // туда короче идидидидидидди щвалпидвыл загруzka

            ObjectDictionary.Add(objDto.ID, gameObject);
            
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

                component.ID = componentDto.ID;

                gameObject.components.Add(component); // добавляем компонент на объект

                //заносим в книжечку наш компонент
                ActorDictionary.Add(componentDto.ID, component);
            }
        }


        //жеперь сценка есть, всо мем
        // тут проходимся по объектам в dto
        foreach (var objDto in dto.Objects)
        {
            //deb("--- Начало загрузки объекта");
            // загружаем трансформ
            //GameObject gameObject = new GameObject(objDto.Name, scene);

            GameObject gameObject = ObjectDictionary[objDto.ID];

            //deb("Позиция объекта выставлена");

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

                // берём наш компот из спикока
                //Actor component = (Actor)Activator.CreateInstance(componentType);
                Actor component = ActorDictionary[componentDto.ID];


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
                        // 1. Узнаем, какой тип данных у этого поля в коде класса
                        Type fieldType = field.FieldType;

                        // 2. Проверяем: поле ссылается на GameObject?
                        if (fieldType == typeof(GameObject))
                        {
                            string id = prop.Value.GetString(); // В JSON лежит ID объекта
                            GameObject obj = ObjectDictionary[id]; // Ищем в словаре
                            field.SetValue(component, obj); // Присваиваем
                        }
                        // 3. Или поле ссылается на какой-то компонент (наследник Actor)?
                        else if (typeof(Actor).IsAssignableFrom(fieldType))
                        {
                            string id = prop.Value.GetString(); // В JSON лежит ID компонента
                            Actor comp = ActorDictionary[id]; // Ищем в словаре
                            field.SetValue(component, comp); // Присваиваем
                        }
                        // 4. Во всех остальных случаях (числа, строки, векторов) — это обычные данные
                        else
                        {
                            object val = JsonSerializer.Deserialize(propValue, fieldType);
                            field.SetValue(component, val); // Присваиваем обычное значение
                        }
                    }
                    
                    PropertyInfo property = componentType.GetProperty(propName, flags); // получаем проперти по флагам которые раньше были выше и по названию проперти

                    if (property != null && property.CanWrite)
                    {
                        // 1. Узнаем, какой тип данных у этого поля в коде класса
                        Type propertyType = property.PropertyType;

                        // 2. Проверяем: поле ссылается на GameObject?
                        if (propertyType == typeof(GameObject))
                        {
                            string id = prop.Value.GetString(); // В JSON лежит ID объекта
                            GameObject obj = ObjectDictionary[id]; // Ищем в словаре
                            property.SetValue(component, obj); // Присваиваем
                        }
                        // 3. Или поле ссылается на какой-то компонент (наследник Actor)?
                        else if (typeof(Actor).IsAssignableFrom(propertyType))
                        {
                            string id = prop.Value.GetString(); // В JSON лежит ID компонента
                            Actor comp = ActorDictionary[id]; // Ищем в словаре
                            property.SetValue(component, comp); // Присваиваем
                        }
                        // 4. Во всех остальных случаях (числа, строки, векторов) — это обычные данные
                        else
                        {
                            object val = JsonSerializer.Deserialize(propValue, propertyType);
                            property.SetValue(component, val); // Присваиваем обычное значение
                        }
                    }
                }
            }
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
            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                // Если часть типов в сборке сломана, берем хотя бы те, которые загрузились
                types = ex.Types.Where(t => t != null).ToArray()!;
            }

            var type = types.FirstOrDefault(t => 
                (t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase) || 
                (t.FullName != null && t.FullName.Equals(typeName, StringComparison.OrdinalIgnoreCase)))
                && typeof(Actor).IsAssignableFrom(t));

            if (type != null) 
            {
                deb($"[УСПЕХ] Найден тип компонента: {type.FullName} в сборке {assembly.GetName().Name}");
                return type;
            }
        }

        // Если всё ещё не найден, пишем в консоль все просканированные сборки для проверки
        deb($"[ОШИБКА] Тип {typeName} не найден ни в одной из сборок:");
        foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
        {
            deb($"  - Сборка: {a.GetName().Name}");
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