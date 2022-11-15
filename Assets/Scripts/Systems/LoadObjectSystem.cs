using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.AddressableAssets;
using UnityEngine;
using Addressables.Data;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

public partial class LoadObjectSystem : SystemBase
{

    private Dictionary<int, AsyncOperationHandle<GameObject>> handleMap = new Dictionary<int, AsyncOperationHandle<GameObject>>();
    private NativeParallelHashMap<int, Entity> resultMap;
    private GameObjectConversionSettings settings;

    protected override void OnCreate()
    {
        var blobAssetStore = new BlobAssetStore();
        settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        settings.ConversionFlags = GameObjectConversionUtility.ConversionFlags.AssignName;

        resultMap = new NativeParallelHashMap<int, Entity>(10, Allocator.Persistent);
    }

    protected override void OnDestroy()
    {
        settings.BlobAssetStore.Dispose();
        resultMap.Dispose();
    }

    protected override void OnUpdate()
    {



        Entities.WithNone<VisualRepresentationLable>().ForEach((Entity entity, in ReferenceToAssetData reference) =>
        {
            if (resultMap.TryGetValue((int)reference.Asset, out var srcEntity))
            {               
                var newEntity = EntityManager.Instantiate(srcEntity);               
                EntityManager.SetComponentData(newEntity, new Parent { Value = entity });

                EntityManager.AddComponent<VisualRepresentationLable>(entity);

            }
            else if (!handleMap.ContainsKey((int)reference.Asset))
            {
                var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>(reference.Path.ToString());

                handleMap.Add((int)reference.Asset, handle);

                var assetName = reference.Asset;              
                
                handle.Completed += operationHandle =>
                {
                    if (operationHandle.IsDone && operationHandle.Result != null)
                    {
                        var go = operationHandle.Result;
                        go.name = assetName.ToString();

                        //var newSrcEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(operationHandle.Result, settings);

                        //EntityManager.AddComponent<LocalToParent>(newSrcEntity);
                        //EntityManager.AddComponent<Parent>(newSrcEntity);

                        //resultMap.Add((int)assetName, newSrcEntity);
                    }
                    else
                    {
                        handleMap.Remove((int)assetName);
                    }
                };
            }

        }).WithStructuralChanges().WithoutBurst().Run();
    }
}




public struct VisualRepresentationLable : IComponentData
{

}
public struct DirectionData : IComponentData
{
    public float3 Direction;
}
//Translation translation = default;
//if (EntityManager.HasComponent<Translation>(entity))
//{
//    translation = EntityManager.GetComponentData<Translation>(entity);
//}
