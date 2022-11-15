using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
public class WASD_MonoBeh : MonoBehaviour
{
    private class MyBaker : Baker<WASD_MonoBeh>
    {
        public override void Bake(WASD_MonoBeh authoring)
        {
            AddComponent(new WASD());
        }
    }
}

public struct WASD : IComponentData
{
    public bool W;
    public bool A;
    public bool S;
    public bool D;
    public bool Shift;
    public float3 MousePosition;
    public bool RightMousCLick;
    public bool LeftMouseClick;
}
