using UnityEngine;
using PanettoneGames;

namespace PanettoneGames
{
    public class Projectile : MonoBehaviour, IGameObjectPooled //implement interface
    {
        private float LaunchSpeed = 20;
        private float maxLifeTime = 2f;
        private float lifeTime;

        public GameObjectPool Pool { get; set; } //implement interface
        private void OnEnable() => lifeTime = 0;

        void Update()
        {
            transform.Translate(Vector3.forward * LaunchSpeed * Time.deltaTime);
            lifeTime += Time.deltaTime;

            if (lifeTime > maxLifeTime)
                Pool.ReturnToPool(this.gameObject); //return to pool instead of destroy
        }

        private void OnCollisionEnter(Collision collision)
        {
            Pool?.ReturnToPool(this.gameObject);//return to pool instead of destroy
        }
    }
}