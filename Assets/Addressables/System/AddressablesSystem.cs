using System.Collections.Generic;
using Addressables.Data;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Addressables.System
{
    public partial class AddressablesSystem : SystemBase
    {
        private Dictionary<AssetName, AsyncOperationHandle<GameObject>> _handleMap = new Dictionary<AssetName, AsyncOperationHandle<GameObject>>();
        private NativeParallelHashMap<int, Entity> _resultMap;

        private GameObjectConversionSettings _settings;
        
        protected override void OnCreate()
        {
            var blobAssetStore = new BlobAssetStore();
            _settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
            _settings.ConversionFlags = GameObjectConversionUtility.ConversionFlags.AssignName;
            
            _resultMap = new NativeParallelHashMap<int, Entity>(10, Allocator.Persistent);
        }

        protected override void OnDestroy()
        {
            _settings.BlobAssetStore.Dispose();
            _resultMap.Dispose();
        }

        protected override void OnUpdate()
        {
            Entities.WithNone<AddressablesData>().ForEach((Entity entity, ref AddressablesRequestData addressablesRequest) =>
            {
                var assetName = addressablesRequest.Name;

                if (_resultMap.TryGetValue((int)assetName, out var srcEntity))
                {
                    var newEntity = EntityManager.Instantiate(srcEntity);

                    EntityManager.AddComponent<LocalToParent>(newEntity);
                    EntityManager.AddComponent<Parent>(newEntity);
                    EntityManager.SetComponentData(newEntity, new Parent { Value = entity });

                    EntityManager.AddComponent<AddressablesData>(entity);
                    EntityManager.SetComponentData(entity, new AddressablesData { Entity = newEntity });

                    return;
                }

                if (!_handleMap.ContainsKey(assetName))
                {
                    var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>(assetName.ToString());
                    handle.Completed += operationHandle =>
                    {
                        if (operationHandle.IsDone && operationHandle.Result != null)
                        {
                            var go = operationHandle.Result;
                            go.name = assetName.ToString();
                            var newSrcEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(operationHandle.Result, _settings);
                            _resultMap.Add((int)assetName, newSrcEntity);
                        }
                        else
                        {
                            _handleMap.Remove(assetName);
                        }
                    };

                    _handleMap.Add(assetName, handle);
                }

            }).WithStructuralChanges().WithoutBurst().Run();
        }
    }
}
public enum AssetName
{
    None
}
