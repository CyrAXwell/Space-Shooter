using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject character1;
    [SerializeField] private GameObject character2;
    [SerializeField] private GameObject character3;
    [SerializeField] private UpgradeSO[] upgrades;
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private Sprite[] skillIcons1;
    [SerializeField] private Sprite[] skillIcons2;
    [SerializeField] private Sprite[] upgradesPanel;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private GameObject ControlsTipsPanel;

    private Player _player;
    private AudioManager _audioManager;

    public void InitializePlayer()
    {
        Pause();
        CreateCharacter();
        DisplayControlsTips();
        CheckSoundToggle();

        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public Player GetPlayer()
    {
        return _player;
    }
    
    private void CreatePlayer(GameObject character, int skill1, int skill2, int upgrades ,string name)
    {
        GameObject playerObject = Instantiate(character, new Vector3(0f, -4.5f, 0f), Quaternion.identity);
        _player = playerObject.GetComponent<Player>();

        // GameObject.Find("Skill 1").transform.GetChild(1).gameObject.GetComponent<Image>().sprite = skillIcons1[skill1];
        // GameObject.Find("Skill 2").transform.GetChild(1).gameObject.GetComponent<Image>().sprite = skillIcons2[skill2];
        //GameObject.Find("Upgrades Display Panel").GetComponent<Image>().sprite = upgradesPanel[upgrades];
        characterName.text = name;
    }

    private void CreateCharacter()
    {
        switch(StateNameController.character)
        {
            default:
                CreatePlayer(character1, 0, 0, 0, "Hunter");
                break;
            case "Character 1":
                CreatePlayer(character1, 0, 0, 0, "Hunter");    
                break;
            case "Character 2":
                CreatePlayer(character2, 0, 1, 1, "Guardian"); 
                break;
            case "Character 3":
                CreatePlayer(character3, 1, 2, 2, "Destroer"); 
                break;
        }
    }

    private void DisplayControlsTips()
    {
        ControlsTipsPanel.SetActive(true);
    }

    public void CloseControlsTips()
    {
        ControlsTipsPanel.SetActive(false);
        Time.timeScale = 1f;
        StateNameController.isPaused = false;
        StateNameController.startTimers = true;
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        StateNameController.isPaused = true;
    }

    public void PlaySoundOnButtonClick()
    {
        _audioManager.PlaySFX(_audioManager.ButtonClick);
    }

    public void ChangeSoundState()
    {
        StateNameController.isSoundOff = !StateNameController.isSoundOff;
        AudioListener.volume = StateNameController.isSoundOff ? 0f : 1f;
    }
    
    private void CheckSoundToggle()
    {
        if(StateNameController.isSoundOff)
        {
            soundToggle.isOn = true;
            ChangeSoundState();     
        }
        else
        {
            AudioListener.volume = 1f;
            soundToggle.isOn = false;
        }
    }
}
