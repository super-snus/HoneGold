public static class Render
{ 
    private static IRenderer _provider;

    public static void Initialize(IRenderer provider)
    {
        _provider = provider;
    }

    // Статические пробросы методов (теперь доступны везде!)
    
    // Задать текущую камеру
    public static void SetCamera(Camera cameraObject)
    {
        _provider?.SetCamera(cameraObject);
    }
}