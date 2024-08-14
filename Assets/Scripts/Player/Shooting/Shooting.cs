using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform[] firePoints; 
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float cooldown;
    [SerializeField] private Animator animator;
    
    private float _timer;
    private Player _playerStats;
    private bool _isCooldown;
    private bool _canShoot = true;
    private int _skillBonusDamage;
    private AudioManager _audioManager;

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
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            Destroy(bullet, 2f);
            Bullet bulletStats = bullet.GetComponent<Bullet>();
            bulletStats.Initialize(_playerStats.GetActiveATK() + _skillBonusDamage, _playerStats.GetActiveCRITRate(), _playerStats.GetActiveCRITDMG());
        }
    }
}