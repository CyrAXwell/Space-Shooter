using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossShooting : MonoBehaviour
{
    [Header("Laser stats")]
    public int laserDamage = 10;
    [SerializeField] private GameObject[] laserM;
    [SerializeField] private GameObject[] laserL;
    [SerializeField] private GameObject[] laserR;
    //[SerializeField] private float maxDistance;
    [SerializeField] private float rechargeTime;
    [SerializeField] private float shootingTime;
    //[SerializeField] private float timeBetweenTakeDamage;
    //[SerializeField] private LayerMask whatIsSolid;
    
    //private bool isHit = false;
    //private bool canDamage = true;
    private bool rechargeTimeLockedOut = true;
    private float timer = 0f;

    private Enemy enemyStats;
    private int attackID = 0;

    [Header("Plasma stats")]
    public int plasmaDamage = 20;
    [SerializeField] private float numbersOfShots;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private GameObject[] plasmaM;
    [SerializeField] private GameObject[] plasmaL1;
    [SerializeField] private GameObject[] plasmaL2;
    [SerializeField] private GameObject[] plasmaR1;
    [SerializeField] private GameObject[] plasmaR2;

    void Start()
    {
        
        timer = rechargeTime;
    }

    void FixedUpdate()
    {
        if(rechargeTimeLockedOut)
        {
            timer -= Time.fixedDeltaTime;
            if ( timer <= 0 )
            {
                timer = rechargeTime;
                rechargeTimeLockedOut = false;
                Attack();
            }
        }
    }

    void Attack()
    {
        attackID = Random.Range(1, 13);
        //attackID = 7;

        switch(attackID)
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
        foreach(GameObject laser in laserL)
        {
            laser.GetComponent<LaserShooting>().StartAttack();
            StartCoroutine(EndLaserShooting(shootingTime, laser));
        }

        foreach(GameObject laser in laserR)
        {
            laser.GetComponent<LaserShooting>().StartAttack();
            StartCoroutine(EndLaserShooting(shootingTime, laser));
        }
    }

    void MidLaserAttack()
    {
        foreach(GameObject laser in laserM)
        {
            laser.GetComponent<LaserShooting>().StartAttack();
            StartCoroutine(EndLaserShooting(shootingTime, laser));
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
        foreach(GameObject plasma in plasmaM)
        {
            plasma.GetComponent<PlasmaShooting>().StartAttack(timeBetweenShots);
            StartCoroutine(EndPlasmaShooting(numbersOfShots * timeBetweenShots, plasma));
        }
    }

    void PlasmaAttackL1()
    {
        int i = 4;
        foreach(GameObject plasma in plasmaL1)
        {
            i--;
            StartCoroutine(PlasmaShootWithDelay(0.02f*i,plasma));
        }
    }

    void PlasmaAttackL2()
    {
        int i = 4;
        foreach(GameObject plasma in plasmaL2)
        {
            i--;
            StartCoroutine(PlasmaShootWithDelay(0.02f*i,plasma));
        }
    }

    void PlasmaAttackR1()
    {
        int i = 4;
        foreach(GameObject plasma in plasmaR1)
        {
            i--;
            StartCoroutine(PlasmaShootWithDelay(0.02f*i,plasma));
        }
    }

    void PlasmaAttackR2()
    {
        int i = 4;
        foreach(GameObject plasma in plasmaR2)
        {
            i--;
            StartCoroutine(PlasmaShootWithDelay(0.02f*i,plasma));
        }
    }


    private IEnumerator PlasmaShootWithDelay(float time, GameObject gun)
    {
        yield return new WaitForSeconds(time);
        gun.GetComponent<PlasmaShooting>().StartAttack(timeBetweenShots);
        StartCoroutine(EndPlasmaShooting(numbersOfShots * timeBetweenShots, gun));
    }

    private IEnumerator EndLaserShooting(float interval, GameObject gun)
    {
        yield return new WaitForSeconds(interval);
        gun.GetComponent<LaserShooting>().StopAttack();
        //laserL[0].GetComponent<LaserShooting>().StopAttack();
        rechargeTimeLockedOut = true;
    }

    private IEnumerator EndPlasmaShooting(float interval, GameObject gun)
    {
        yield return new WaitForSeconds(interval);
        gun.GetComponent<PlasmaShooting>().StopAttack();
        //laserL[0].GetComponent<LaserShooting>().StopAttack();
        rechargeTimeLockedOut = true;
    }
    
}
