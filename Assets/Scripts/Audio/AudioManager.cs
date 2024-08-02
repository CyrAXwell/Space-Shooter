using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("AudioSources")]
    [SerializeField] private AudioSource sFXSource;

    [Header("UI")]
    public AudioClip ButtonClick;

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

    public void PlaySFX(AudioClip clip)
    {
        sFXSource.PlayOneShot(clip, 1f);
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
