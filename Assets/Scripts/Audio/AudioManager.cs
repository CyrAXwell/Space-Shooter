using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("AudioSources")]
    [SerializeField] private AudioSource sFXSource;

    [Header("UI")]
    public AudioClip ButtonClick; // 0.7f
    public AudioClip BackButtonClick; // 1f?
    public AudioClip CharacterSelection; //0.7f
    public AudioClip EnemyHit; // 0.3f
    public AudioClip WaveComplete; // 0.5f
    public AudioClip Lose; // 0.7f
    public AudioClip Win; // 0.7f
    public AudioClip PlayerHit; // 0.7f
    public AudioClip Shoot; // 0.2f
    public AudioClip LevleUp; // 1f?
    public AudioClip UseSkill; // 0.5f
    public AudioClip TooltipButtonClick; // 0.7f

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

    public void MuteSound(bool mute)
    {
        sFXSource.mute = mute;
    }
}
