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
        //EntityManager.AddComponent<Translation>(entity);
        EntityManager.SetComponentData(entity, new Translation() { Value = new float3() });

        //EntityManager.AddComponent<Rotation>(entity);
        EntityManager.SetComponentData(entity, new Rotation() { Value = new quaternion() });
    }
    protected override void OnUpdate()
    {
        // Assign values to local variables captured in your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        float deltaTime = Time.DeltaTime;

        // This declares a new kind of job, which is a unit of work to do.
        // The job is declared as an Entities.ForEach with the target components as parameters,
        // meaning it will process all entities in the world that have both
        // Translation and Rotation components. Change it to process the component
        // types you want.



        Entities
            .ForEach((Entity entity, ref Translation translation, in Rotation rotation, in WASD wasd) =>
            {
                // Implement the work to perform for each entity here.
                // You should only access data that is local or that is a
                // field on this job. Note that the 'rotation' parameter is
                // marked as 'in', which means it cannot be modified,
                // but allows this job to run in parallel with other jobs
                // that want to read Rotation component data.
                // For example,
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



