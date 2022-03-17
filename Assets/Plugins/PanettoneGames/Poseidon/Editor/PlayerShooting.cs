using UnityEngine;
using UnityEngine.InputSystem;
namespace PanettoneGames.Poseidon.Utilities
{
    public class PlayerShooting : PooledShootingBehaviour
    {
        [Tooltip("Reference to the Fire Input Action")]
        [SerializeField] private InputActionReference fireButton;
        private bool isHeldDown;

        private void OnEnable()
        {
            if (fireButton == null)
            {
                Debug.LogError($"No Fire action was assigned to {name}");
                return;
            }
            fireButton.action.Enable();
            //fireButton.action.performed += ctx => base.Fire();

            fireButton.action.started += (ctx) => isHeldDown = true;
            fireButton.action.canceled += (ctx) => isHeldDown = false;
        }

        private void OnDisable() => fireButton.action.Disable();

        public void SetFireButton(InputActionReference fireButton) => this.fireButton = fireButton;
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
            isHeldDown &&
            timer >= fireDelay;
    }
}