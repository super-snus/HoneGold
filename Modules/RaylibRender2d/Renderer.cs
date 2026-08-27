using System.Numerics;
using Raylib_cs;
using Silk.NET.Vulkan;

class RayLibRender : IRenderer
{
    int PPU = 64;
    public Camera2D camera = new Camera2D();
    public override void FrameStart()
    {
        Raylib.BeginDrawing();
    }
    public override void FrameEnd()
    {   
        Raylib.ClearBackground(Color.DarkBlue);
        Raylib.EndDrawing();
    }
    public override void Draw(List<GameObject> gameObjects)
    {
        Raylib.BeginMode2D(camera);
        foreach (var gameObject in gameObjects)
        {
            if (gameObject == null) { break; }
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            var sprite = spriteRenderer.sprite;
            var texture = sprite.texture;

            //применяем мемные размеры
            //texture.texture.Height = (int)(sprite.height * PPU);
            //texture.texture.Width = (int)(sprite.width * PPU);

            //выщитываем позицию
            float x = gameObject.transform.position.X * PPU;
            float y = gameObject.transform.position.Y * PPU;

            x = -x;
            y = -y;

            //float width = (sprite.width + gameObject.transform.scale.X) * PPU;
            //float height = (sprite.height + gameObject.transform.scale.Y) * PPU;

            float width = gameObject.transform.scale.X * PPU; // применяем только трансформ абъекта
            float height = gameObject.transform.scale.Y * PPU;

            Rectangle source = new Rectangle(
                0,
                0,
                texture.texture.Width,
                texture.texture.Height
            );

            Rectangle destination = new Rectangle(
                x,
                y,
                width,
                height
            );
            Vector2 origin = new Vector2(width/2, height/2);
            //Raylib.DrawTexture(texture.texture, x, y, Color.White);
            Raylib.DrawTexturePro(texture.texture, source, destination, origin, (gameObject.transform.rotation.Z * (180f / MathF.PI)), Color.White);
        }
    }
    public override bool WindowShouldClose()
    {
        return Raylib.WindowShouldClose();
    }

    public override void InitWindow(int width, int height, string title)
    {
        Raylib.InitWindow(width, height, title);

        camera = new Camera2D();
        camera.Target = new System.Numerics.Vector2(0, 0); // На что смотрит камера в мире
        camera.Offset = new System.Numerics.Vector2(width / 2f, height / 2f); // Центр экрана (0,0 будет посредине)
        camera.Rotation = 0.0f;
        camera.Zoom = 1.0f; // Можно приближать/отдалять мир
    }

    public override float deltaTime()
    {
        return Raylib.GetFrameTime();
    }
}