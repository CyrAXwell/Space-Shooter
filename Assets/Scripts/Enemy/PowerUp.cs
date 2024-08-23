using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float multiplier;
    [SerializeField] private GameObject powerUpArea;
    [SerializeField] private float ActivationTime;

    private void OnEnable()
    {
        StartCoroutine(ActivatePowerUp(ActivationTime));
    }

    public float GetPowerMultiplier() => multiplier;

    private IEnumerator ActivatePowerUp(float interval)
    {
        yield return new WaitForSeconds(interval);
        powerUpArea.SetActive(true);
        Debug.Log(powerUpArea.GetComponent<Collider2D>().CompareTag("PowerUp"));
        GetComponent<Enemy>().ActivatePowerUp(powerUpArea.GetComponent<Collider2D>());//transform.GetChild(4).GetComponent<Collider2D>());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        powerUpArea.SetActive(false);
    }
}
