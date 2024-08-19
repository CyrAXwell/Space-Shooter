using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform[] firePoints; 
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float cooldown;
    [SerializeField] private Animator animator;
    
    private float _timer;
    private Player _playerStats;
    private bool _isCooldown;
    private bool _canShoot = true;
    private int _skillBonusDamage;
    private ObjectPoolManager _objectPool;
    private AudioManager _audioManager;

    public void Initialize(ObjectPoolManager objectPool)
    {
        _objectPool = objectPool;
    }

    private void Start()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        _timer = cooldown;
        _playerStats = GetComponent<Player>();
    }

    public void StopShooting() => _canShoot = false;
    public void ResumeShooting() => _canShoot = true;
    public void UpdateSkillBonusDamage(int damage) => _skillBonusDamage = damage;
    public void UpdateCooldown(float time) => cooldown = time;
    public float GetCooldown() => cooldown;

    private void Update()
    {
        if (_isCooldown)
        {
            _timer -= Time.deltaTime;
        
            if (_timer <= 0)
                _isCooldown = false;
        }

        if(Input.GetButton("Jump") && !_isCooldown && _canShoot && !StateNameController.isPaused)
            Shoot();     
    }

    private void Shoot()
    {
        _audioManager.PlaySFX(_audioManager.Shoot, 0.2f);
        _isCooldown = true;
        _timer = cooldown;

        animator.SetTrigger("Shoot");
        
        foreach (Transform firePoint in firePoints)
        {
            Bullet bullet = _objectPool.GetObject(bulletPrefab).GetComponent<Bullet>();
            bullet.transform.position = firePoint.position;
            bullet.gameObject.name = bulletPrefab.name.ToString();
            _objectPool.ReleaseObject(bullet, 1f);
            bullet.Initialize(_objectPool, _playerStats.GetActiveATK() + _skillBonusDamage, _playerStats.GetActiveCRITRate(), _playerStats.GetActiveCRITDMG());
        }
    }
}