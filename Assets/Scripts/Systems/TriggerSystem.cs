using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public partial class TriggerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Dependency = new TriggerGravityFactorJob().Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), Dependency);
    }

    struct TriggerGravityFactorJob : ITriggerEventsJob
    {
      

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            Debug.Log(entityA.ToString() + " " + entityB.ToString());

        }
    }
}
