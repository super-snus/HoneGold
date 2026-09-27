namespace HoneEngine
{
    public static class Application
    {
        public static string BaseDirectoryPath { get; private set; } = string.Empty;
        public static string GameDataPath { get; private set; } = string.Empty;
        public static string AssetsPath { get; private set; } = string.Empty;

        public static void Init(string baseDirectory, string gameDataPath, string assetsPath)
        {
            BaseDirectoryPath = baseDirectory;
            GameDataPath = gameDataPath;
            AssetsPath = assetsPath;
        }
    }
}