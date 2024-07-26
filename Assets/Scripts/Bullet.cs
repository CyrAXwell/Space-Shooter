using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public GameObject hitEffect;

    public int damage;
    public int critChance;
    public int critDamage;

    public float speed;
    public float lifeTime;
    public LayerMask whatIsSolid;

    private Vector2 mPrevPos;
    private Vector2 pos;

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
            if (hits[i].collider != null) {
                if (hits[i].collider.CompareTag("Enemy")) {
                    hits[i].collider.GetComponent<Enemy>().TakeDamage(damage, critChance, critDamage);
                }
                if (hits[i].collider.CompareTag("Player")) {
                    hits[i].collider.GetComponent<Player>().TakeDamage(damage);
                    
                }
                if (hits[i].collider.CompareTag("Shield")) {
                    hits[i].collider.GetComponent<Shield>().TakeDamage(damage);
                    //Debug.Log(hits[i].collider.name);
                }
                if (hits[i].collider.CompareTag("EnemyShield")) {
                    hits[i].collider.GetComponent<EnemyShieldStats>().TakeDamage(damage);
                }
                if (hits[i].collider.CompareTag("Boss")) {
                    hits[i].collider.GetComponent<Boss>().TakeDamage(damage, critChance, critDamage);
                }
                GameObject effect = Instantiate(hitEffect, hits[i].point, Quaternion.identity);
                Destroy(effect,1f);
                DestroyBullet();
                break;
            }

        }

        Debug.DrawLine(transform.position, mPrevPos);
 
    }

    void DestroyBullet() {
        
        Destroy(gameObject);
    }

}
