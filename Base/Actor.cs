public class Actor 
{
    public GameObject GameObject { get; internal set; }
    public bool Enabled = true;
    
    // Виртуальные методы, которые игрок переопределит
    public virtual void Start() {}
    public virtual void Update() {}

}
