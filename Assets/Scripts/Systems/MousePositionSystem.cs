using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class MousePositionSystem : SystemBase
{
    private Camera camera;
    protected override void OnUpdate()
    {
        if (camera == null)
            camera = Camera.main;

        float3 mousePosition = float3.zero;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            mousePosition = raycastHit.point;
        }


        Entities.ForEach((ref WASD wasd) =>
        {
            wasd.MousePosition = mousePosition;

        }).Schedule();
    }
}
