using Raylib_cs;
public class RaylibInput : IInput
{
    public override bool IsKeyPressed(int key) { return Raylib.IsKeyPressed((KeyboardKey)key); } // только в первый кадр после нажатия клавишы
    public override bool IsKeyDown(int key) { return Raylib.IsKeyDown((KeyboardKey)key); } // каждый кадр когда клавиша попущена
    public override bool IsKeyUp(int key) { return Raylib.IsKeyUp((KeyboardKey)key); } // каждый кадр когда клавиша поднята
    public override int GetKeyPressed() { return Raylib.GetKeyPressed(); } // возвращает код нажатой клавиши
    public override int GetCharPressed() { return Raylib.GetCharPressed(); } // возвращает символ нажатой клавиши в юникод
}