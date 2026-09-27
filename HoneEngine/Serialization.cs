using System.Collections.Generic;
using System.Text.Json;


public class SceneDataDto
{
    public string Name { get; set; } = string.Empty;
    public List<GameObjectDto> Objects { get; set; } = new();
}

public class GameObjectDto
{
    public string ID { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public TransformDto Transform { get; set; } = new();
    public List<ComponentDto> Components { get; set; } = new();
}

public class TransformDto
{
    public Vector3Dto Position { get; set; } = new();
    public Vector3Dto Rotation { get; set; }
    public Vector3Dto Scale { get; set; } = new();
}

public class Vector2Dto
{
    public float X { get; set; }
    public float Y { get; set; }
}

public class Vector3Dto
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

public class ComponentDto
{
    // Имя класса компонента (например, "SpriteRenderer" или "TestComponent")
    public string Type { get; set; } = string.Empty;

    public string ID { get; set; } = string.Empty;

    // Сырые данные свойств компонента в формате JSON элемента
    public Dictionary<string, JsonElement> Properties { get; set; } = new();
}