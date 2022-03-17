using UnityEngine;
using UnityEngine.InputSystem;
namespace PanettoneGames.Poseidon.Utilities
{
    public class PlayerShooting : PooledShootingBehaviour
    {
        [Tooltip("Reference to the Fire Input Action")]
        [SerializeField] private InputActionReference fireButton;

        private void OnEnable()
        {
            if (fireButton == null)
            {
                Debug.LogError($"No fire points assigned to {name}");
                return;
            }
            fireButton.action.performed += ctx => Fire();
        }

        private void OnDisable() => fireButton.action.performed -= ctx => Fire();

        private void Update()
        {
            if (fireButton == null) return;

            if (CanFire)
            {
                timer = 0;
                Fire();
            }
            timer += Time.deltaTime;
        }
        private bool CanFire =>
                fireButton.action.IsPressed() && timer >= fireDelay;
    }
}