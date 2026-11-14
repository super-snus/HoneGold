using System.Xml.Serialization;

public class Actor 
{
    public GameObject GameObject { get; internal set; }
    public string ID { get; set; } = string.Empty;
    public bool Enabled = true;
    
    // старты всякие апдейты воооот
    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void FixedUpdate() {}

    // столкновения
    public virtual void OnCollisionEnter(GameObject gameObject) {}
    public virtual void OnCollisionStay(GameObject gameObject) {}
    public virtual void OnCollisionExit(GameObject gameObject) {}

}
