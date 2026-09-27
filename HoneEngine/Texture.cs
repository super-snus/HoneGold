using HoneEngine;
using Raylib_cs;
public class Texture
{
    public Raylib_cs.Texture2D texture;
    public string texture_patch;
    public void Load(string fileName)
    {
        texture_patch = Path.Combine(Application.AssetsPath, fileName);
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(texture_patch);
        texture = Raylib.LoadTexture(texture_patch);
    }
}