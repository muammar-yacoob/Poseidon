using PanettoneGames.Poseidon;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponLauncher : MonoBehaviour
{
    [SerializeField] [Tooltip("The GameObjects Pool")] private GameObjectPool ProjectilePool;

    [SerializeField] [Range(0.05f, 2f)] private float fireRate = 0.25f;
    [SerializeField] Transform[] firePoints;

    [SerializeField] private InputActionReference fireButton;
    [SerializeField] bool hideOnLaunch;

    public static event Action<Transform[]> OnFire = delegate { };
    private float fireTimer;
    bool isHeldDown;

    private void OnEnable()
    {
        fireButton.action.Enable();
        fireButton.action.started += (ctx) => isHeldDown = true;
        fireButton.action.canceled += (ctx) => isHeldDown = false;

        ProjectilePool.Prewarm();
    }

    private void OnDisable() => fireButton.action.Disable();

    void Update()
    {
        if (CanFire && isHeldDown)
            Fire();

        if (CanFire)
        {
            for (int i = 0; i < firePoints.Length; i++)
                firePoints[i].gameObject.SetActive(true);
        }

        fireTimer += Time.deltaTime;
    }

    private bool CanFire => fireTimer > fireRate;

    private void Fire()
    {
        fireTimer = 0;


        for (int i = 0; i < firePoints.Length; i++)
        {
            var shot = ProjectilePool.Get();
            //shot.transform.parent = this.transform;
            shot.transform.position = firePoints[i].position;
            shot.transform.rotation = firePoints[i].rotation;
            shot.gameObject.SetActive(true);
            if (hideOnLaunch)
                firePoints[i].gameObject.SetActive(false);
        }

        OnFire(firePoints);
    }


}

