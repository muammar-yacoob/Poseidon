using UnityEngine;
using PanettoneGames.Poseidon.Core;

public class Tank : MonoBehaviour
{
    [SerializeField] private GameObjectPool projectilePool;
    [SerializeField] private Transform firePoint;
    private float fireTimer = 0.5f;
    private float timer;

    private void Start() => projectilePool.Prewarm();

    void Update()
    {
        if (timer > fireTimer) Shoot();
        timer += Time.deltaTime;
    }
    private void Shoot()
    {
        timer = 0;
        var shot = projectilePool.Get();
        shot.transform.position = firePoint.transform.position;
        shot.transform.rotation = firePoint.transform.rotation;
    }
}
