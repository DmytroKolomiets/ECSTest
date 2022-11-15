using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class MoveSystem : SystemBase
{
    protected override void OnCreate()
    {
        var entity = EntityManager.CreateEntity(typeof(Translation), typeof(Rotation));
        EntityManager.SetComponentData(entity, new Translation() { Value = new float3() });
        EntityManager.SetComponentData(entity, new Rotation() { Value = new quaternion() });

        var entity2 = EntityManager.CreateEntity(typeof(CheckSpace));
        EntityManager.SetName(entity2, "SPACE");
    }
    protected override void OnUpdate()
    {
        float deltaTime = UnityEngine.Time.deltaTime;

        Entities.ForEach((Entity entity, ref Translation translation, in Rotation rotation, in WASD wasd) =>
        {

            float speed = 5;

            var moveDirection = float3.zero;
            if (wasd.W)
                moveDirection.y = 1;
            else if (wasd.S)
                moveDirection.y = -1;

            if (wasd.A)
                moveDirection.x = -1;
            else if (wasd.D)
                moveDirection.x = 1;
            if (wasd.W || wasd.A || wasd.S || wasd.D)
                translation.Value += math.mul(rotation.Value, math.normalize(moveDirection) * speed) * deltaTime;


        }).Schedule();
    }
}
public struct CheckSpace : IComponentData, IEnableableComponent
{
    public bool ThisIsBool;
    public int one;
}


