using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class InputSystem : SystemBase
{
    bool isWLast;
    bool isALast;
    bool isSLast;
    bool isDLast;
    protected override void OnUpdate()
    {
        
        var w = false;
        var a = false;
        var s = false;
        var d = false;
        var shift = false;
        var lefMouseButtom = false;
        var rightMouseBottom = false;

        if (Input.GetKeyDown("w"))
        {
            isWLast = true;
            isSLast = false;
        }
        if (Input.GetKeyDown("a"))
        {
            isALast = true;
            isDLast = false;
        }
        if (Input.GetKeyDown("s"))
        {
            isSLast = true;
            isWLast = false;
        }
        if (Input.GetKeyDown("d"))
        {
            isDLast = true;
            isALast = false;
        }

        if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
        {
            shift = true;
        }
        if (Input.GetMouseButton(0))
        {
            lefMouseButtom = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            rightMouseBottom = true;
        }

        if (UnityEngine.Input.GetKey("w"))
        {
            w = true;
            if (isWLast)
                s = false;
        }
        if (UnityEngine.Input.GetKey("a"))
        {
            a = true;
            if (isALast)
                d = false;
        }
        if (UnityEngine.Input.GetKey("s"))
        {
            s = true;
            if (isSLast)
                w = false;
        }
        if (UnityEngine.Input.GetKey("d"))
        {
            d = true;
            if (isDLast)
                a = false;
        }
        Entities.ForEach((ref WASD wasd) =>
        {           
            wasd.W = w;
            wasd.A = a;
            wasd.S = s;
            wasd.D = d;
            wasd.Shift = shift;
            wasd.LeftMouseClick = lefMouseButtom;
            wasd.RightMousCLick = rightMouseBottom;

        }).Schedule();
    }
}

