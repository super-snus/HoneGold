using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using HoneEngine;

// Структура для главного конфига игры
class GameConfig
{
    public string Name { get; set; }
    public string Version { get; set; }
    public List<string> Modules { get; set; }
    public List<SceneMeta> Scenes { get; set; }
}

class Program
{
    //public static List<GameObject> AllObjects = new List<GameObject>();
    //public static List<Scene> Scenes = new List<Scene>();
    //public static List<GameObject> RenderObjects = new List<GameObject>();

    public static IRenderer RenderModule = null;
    public static IPhysics PhysicsModule = null;

    public static void Main(string[] args)
    {
        // 1. Настраиваем правильные пути (как в Unity: папка mem_GameData)
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string gameDataPath = Path.Combine(baseDir, "mem_GameData");
        string modulesPath = Path.Combine(gameDataPath, "Modules");
        string assetsPath = Path.Combine(gameDataPath, "Assets");

        string _game_patch = Path.Combine(assetsPath, "HoneGame.dll");

        AssemblyLoadContext.Default.Resolving += (context, assemblyName) =>
        {
            if (assemblyName.Name == "HoneGame")
            {
                if (File.Exists(_game_patch)) return context.LoadFromAssemblyPath(_game_patch);
            }
            return null;
        };


        //Type _game_type = _game_assembly.GetTypes();

        // 2. Читаем главный GameConfig.json из папки Modules
        string gameConfigPath = Path.Combine(gameDataPath, "GameConfig.json");
        
        if (!File.Exists(gameConfigPath))
        {
            Console.WriteLine($"ОШИБКА: Не найден конфиг по пути: {gameConfigPath}");
            return;
        }

        string gameConfigJson = File.ReadAllText(gameConfigPath);
        GameConfig gameConfig = JsonSerializer.Deserialize<GameConfig>(gameConfigJson);

        // 3. Грузим модули из списка в конфиге
        foreach (string moduleFileName in gameConfig.Modules)
        {
            string modulePath = Path.Combine(modulesPath, moduleFileName);

            if (File.Exists(modulePath))
            {
                // Загружаем DLL в память
                Assembly moduleAssembly = Assembly.LoadFrom(modulePath);
                Console.WriteLine($"Loaded module DLL: {moduleAssembly.GetName().Name}");

                // Получаем список ВООБЩЕ ВСЕХ классов и типов, которые есть в этой DLL
                Type[] allTypes = moduleAssembly.GetTypes();

                // Перебираем каждый тип по очереди
                foreach (Type t in allTypes)
                {
                    // Сразу пропускаем интерфейсы и абстрактные классы (их нельзя создать через new)
                    if (t.IsInterface || t.IsAbstract) 
                        continue;

                    // 1. Проверяем, является ли класс Рендерером
                    if (typeof(IRenderer).IsAssignableFrom(t))
                    {
                        RenderModule = (IRenderer)Activator.CreateInstance(t);
                        Console.WriteLine($"УСПЕХ: Загружен IRenderer из класса {t.Name}");
                    }

                    // 2. Проверяем, является ли класс Физикой
                    if (typeof(IPhysics).IsAssignableFrom(t))
                    {
                        PhysicsModule = (IPhysics)Activator.CreateInstance(t);
                        Console.WriteLine($"УСПЕХ: Загружен IPhysics из класса {t.Name}");
                    }

                    // 3. Сюда можно добавить еще проверки (IAudio, INetwork и т.д.)
                    // if (typeof(IAudio).IsAssignableFrom(t)) { ... }
                }
            }
            else
            {
                Console.WriteLine("Файл DLL не найден.");
            }
        }

        // как то перенести сцены все в SceneManager
        // ^^^ короче то что выше ну типа всё мы уже переносим мемечлен

        // --- ИНИЦИАЛИЗАЦИЯ ДВИЖКА ---

        float _physicsTimeStep = 1f / 60f;
        float _accumulator = 0f;

        // Инициализируем окно через наш динамический RenderModule
        RenderModule?.InitWindow(640, 480, "Hone Engine Test");
        
        // (Физику и инпут пока оставим закомментированными или подключишь по аналогии) (anthology)
        //IPhysics physics = new PhysicsModule();
        PhysicsModule?.Init();

        Application.Init(baseDir, gameDataPath, assetsPath);

        SceneManager.Init(gameConfig.Scenes);
        SceneManager.LoadScene(0);

        // Старт компонентов
        foreach (var scene in SceneManager.LoadedScenes)
        {
            foreach (var obj in scene.Objects)
            {
                foreach (var component in obj.components)
                {
                    //if (component is SpriteRenderer)
                    //{
                    //    RenderObjects?.Add(obj);
                    //}
                    component.Start();
                }
            }
        }
        
        // Главный цикл движка
        while (!RenderModule.WindowShouldClose())
        {
            RenderModule?.FrameStart(); 

            foreach (var scene in SceneManager.LoadedScenes)
            {
                foreach (var obj in scene.Objects)
                {
                    foreach (var component in obj.components)
                    {
                        component.Update();
                    }
                }
            }
            // Физика
            _accumulator += RenderModule.deltaTime();
            while (_accumulator >= _physicsTimeStep)
            {
                List<GameObject> _physics_objects = new List<GameObject>();
                foreach (var scene in SceneManager.LoadedScenes)
                {
                    foreach (var obj in scene.Objects)
                    {
                        _physics_objects.Add(obj);
                        foreach (var component in obj.components)
                        {
                            component.FixedUpdate();
                        }
                    }
                }
                PhysicsModule?.Step(_physics_objects, _physicsTimeStep);
                _accumulator -= _physicsTimeStep;
            }
            
            List<GameObject> _render_objects = new List<GameObject>();
            foreach (var scene in SceneManager.LoadedScenes)
            {
                foreach (var obj in scene.Objects)
                {
                    _render_objects?.Add(obj);
                    Console.WriteLine(obj.name);
                }
            }
            RenderModule?.Draw(_render_objects);

            
            RenderModule?.FrameEnd();
        }
    }
}
