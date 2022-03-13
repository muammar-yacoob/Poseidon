using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponLauncher : MonoBehaviour
{
    [SerializeField] private GameObjectPool ProjectilePool;

    [SerializeField] [Range(0.05f, 2f)] private float fireRate = 0.25f;
    [SerializeField] Transform[] firePoints;
    [SerializeField] InputActionAsset playerControls;
    [SerializeField] [Tooltip("Action Name from your Actions Asset")] string actionName;
    [SerializeField] bool hideOnLaunch;

    public static event Action<Transform[]> OnFire = delegate { };
    private float fireTimer;
    private InputAction fireKey;
    bool isHeldDown;

    private void Awake() => ProjectilePool.PreWarm();


    private void OnEnable()
    {
        fireKey = playerControls.FindAction(actionName);
        fireKey.Enable();
        //fireKey.performed += Fire;
        fireKey.started += (ctx) => isHeldDown = true;
        fireKey.canceled += (ctx) => isHeldDown = false;
    }


    private void OnDisable() => fireKey.Disable();


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

