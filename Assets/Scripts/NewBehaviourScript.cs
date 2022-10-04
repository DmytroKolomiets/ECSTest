using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       // World.DefaultGameObjectInjectionWorld.EntityManager.Creat
        //var entity = EntityManager.CreateEntity(typeof(Translation), typeof(Rotation));
        //Addressables.LoadAssetAsync<GameObject>("Assets/Objects/Projectile.prefab").Completed += OnLoadDone;
        //Addressables.InstantiateAsync("Assets/Objects/Projectile.prefab");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {
        Instantiate(obj.Result);
    }

}
