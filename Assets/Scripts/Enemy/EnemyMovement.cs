using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float xDelta;
    [SerializeField] private float yDelta;
    [SerializeField] private float leftBorder;
    [SerializeField] private float rightBorder;
    [SerializeField] private float topBorder;
    [SerializeField] private float bottomBorder;

    private Rigidbody2D _rb;
    private Vector2 _newPoint;
    private float _minX;
    private float _maxX;
    private float _minY;
    private float _maxY;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        Vector3 spawnPoint = _rb.transform.position;

        float rightCor = spawnPoint.x + xDelta > rightBorder ? spawnPoint.x + xDelta - rightBorder : 0f;
        float leftCor = spawnPoint.x - xDelta < leftBorder ? spawnPoint.x - xDelta - leftBorder : 0f;
        float topCor = spawnPoint.y + yDelta > topBorder ? spawnPoint.y + yDelta - topBorder : 0f;
        float bottomCor = spawnPoint.y - yDelta < bottomBorder ? spawnPoint.y - yDelta - bottomBorder : 0f;

        _minX = spawnPoint.x - xDelta - leftCor;
        _maxX = spawnPoint.x + xDelta - rightCor;
        _minY = spawnPoint.y - yDelta - bottomCor;
        _maxY = spawnPoint.y + yDelta - topCor;

        _newPoint = new Vector2(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY));
    }

    private void Update()
    {
        _rb.transform.position = Vector2.MoveTowards(_rb.transform.position, _newPoint, speed * Time.deltaTime);

        if(Vector2.Distance(_rb.transform.position,_newPoint) <= 0.05) 
            _newPoint = new Vector2(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY));
    }
}
