using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("AudioSources")]
    [SerializeField] private AudioSource sFXSource;

    [Header("UI")]
    public AudioClip ButtonClick;
    public AudioClip BackButtonClick;
    public AudioClip CharacterSelection;
    public AudioClip EnemyHit;
    public AudioClip WaveComplete;
    public AudioClip Lose;
    public AudioClip Win;
    public AudioClip PlayerHit;
    public AudioClip Shoot;
    public AudioClip LevleUp;
    public AudioClip UseSkill;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        } 
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        sFXSource.PlayOneShot(clip, volume);
    }

    public void PauseSound()
    {
        sFXSource.mute = true;
    }

    public void ResumeSound()
    {
        sFXSource.mute = false;
    }
}
