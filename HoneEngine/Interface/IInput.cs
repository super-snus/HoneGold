public class IInput {
    public virtual bool IsKeyPressed(int key) { return false; } // только в первый кадр после нажатия клавишы
    public virtual bool IsKeyDown(int key) { return false; } // каждый кадр когда клавиша попущена
    public virtual bool IsKeyUp(int key) { return false; } // каждый кадр когда клавиша поднята
    public virtual int GetKeyPressed() { return 0; } // возвращает код нажатой клавиши
    public virtual int GetCharPressed() { return '0'; } // возвращает символ нажатой клавиши в юникод
}