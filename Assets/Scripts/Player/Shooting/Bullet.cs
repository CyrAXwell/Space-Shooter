using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private ExplosionEffect hitEffect;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask whatIsSolid;

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
        Vector2 prevPos = transform.position;
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        Vector2 pos = transform.position;

        RaycastHit2D[] hits = Physics2D.RaycastAll(prevPos, (pos - prevPos).normalized, (pos - prevPos).magnitude, whatIsSolid);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider != null) 
            {
                switch (hits[i].collider.tag)
                {
                    case "Enemy" : hits[i].collider.GetComponent<Enemy>().TakeDamage(_damage, _critChance, _critDamage); break;
                    case "EnemyShield" : hits[i].collider.GetComponent<EnemyShieldStats>().TakeDamage(_damage); break;
                    case "Boss" : hits[i].collider.GetComponent<Boss>().TakeDamage(_damage, _critChance, _critDamage); break;
                    case "Player" : hits[i].collider.GetComponent<Player>().TakeDamage(_damage); break;
                    case "Shield" : hits[i].collider.GetComponent<Shield>().TakeDamage(_damage); break;
                }                    

                ExplosionEffect effect = _objectPool.GetObject(hitEffect).GetComponent<ExplosionEffect>();
                effect.gameObject.name = hitEffect.name.ToString();
                effect.transform.position = hits[i].point;
                _objectPool.ReleaseObject(effect, 1f);
                _objectPool.ReleaseObject(this);
                break;
            }
        }
        Debug.DrawLine(transform.position, prevPos);
    }
}
