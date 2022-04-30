using PanettoneGames.Poseidon.Core;
using UnityEngine;

namespace PanettoneGames
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bomb : MonoBehaviour, IGameObjectPooled
    {
        [SerializeField] [Range(10, 100)] float launchSpeed = 30f;
        [SerializeField] [Range(1, 10)] private float maxLifeTime = 5f;
        [SerializeField] [Tooltip("Remember to turn off particle play on awake")] ParticleSystem FX;
        [SerializeField] [Tooltip("Typically, player's layer")] LayerMask layerMask;

        private float lifeTime;
        private Rigidbody rb;
        private Renderer rend;

        public GameObjectPool Pool { get; set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rend = GetComponent<Renderer>();
        }

        private void OnEnable()
        {
            rend.enabled = true;
            lifeTime = 0;
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.forward * launchSpeed, ForceMode.Impulse);
        }
        void Update()
        {
            lifeTime += Time.deltaTime;

            if (lifeTime > maxLifeTime)
                Pool?.ReturnToPool(this.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if ((layerMask & 1 << collision.gameObject.layer) != 1 << collision.gameObject.layer)
                return;

            //return to pool instead of destroy
            if (FX == null)
            {
                Pool.ReturnToPool(gameObject);
            }
            else
            {
                FX.Play();
                Pool.ReturnToPool(gameObject, FX.main.duration);
            }
        }
    }
}