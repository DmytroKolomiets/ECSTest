using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static UnityEngine.EventSystems.EventTrigger;

#if UNITY_EDITOR
public class AssetConfig : MonoBehaviour
{
    [SerializeField] private ConfigObject[] prefabs;
    private class Baker : Baker<AssetConfig>
    {
        public override void Bake(AssetConfig authoring)
        {
            var buffer = AddBuffer<PrefabSpawnerBufferElement>();
            foreach (var item in authoring.prefabs)
            {              
                buffer.Add(new PrefabSpawnerBufferElement { Prefab = new EntityPrefabReference(item.GO), Name = item.Name, entity = GetEntity(item.GO) });
            }
        }
    }   
}
#endif
[System.Serializable]
public class ConfigObject
{
    public GameObject GO;
    public AssetObject Name;
}
public struct PrefabSpawnerBufferElement : IBufferElementData
{
    public EntityPrefabReference Prefab;
    public AssetObject Name;
    public Entity entity;
}
public enum AssetObject
{
    FirstSpell,
    SecondSpell
}

