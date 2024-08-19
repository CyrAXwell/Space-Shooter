using System.Collections;
using UnityEngine;

public class EnemyBossShooting : MonoBehaviour
{
    [SerializeField] private float timeBetweenAttack; // 2.5
    [SerializeField] private int laserDamage = 10; // 35
    [SerializeField] private int plasmaDamage = 20; // 50
    [SerializeField] private float laserAttackDuration; // 3
    [SerializeField] private float plasmaShotsAmount; // 5
    [SerializeField] private float timeBetweenPlasmaShots; // 0.2
    [SerializeField] private LaserShooting[] laserM;
    [SerializeField] private LaserShooting[] laserL;
    [SerializeField] private LaserShooting[] laserR;
    [SerializeField] private PlasmaShooting[] plasmaM;
    [SerializeField] private PlasmaShooting[] plasmaL1;
    [SerializeField] private PlasmaShooting[] plasmaL2;
    [SerializeField] private PlasmaShooting[] plasmaR1;
    [SerializeField] private PlasmaShooting[] plasmaR2;

    private bool _isCooldown = true;
    private float _timer;
    private int _attackID;
    private ObjectPoolManager _objectPool;

    public void Initialize(ObjectPoolManager objectPool)
    {
        _objectPool = objectPool;
        InitializePlasmaGun(plasmaM);
        InitializePlasmaGun(plasmaL1);
        InitializePlasmaGun(plasmaL2);
        InitializePlasmaGun(plasmaR1);
        InitializePlasmaGun(plasmaR2);
    }

    private void InitializePlasmaGun(PlasmaShooting[] plasmaGuns)
    {
        foreach (var plasmaGun in plasmaGuns)
            plasmaGun.Initialize(_objectPool);
    }

    private void Start()
    {
        _timer = timeBetweenAttack;
    }

    public int GetLaserDamage() => laserDamage;
    public int GetPlasmaDamage() => plasmaDamage;

    private void Update()
    {
        if(_isCooldown)
        {
            _timer -= Time.deltaTime;
            if ( _timer <= 0 )
            {
                _timer = timeBetweenAttack;
                _isCooldown = false;
                Attack();
            }
        }
    }

    private void Attack()
    {
        _attackID = Random.Range(1, 13);

        switch(_attackID)
        {
            case 1:
                FullLaserAttack();
                PlasmaAttackM();
                break;
            case 2:
                FullLaserAttack();
                break;
            case 3:
                SideLaserAttack();
                PlasmaAttackM();
                break;
            case 4:
                FullLaserAttack();
                PlasmaAttackM();
                Side1PlasmaAttack();
                break;
            case 5:
                FullLaserAttack();
                Side1PlasmaAttack();
                break;
            case 6:
                SideLaserAttack();
                Side1PlasmaAttack();
                PlasmaAttackM();
                break;
            case 7:
                SidePlasmaAttack();
                break;
            case 8:
                SidePlasmaAttack();
                MidLaserAttack();
                break;
            case 9:
                FullPlasmaAttack();
                break;
            case 10:
                Side2PlasmaAttack();
                PlasmaAttackM();
                MidLaserAttack();
                break;
            case 11:
                Side2PlasmaAttack();
                MidLaserAttack();
                break;
            case 12:
                Side2PlasmaAttack();
                PlasmaAttackM();
                break;
            case 13:
                SidePlasmaAttack();
                SideLaserAttack();
                break;
        } 
    }

    void FullLaserAttack()
    {
        SideLaserAttack();
        MidLaserAttack();
    }

    void SideLaserAttack()
    {
        foreach(LaserShooting laser in laserL)
        {
            laser.GetComponent<LaserShooting>().StartAttack();
            StartCoroutine(EndLaserShooting(laserAttackDuration, laser));
        }

        foreach(LaserShooting laser in laserR)
        {
            laser.GetComponent<LaserShooting>().StartAttack();
            StartCoroutine(EndLaserShooting(laserAttackDuration, laser));
        }
    }

    void MidLaserAttack()
    {
        foreach(LaserShooting laser in laserM)
        {
            laser.GetComponent<LaserShooting>().StartAttack();
            StartCoroutine(EndLaserShooting(laserAttackDuration, laser));
        }
    }

    void FullPlasmaAttack()
    {
        PlasmaAttackM();
        PlasmaAttackL1();
        PlasmaAttackL2();
        PlasmaAttackR1();
        PlasmaAttackR2(); 
    }

    void SidePlasmaAttack()
    {
        PlasmaAttackL1();
        PlasmaAttackL2();
        PlasmaAttackR1();
        PlasmaAttackR2();
    }

    void Side1PlasmaAttack()
    {
        PlasmaAttackL1();
        PlasmaAttackR1();
    }

    void Side2PlasmaAttack()
    {
        PlasmaAttackL2();
        PlasmaAttackR2();
    }

    void PlasmaAttackM()
    {
        foreach(PlasmaShooting plasma in plasmaM)
        {
            plasma.GetComponent<PlasmaShooting>().StartAttack(timeBetweenPlasmaShots);
            StartCoroutine(EndPlasmaShooting(plasmaShotsAmount * timeBetweenPlasmaShots, plasma));
        }
    }

    void PlasmaAttackL1()
    {
        int i = 4;
        foreach(PlasmaShooting plasma in plasmaL1)
        {
            i--;
            StartCoroutine(PlasmaShootWithDelay(0.02f * i ,plasma));
        }
    }

    void PlasmaAttackL2()
    {
        int i = 4;
        foreach(PlasmaShooting plasma in plasmaL2)
        {
            i--;
            StartCoroutine(PlasmaShootWithDelay(0.02f * i ,plasma));
        }
    }

    void PlasmaAttackR1()
    {
        int i = 4;
        foreach(PlasmaShooting plasma in plasmaR1)
        {
            i--;
            StartCoroutine(PlasmaShootWithDelay(0.02f * i ,plasma));
        }
    }

    void PlasmaAttackR2()
    {
        int i = 4;
        foreach(PlasmaShooting plasma in plasmaR2)
        {
            i--;
            StartCoroutine(PlasmaShootWithDelay(0.02f * i ,plasma));
        }
    }

    private IEnumerator PlasmaShootWithDelay(float time, PlasmaShooting gun)
    {
        yield return new WaitForSeconds(time);
        gun.StartAttack(timeBetweenPlasmaShots);
        StartCoroutine(EndPlasmaShooting(plasmaShotsAmount * timeBetweenPlasmaShots, gun));
    }

    private IEnumerator EndLaserShooting(float interval, LaserShooting gun)
    {
        yield return new WaitForSeconds(interval);
        gun.StopAttack();
        _isCooldown = true;
    }

    private IEnumerator EndPlasmaShooting(float interval, PlasmaShooting gun)
    {
        yield return new WaitForSeconds(interval);
        gun.StopAttack();
        _isCooldown = true;
    }
    
}
