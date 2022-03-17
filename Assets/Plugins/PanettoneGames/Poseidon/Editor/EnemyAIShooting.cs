using UnityEngine;
namespace PanettoneGames.Poseidon.Utilities
{
    public class EnemyAIShooting : PooledShootingBehaviour
    {
        private void Update()
        {
            if (CanFire)
            {
                timer = 0;
                Fire();
            }
            timer += Time.deltaTime;
        }
        private bool CanFire => timer >= fireDelay;
    }
}