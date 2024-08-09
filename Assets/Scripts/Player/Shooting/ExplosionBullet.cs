using UnityEngine;

public class ExplosionBullet : MonoBehaviour
{
    public int damage;
    public int critChance;
    public int critDamage;

    public float speed;
    public float splashRange;
    public float lifeTime;
    public LayerMask whatIsSolid;

    private Vector2 mPrevPos;
    private Vector2 pos;

    public GameObject hitEffect;

    private void Start()
    {
        mPrevPos.x = transform.position.x;
        mPrevPos.y = transform.position.y;
        Invoke("DestroyBullet", lifeTime);
    }

    private void FixedUpdate()
    {
        mPrevPos = transform.position;

        transform.Translate(Vector2.up * speed * Time.deltaTime);

        pos.x = transform.position.x;
        pos.y = transform.position.y;

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

    void DestroyBullet() {
        
        Destroy(gameObject);
    }

    void ExplosionDamage(Vector2 point)
    {
        var hitColliders = Physics2D.OverlapCircleAll(point, splashRange);
        foreach(var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy")) {
                hitCollider.GetComponent<Enemy>().TakeDamage(damage, critChance, critDamage);
            }
            if (hitCollider.CompareTag("EnemyShield")) {
                hitCollider.GetComponent<EnemyShieldStats>().TakeDamage(damage);
            }
            if (hitCollider.CompareTag("Boss")) {
                hitCollider.GetComponent<Boss>().TakeDamage(damage, critChance, critDamage);
            }
        }
        
        DestroyBullet();
    }
}
