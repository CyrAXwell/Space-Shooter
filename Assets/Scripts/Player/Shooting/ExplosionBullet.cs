using UnityEngine;

public class ExplosionBullet : MonoBehaviour
{
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask whatIsSolid;
    [SerializeField] private float splashRange;
    [SerializeField] private float lifeTime;

    private int _damage;
    private int _critChance;
    private int _critDamage;

    private void Start()
    {
        Invoke("DestroyBullet", lifeTime);
    }

    public void Initialize(int damage, int critChance = 0,  int critDamage = 0)
    {
        _damage = damage;
        _critChance = critChance;
        _critDamage = critDamage;
    }

    private void FixedUpdate()
    {
        Vector2 mPrevPos = transform.position;

        transform.Translate(Vector2.up * speed * Time.deltaTime);

        Vector2 pos = transform.position;

        RaycastHit2D[] hits = Physics2D.RaycastAll(mPrevPos, (pos - mPrevPos).normalized, (pos - mPrevPos).magnitude, whatIsSolid);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider != null) 
            {
                GameObject effect = Instantiate(hitEffect, hits[i].point, Quaternion.identity);
                Destroy(effect,0.5f);
                ExplosionDamage(hits[i].point);
            }
        }
        Debug.DrawLine(transform.position, mPrevPos);
    }

    private void DestroyBullet() 
    {  
        Destroy(gameObject);
    }

    private void ExplosionDamage(Vector2 point)
    {
        var hitColliders = Physics2D.OverlapCircleAll(point, splashRange);
        foreach(var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy")) {
                hitCollider.GetComponent<Enemy>().TakeDamage(_damage, _critChance, _critDamage);
            }
            if (hitCollider.CompareTag("EnemyShield")) {
                hitCollider.GetComponent<EnemyShieldStats>().TakeDamage(_damage);
            }
            if (hitCollider.CompareTag("Boss")) {
                hitCollider.GetComponent<Boss>().TakeDamage(_damage, _critChance, _critDamage);
            }
        }
        DestroyBullet();
    }
}
