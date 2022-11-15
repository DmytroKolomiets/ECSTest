using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static UnityEngine.Rendering.DebugUI;

public partial class FirstSpellSystem : SystemBase
{
    EntityArchetype archetype;
    protected override void OnCreate()
    {
        archetype = EntityManager.CreateArchetype(typeof(ReferenceToAssetData), typeof(LocalToWorld), typeof(Translation), typeof(SimpleProjectile), typeof(DirectionData));
    }
    protected override void OnUpdate()
    {
        var a = archetype;

        var ecb = new EntityCommandBuffer(Allocator.TempJob);

        Entities.ForEach((ref WASD wasd, in Translation position) =>
        {           
            if (wasd.RightMousCLick)
            {               
                UnityEngine.Debug.Log("aaaas");
                var enity = ecb.CreateEntity(a);
                ecb.SetComponent<ReferenceToAssetData>(enity, new ReferenceToAssetData() { Asset = AssetObject.FirstSpell, Path = "Assets/Objects/Projectile.prefab" });
                ecb.SetComponent<Translation>(enity, new Translation() { Value = position.Value });
                ecb.SetComponent<DirectionData>(enity, new DirectionData() { Direction = wasd.MousePosition - position.Value });
            }            
        }).Schedule();
        this.Dependency.Complete();
        ecb.Playback(this.EntityManager);
        ecb.Dispose();


        Entities.WithAll<SimpleProjectile>().ForEach((ref Translation position, in DirectionData direction) =>
        {
            var z = position.Value.z;
            position.Value += (math.normalize(direction.Direction) * 0.1f);
            position.Value.z = z;

        }).Schedule();

    }
}
