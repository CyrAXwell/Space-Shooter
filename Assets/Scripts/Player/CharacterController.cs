using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;

    private Vector2 _direction;
    private Rigidbody2D _rb2D;
    private float _currentSpeed;

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }
    
    private void Update() {
        _direction.x = Input.GetAxisRaw("Horizontal");
        _direction.y = Input.GetAxisRaw("Vertical");
        
        _direction.Normalize();

        if(_direction.magnitude > 0f)
        {
            _currentSpeed += Time.deltaTime * acceleration;
            _currentSpeed = _currentSpeed > speed ? speed : _currentSpeed;
        }
        else
        {
            _currentSpeed = 0;
        }
    }

    private void FixedUpdate()
    {
        _rb2D.velocity = _direction * _currentSpeed;
    }

}
