using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform[] firePoints; 
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float cooldown;
    [SerializeField] private Animator animator;
    [SerializeField] AudioSource shootSound;
    
    private float _timer;
    private Player _playerStats;
    private bool _isCooldown;
    private bool _canShoot = true;
    private int _skillBonusDamage;

    private void Start()
    {
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
        PlayShootSound();
        _isCooldown = true;
        _timer = cooldown;

        animator.SetTrigger("Shoot");
        
        foreach (Transform firePoint in firePoints)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            Destroy(bullet, 2f);
            Bullet bulletStats = bullet.GetComponent<Bullet>();
            bulletStats.Initialize(_playerStats._activeATK + _skillBonusDamage, _playerStats._activeCRITRate, _playerStats._activeCRITDMG);
        }
    }

    void PlayShootSound() => shootSound.Play();
}