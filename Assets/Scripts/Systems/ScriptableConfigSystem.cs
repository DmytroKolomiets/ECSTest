using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ScriptableConfig.System
{
    public abstract partial class ScriptableConfigSystem<TConfig> : SystemBase where TConfig : ScriptableObject
    {
        public bool IsReady => Config != null;

        protected TConfig Config { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();

            var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<TConfig>(typeof(TConfig).Name);
            handle.Completed += operationHandle =>
            {
                Config = operationHandle.Result;
                OnConfigReady();
            };
        }

        protected virtual void OnConfigReady(){}
    }
}
