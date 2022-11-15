using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Scenes;
using Unity.Transforms;
using Unity.VisualScripting;



[RequireMatchingQueriesForUpdate]
public partial class LoadPrefabs : SystemBase
{
    private BeginSimulationEntityCommandBufferSystem m_BeginSimECBSystem;
    EntityArchetype archetype;
    Entity systemEntity;
    private Dictionary<AssetObject, Entity> dic = new Dictionary<AssetObject, Entity>();
    protected override void OnCreate()
    {
        archetype = EntityManager.CreateArchetype(typeof(LocalToWorld), typeof(Translation));
        systemEntity = EntityManager.CreateEntity(archetype);      
        m_BeginSimECBSystem = World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var ecb = m_BeginSimECBSystem.CreateCommandBuffer().AsParallelWriter();
        Entities.WithNone<CachedLable>().ForEach((Entity entity, int entityInQueryIndex, DynamicBuffer<PrefabSpawnerBufferElement> prefabs) =>
        {

            for (int i = 0; i < prefabs.Length; i++)
            {
                var item = prefabs[i];
                ecb.AddComponent<LocalToParent>(entityInQueryIndex, item.entity, new LocalToParent());
                ecb.AddComponent<Parent>(entityInQueryIndex, item.entity, new Parent() { Value = systemEntity});
                dic.Add(item.Name, item.entity);
            }

            ecb.AddComponent(entityInQueryIndex, entity, new CachedLable());

        }).WithoutBurst().Run();

        Entities.WithNone<VisualRepresentationLable>().ForEach((Entity entity, int entityInQueryIndex, in ReferenceToAssetData asset) =>
        {

            if (dic.TryGetValue(asset.Asset, out var prefab))
            {
                var e = ecb.Instantiate(entityInQueryIndex,  prefab);
                
                ecb.AddComponent(entityInQueryIndex, entity, new VisualRepresentationLable());
                ecb.SetComponent<Parent>(entityInQueryIndex, e, new Parent() { Value = entity });
            }


        }).WithoutBurst().Run();
        m_BeginSimECBSystem.AddJobHandleForProducer(Dependency);
    }
}
public struct CachedLable : IComponentData, IQueryTypeParameter
{

}