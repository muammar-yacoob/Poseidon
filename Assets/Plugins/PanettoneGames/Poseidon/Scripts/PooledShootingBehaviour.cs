using System;
using System.Collections.Generic;
using UnityEngine;
namespace PanettoneGames.Poseidon.Utilities
{
    public abstract class PooledShootingBehaviour : MonoBehaviour
    {
        [SerializeField] [Tooltip("Delay between shots in seconds")] protected float fireDelay = 02f;
        [SerializeField] [Tooltip("The pool asset from which projectiles will fire")] private GameObjectPool pool;
        [SerializeField] [Tooltip("Delay between shots in seconds")] protected List<Transform> firePoints;
        [SerializeField] [Tooltip("Optional sound effect on shooting")] protected AudioClip sFX;

        public static Action<AudioClip> OnFire = delegate { };
        protected float timer;
        private void Awake() => pool.Prewarm();
        public void SetPool(GameObjectPool poolAsset)
        {
            this.pool = poolAsset;
        }
        protected void Fire()
        {
            if (firePoints.Count < 1)
            {
                Debug.LogError($"No fire points assigned to {name}");
                return;
            }
            for (int i = 0; i < firePoints.Count; i++)
            {
                var bullet = pool.Get();
                bullet.transform.position = firePoints[i].position;
                bullet.transform.rotation = firePoints[i].rotation;
            }
            OnFire?.Invoke(sFX);
        }
    }
}