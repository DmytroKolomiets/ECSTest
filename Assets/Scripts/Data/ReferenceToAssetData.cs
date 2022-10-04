using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct ReferenceToAssetData : IComponentData
{
    public FixedString128Bytes Path;
    public AssetObject Asset;
}
