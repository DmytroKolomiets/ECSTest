using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetConfig : MonoBehaviour, IConvertGameObjectToEntity
{
    [SerializeField] private ConfigObject[] assetPaths;
    


    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        //for (int i = 0; i < assetPaths.Length; i++)
        //{
        //    var e = dstManager.CreateEntity(typeof(ReferenceToAssetData));
        //    dstManager.SetComponentData(e, new ReferenceToAssetData() { Path = assetPaths[i].reference.AssetGUID, Asset = assetPaths[i].Name });
        //}
    }
}
[System.Serializable]
public class ConfigObject
{
    public AssetReference reference;
    public AssetObject Name;
}
public enum AssetObject
{
    FirstSpell,
    SecondSpell
}
