public class Actor 
{
    public GameObject GameObject { get; internal set; }
    public string ID { get; set; } = string.Empty;
    public bool Enabled = true;
    
    // Виртуальные методы
    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void FixedUpdate() {}

}
