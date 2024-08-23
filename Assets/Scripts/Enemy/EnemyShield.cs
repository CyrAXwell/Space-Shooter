using System.Collections;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [SerializeField] private float activationTime;
    [SerializeField] private float recoveryTime; 
    [SerializeField] private EnemyShieldStats shield; 
    [SerializeField] private Animator animator;

    private float _timer;
    private bool _shieldIsActive;
   
    private void OnEnable()
    {
        _timer = activationTime;
        _shieldIsActive = false;
        animator.Play("Entry", 0);
        shield.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else if (!_shieldIsActive)
        {
            _shieldIsActive = true;
            animator.SetBool("ShieldActivation", true);
            StartCoroutine(ShieldActivation(activationTime));
        }
    }

    public void OnShieldDestroy()
    {
        animator.SetBool("ShieldActive", false);
        shield.gameObject.SetActive(false);
        _timer = recoveryTime;
        _shieldIsActive = false;
    }

    private IEnumerator ShieldActivation(float time)
    {   
        yield return new WaitForSeconds(time);
        animator.SetBool("ShieldActivation", false);
        animator.SetBool("ShieldActive", true);
        shield.gameObject.SetActive(true);
        shield.Initialize(gameObject.GetComponent<Enemy>().GetWave(), this);
    }
}
