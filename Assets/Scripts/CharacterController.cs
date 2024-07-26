using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [SerializeField] float speed;
    private Vector2 direction;
    private Rigidbody2D rb;
    private Vector2 speedFunction;
    [SerializeField] float acceleration;
    public float currentSpeed = 0f;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update() {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        
        direction.Normalize();
        
        // speedFunction +=  direction * acceleration * Time.deltaTime;

        // if(Mathf.Abs(speedFunction.x) >= speed)
        // {
        //    speedFunction.x = direction.x * speed;
        // }

        // if(Mathf.Abs(speedFunction.y) >= speed)
        // {
        //    speedFunction.y = direction.y * speed;
        // }


        if(direction.magnitude != 0f)
        {
            currentSpeed += Time.deltaTime * acceleration;
            if(currentSpeed >= speed)
            {
                currentSpeed = speed;
            }
        }

        if(direction.magnitude == 0)
        {
            currentSpeed = 0;
        }

        //Debug.Log("speed " + currentSpeed);
        // Vector3 pos = transform.position;
        // pos.y += Input.GetAxis("Vertical") * speed * Time.deltaTime;
        // pos.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        // transform.position = pos;
    }

    void FixedUpdate(){
        
        //rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        // direction.x = Input.GetAxisRaw("Horizontal");
        // direction.y = Input.GetAxisRaw("Vertical");

        // direction.Normalize();
        
        rb.velocity = direction * currentSpeed;
        //rb.velocity = speedFunction;
        //rb.velocity = direction * speed;

        // Vector3 pos = transform.position;
        // pos.y += Input.GetAxis("Vertical") * speed * Time.fixedDeltaTime;
        // pos.x += Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime;
        // transform.position = pos;

        
    }

}
