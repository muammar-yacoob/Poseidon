using UnityEngine;

public class Bullet : MonoBehaviour, IGameObjectPooled
{
    [SerializeField] [Range(10, 100)] float launchSpeed = 30f;
    [SerializeField] [Range(1, 10)] private float maxLifeTime = 2f;
    [SerializeField] [Tooltip("Remember to turn off particle play on awake")] ParticleSystem FX;
    [SerializeField] [Tooltip("Typically, player's layer")] private LayerMask ignoredLayers;

    private float lifeTime;
    private Renderer rend;

    public GameObjectPool Pool { get; set; }

    private void Awake() => rend = GetComponent<Renderer>();

    private void OnEnable()
    {
        rend.enabled = true;
        lifeTime = 0;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * launchSpeed * Time.deltaTime);
        lifeTime += Time.deltaTime;

        if (lifeTime > maxLifeTime)
            Pool?.ReturnToPool(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((ignoredLayers & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
            return;

        rend.enabled = false;

        if (FX != null)
            FX.Play();
        Pool?.ReturnToPool(this.gameObject, FX.main.duration);
    }
}
