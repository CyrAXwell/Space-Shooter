using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;

    private Vector2 _direction;
    private Rigidbody2D _rb2D;
    public float currentSpeed = 0f;

    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }
    
    void Update() {
        _direction.x = Input.GetAxisRaw("Horizontal");
        _direction.y = Input.GetAxisRaw("Vertical");
        
        _direction.Normalize();

        if(_direction.magnitude > 0f)
        {
            currentSpeed += Time.deltaTime * acceleration;
            currentSpeed = currentSpeed > speed ? speed : currentSpeed;
        }
        else
        {
            currentSpeed = 0;
        }
    }

    void FixedUpdate()
    {
        _rb2D.velocity = _direction * currentSpeed;
    }

}
