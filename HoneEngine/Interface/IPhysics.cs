public class IPhysics
{
    public virtual void Init() { }
    public virtual void Step(List<GameObject> gameObjects, float deltaTime) { }
    public virtual bool CheckCollision(GameObject a, GameObject b) { return false; }
}