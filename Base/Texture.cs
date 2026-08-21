using Raylib_cs;
public class Texture
{
    public Raylib_cs.Texture2D texture;
    public string texture_patch;
    public void Load(string fileName)
    {
        texture_patch = fileName;
        texture = Raylib.LoadTexture(fileName);
    }
}