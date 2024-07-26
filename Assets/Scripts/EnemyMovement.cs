using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float speed;
    //private float way = 0f;
    //private Vector2 direction;
    private Rigidbody2D rb;

    private GameObject target;

    private Vector3 spawnPoint;
    [SerializeField] float xDelta;
    [SerializeField] float yDelta;

    [SerializeField] float leftBorder;
    [SerializeField] float rightBorder;
    [SerializeField] float topBorder;
    [SerializeField] float bottomBorder;

    private float leftCor = 0f;
    private float rightCor = 0f;
    private float topCor = 0f;
    private float bottomCor = 0f;

    private Vector2 newPoint;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //GetTarget();
        spawnPoint = rb.transform.position;

        // rightCor = CheckBorder(spawnPoint.x, xDelta, rightBorder);
        // leftCor = CheckBorder(-spawnPoint.x, -yDelta, -leftBorder);
        // topCor = CheckBorder(spawnPoint.y, yDelta, topCor);
        // bottomCor = CheckBorder(spawnPoint.y, yDelta, -bottomCor);

        if(spawnPoint.x + xDelta > rightBorder)
        {
            rightCor = spawnPoint.x + xDelta - rightBorder;
        }
        
        if(spawnPoint.x - xDelta < leftBorder)
        {
            leftCor = spawnPoint.x - xDelta - leftBorder;
        }
        
        if(spawnPoint.y + yDelta > topBorder)
        {
            topCor = spawnPoint.y + yDelta - topBorder;
        }

        if(spawnPoint.y - yDelta < bottomBorder)
        {
            bottomCor = spawnPoint.y - yDelta - bottomBorder;
        }
        //Debug.Log(bottomCor);
        newPoint = new Vector2(Random.Range(spawnPoint.x - xDelta - leftCor, spawnPoint.x + xDelta - rightCor), Random.Range(spawnPoint.y - yDelta - bottomCor, spawnPoint.y + yDelta - topCor));
        //Debug.Log(newPoint);
    }


    void Update()
    {

        if(Vector2.Distance(rb.transform.position,newPoint) <= 0.05) 
        {
            newPoint = new Vector2(Random.Range(spawnPoint.x - xDelta - leftCor , spawnPoint.x + xDelta - rightCor), Random.Range(spawnPoint.y - yDelta - bottomCor, spawnPoint.y + yDelta - topCor));
            //Debug.Log(newPoint);
        }
        
        
        // if(target != null)
        // {
        //     if(Mathf.Abs(rb.transform.position.x - target.transform.position.x) >= 0.2f)
        //     {
        //         if(rb.transform.position.x >= target.transform.position.x)
        //         {
        //             way = -1;
        //         } else
        //         {
        //             way = 1;
        //         }
        //     } else 
        //     {
        //         way = 0;
        //     }
        // }
            
         
        // direction.x = way;
        // direction.y = 0;
        
    }

    void GetTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        rb.transform.position = Vector2.MoveTowards(rb.transform.position, newPoint, speed * Time.fixedDeltaTime);
        // if(target != null)
        // {
        //     rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        // }
    }

    // private float CheckBorder(float spawn, float delta, float border)
    // {
    //     float cor = 0f;
    //     if(spawn + delta > border)
    //     {
    //         cor = spawn + delta - border;
    //     }
    //     return cor;
    // }
}
