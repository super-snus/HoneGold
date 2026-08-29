public class Actor 
{
    public GameObject GameObject { get; internal set; }
    public bool Enabled = true;
    
    // Виртуальные методы
    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void FixedUpdate() {}

}
