using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float multiplier;
    [SerializeField] private GameObject powerUpArea;
    [SerializeField] private float ActivationTime;


    void Start()
    {
        StartCoroutine(ActivatePowerUp(ActivationTime));
    }



    private IEnumerator ActivatePowerUp(float interval)
    {
        yield return new WaitForSeconds(interval);
        powerUpArea.SetActive(true);
        GetComponent<Enemy>().ActivatePowerUp(transform.GetChild(4).GetComponent<Collider2D>());

    }


}
