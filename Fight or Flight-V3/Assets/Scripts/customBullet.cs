using UnityEngine;

public class customBullet : MonoBehaviour
{
    [Header("--- Bullet Components ---")]
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject visualFX;
    public LayerMask AOE;

    [Header("--- Bullet Stats ---")]
    [Range(0f, 1f)] public float bounce;
    public bool gravity;
    public int explosionDamage;
    public float explosionRange;

    [Header("--- Explotion Stats ---")]
    [SerializeField] int maxHits;
    [SerializeField] float timer;
    public bool contactExplode = true;

    int impacts;
    PhysicMaterial physicMaterial;

    private void Start()
    {
        Setup();
    }
    private void Update()
    {
        // expplotion timing (Based on bounces)
        if(impacts > maxHits) { Explode(); }

        // Explotion Counter (Based on timer)
        timer -= Time.deltaTime;
        if(timer <= 0) { Explode(); }
    }
    private void Setup()
    {
        physicMaterial = new PhysicMaterial();
        physicMaterial.bounciness = bounce;
        physicMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        physicMaterial.bounceCombine = PhysicMaterialCombine.Maximum;

        // Material
        GetComponent<SphereCollider>().material = physicMaterial;

        // Gravity
        rb.useGravity = gravity;
    }
    private void Explode()
    {
        if(visualFX != null) { Instantiate(visualFX, transform.position, Quaternion.identity); }

        // AOE Check
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, AOE);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<enemyAI>().takeDamage(explosionDamage);
        }

        // Delay before it dissapears
        Invoke("Delay", 0.05f);

    }

    private void BulletDeath()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Bullet")) { return; }

        // Counting Bounces
        impacts++;

        // For Bullets that Explode on impact
        if(collision.collider.CompareTag("Enemy") && contactExplode) { Explode(); }
    }
}
