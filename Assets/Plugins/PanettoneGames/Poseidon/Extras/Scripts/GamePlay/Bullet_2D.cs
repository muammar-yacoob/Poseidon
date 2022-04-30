using PanettoneGames.Poseidon.Core;
using UnityEngine;

namespace PanettoneGames.Poseidon.Gameplay
{
    public class Bullet_2D : MonoBehaviour, IGameObjectPooled //implement interface
    {
        [SerializeField] private float LaunchSpeed = 20;
        [SerializeField] [Tooltip("In Seconds")] private float maxLifeTime = 3f;
        [SerializeField] [Tooltip("Typically, player's layer")] private LayerMask layerMask;
        [SerializeField] [Tooltip("Remember to turn off particle play on awake")] ParticleSystem FX;

        private float lifeTime;

        public GameObjectPool Pool { get; set; } //implement interface
        private void OnEnable() => lifeTime = 0;

        void Update()
        {
            transform.Translate(Vector2.up * LaunchSpeed * Time.deltaTime);
            lifeTime += Time.deltaTime;

            if (lifeTime > maxLifeTime)
                Pool?.ReturnToPool(gameObject); //return to pool instead of destroy
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if ((layerMask & 1 << collision.gameObject.layer) != 1 << collision.gameObject.layer)
                return;

            //return to pool instead of destroy
            if (FX == null)
            {
                Pool?.ReturnToPool(gameObject);
            }
            else
            {
                FX.Play();
                Pool?.ReturnToPool(gameObject,FX.main.duration);
            }
        }
    }
}