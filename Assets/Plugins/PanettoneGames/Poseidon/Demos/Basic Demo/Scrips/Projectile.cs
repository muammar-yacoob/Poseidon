using UnityEngine;

namespace PanettoneGames.Gameplay
{
    public class Projectile : MonoBehaviour, IGameObjectPooled //implement interface
    {
        [SerializeField] private float LaunchSpeed = 20;
        [SerializeField] [Tooltip("In Seconds")] private float maxLifeTime = 3f;
        [SerializeField] [Tooltip("In Seconds")] private WorldSpace worldSpace = WorldSpace._2D;

        private float lifeTime;
        private Vector3 dir;

        public GameObjectPool Pool { get; set; } //implement interface
        private void OnEnable()
        {
            lifeTime = 0;
            dir = worldSpace == WorldSpace._2D ? Vector3.up : Vector3.forward;
        }

        void Update()
        {
            transform.Translate(dir * LaunchSpeed * Time.deltaTime);
            lifeTime += Time.deltaTime;

            if (lifeTime > maxLifeTime)
                Pool.ReturnToPool(this.gameObject); //return to pool instead of destroy
        }

        private void OnCollisionEnter(Collision collision)
        {
            Pool?.ReturnToPool(this.gameObject);//return to pool instead of destroy
        }
        private enum WorldSpace { _2D, _3D }
    }
}