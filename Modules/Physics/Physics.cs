using System.Numerics;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Collision.Shapes;
using Silk.NET.Vulkan;

public class Physics : IPhysics
{
    private Dictionary<RigitBody2D, Body> bodyMap = new Dictionary<RigitBody2D, Body>();
    Vector2 gravity;
    World world;
    public override void Init()
    {
        //bodyMap = new Dictionary<RigitBody2D, B2Body>();
        gravity = new Vector2(0, -10);
        world = new World(ToPhysics(gravity));
        
    }
    public override void Step(List<GameObject> gameObjects, float deltaTime)
    {
        foreach (var gameObject in gameObjects)
        {
            // тут проверяем, если у объекта нету ассоциации в списке с body то создаём ему там всё иначе просто передаём корды на transform и всо
            RigitBody2D rigitBody2D = gameObject.GetComponent<RigitBody2D>();
            if (rigitBody2D == null) continue; // если какахи нет то мы не кушаем её
            if (bodyMap.ContainsKey(rigitBody2D)) // есть в спыске
            {
                Body body = bodyMap[rigitBody2D];

                //применяем позицию из физики в наш движок для рендера и тд
                gameObject.transform.position.X = body.Position.X;
                gameObject.transform.position.Y = body.Position.Y;
                gameObject.transform.rotation.Z = body.Rotation;
                
                if (rigitBody2D._targetVelocity.HasValue)
                {
                    //жестоко перезапипиндывавем жозы
                    body.LinearVelocity = ToPhysics(rigitBody2D._targetVelocity.Value);
                    rigitBody2D._targetVelocity = null;
                } else
                {
                    //применяем _pendingVelocityDelta из рб объекта
                    body.LinearVelocity += ToPhysics(rigitBody2D._pendingVelocityDelta);
                    rigitBody2D._pendingVelocityDelta = new Vector2(0, 0);   
                }
            } else // нету в списке
            {
                Body body = BodyFactory.CreateBody(world, ToPhysics(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y))); // создаём физический мем
                body.BodyType = IntToBodyType(rigitBody2D.BodyType); // тип объекта мемного
                body.Mass = rigitBody2D.mass;

                FixtureFactory.AttachRectangle(1f, 1f, 1f, ToPhysics(new Vector2(0f, 0f)), body); // создаём хитбокс типа мемемемеме lkz abpbxyjuj rfrfirj

                bodyMap.Add(rigitBody2D, body);
            }
        }

        world.Step(deltaTime);
    }
    private BodyType IntToBodyType(int value)
    {
        return value switch
        {
            0 => BodyType.Static,
            1 => BodyType.Kinematic,
            2 => BodyType.Dynamic,
            _ => BodyType.Dynamic
        };
    }
    public override bool CheckCollision(GameObject a, GameObject b)
    {
        return false;
    }

    private Microsoft.Xna.Framework.Vector2 ToPhysics(Vector2 sysVec)
    {
        return new Microsoft.Xna.Framework.Vector2(sysVec.X, sysVec.Y);
    }

    // Конвертация ИЗ вектора физики (Velcro/MonoGame) В System.Numerics
    private Vector2 ToSystem(Microsoft.Xna.Framework.Vector2 physVec)
    {
        return new Vector2(physVec.X, physVec.Y);
    }
}