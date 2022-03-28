using PanettoneGames.Poseidon;
using UnityEngine;
namespace PanettoneGames.Gameplay
{
    public class TankProjectile : MonoBehaviour, IGameObjectPooled //inheret interface
    {
        private float LaunchSpeed = 20;
        public GameObjectPool Pool { get; set; } //implement interface
        void Update() => transform.Translate(Vector3.forward * LaunchSpeed * Time.deltaTime);
        private void OnCollisionEnter(Collision collision) => Pool?.ReturnToPool(gameObject); //return to pool instead of destroy
    }
}