using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class DashSystem : SystemBase
{
    private float time = float.MaxValue;
    protected override void OnUpdate()
    {
        time += UnityEngine.Time.deltaTime;

        Entities.ForEach((ref WASD wasd, ref Translation translation, in Rotation rotation) =>
        {
            if (time < 1)
                return;

            if (wasd.Shift)
            {
                wasd.W = false;
                wasd.A = false;
                wasd.S = false;
                wasd.D = false;
                time = 0;
                float3 dash = math.normalize(wasd.MousePosition - translation.Value) * 3;
                translation.Value.x += dash.x;
                translation.Value.y += dash.y;
            }

        }).WithoutBurst().Run();
    }
}
