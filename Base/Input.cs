public static class Input
{
    // Ссылка на твой интерфейс
    private static IInput _provider;

    // Метод инициализации (вызываешь один раз при старте движка)
    public static void Initialize(IInput provider)
    {
        _provider = provider;
    }

    // Статические пробросы методов (теперь доступны везде!)
    public static bool IsKeyPressed(int key)
    {
        return _provider?.IsKeyPressed(key) ?? false;
    }

    public static bool IsKeyDown(int key)
    {
        return _provider?.IsKeyDown(key) ?? false;
    }

    public static int GetKeyPressed()
    {
        return _provider?.GetKeyPressed() ?? 0;
    }

    public static int GetCharPressed()
    {
        return _provider?.GetCharPressed() ?? '0';
    }
}