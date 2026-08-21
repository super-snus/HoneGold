using Silk.NET.GLFW;

public class IRenderer
{
    public virtual void InitWindow(int width, int height, string title) {}
    public virtual void FrameStart() {}
    public virtual void FrameEnd() {}
    public virtual void Draw(List<GameObject> gameObjects) {}
    public virtual bool WindowShouldClose() { return false; }
    public virtual float deltaTime() { return 0f; }
}