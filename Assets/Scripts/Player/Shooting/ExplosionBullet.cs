using UnityEngine;

public class ExplosionBullet : MonoBehaviour
{
    [SerializeField] private ExplosionEffect hitEffect;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask whatIsSolid;
    [SerializeField] private float splashRange;
    [SerializeField] private float lifeTime;

    private ObjectPoolManager _objectPool;
    private int _damage;
    private int _critChance;
    private int _critDamage;

    public void Initialize(ObjectPoolManager objectPool, int damage, int critChance = 0,  int critDamage = 0)
    {
        _objectPool = objectPool;
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
                var hitColliders = Physics2D.OverlapCircleAll(hits[i].point, splashRange);
                foreach(var hitCollider in hitColliders)
                {
                    switch (hitCollider.tag)
                    {
                        case "Enemy" : hitCollider.GetComponent<Enemy>().TakeDamage(_damage, _critChance, _critDamage); break;
                        case "EnemyShield" : hitCollider.GetComponent<EnemyShieldStats>().TakeDamage(_damage); break;
                        case "Boss" : hitCollider.GetComponent<Boss>().TakeDamage(_damage, _critChance, _critDamage); break;
                    }
                }
                ExplosionEffect effect = _objectPool.GetObject(hitEffect).GetComponent<ExplosionEffect>();
                effect.gameObject.name = hitEffect.name.ToString();
                effect.transform.position = hits[i].point;
                _objectPool.ReleaseObject(effect, 0.5f);
                _objectPool.ReleaseObject(this);
                break;
            }
        }
        Debug.DrawLine(transform.position, mPrevPos);
    }
}
