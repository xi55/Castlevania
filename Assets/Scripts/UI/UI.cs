using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
public class UI : MonoBehaviour, ISaveManager
{
    [Header("End Screen")]
    [SerializeField] private UI_Fade fadeScreen;
    [SerializeField] private GameObject diedText;
    [SerializeField] private GameObject restartButton;
    [Space]
    [SerializeField] private GameObject characterUI;
    public GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;


    public UI_ToolTip itemTip;
    public UI_StatToolTip statTip;
    public UI_CraftWindow craftWindow;
    public UI_SkillToolTip skillTip;

    [SerializeField] private UI_VolumeSlider[] volumeSettings; 
    // Start is called before the first frame update
    private void Awake()
    {

        SwitchTo(skillTreeUI);
        SwitchTo(characterUI);
        fadeScreen.gameObject.SetActive(true);
    }
    void Start()
    {
        SwitchTo(null);
        SwitchTo(inGameUI);
        //tip = GetComponentInChildren<UI_ToolTip>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.U))
            SwitchWithKey(characterUI);
        if(Input.GetKeyUp(KeyCode.K))
            SwitchWithKey(skillTreeUI);
        if(Input.GetKeyUp(KeyCode.N))
            SwitchWithKey(craftUI);
        if(Input.GetKeyUp(KeyCode.O))
            SwitchWithKey(optionsUI);
    }

    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_Fade>() != null;
            if(!fadeScreen)
                transform.GetChild(i).gameObject.SetActive(false);
        }
        if (_menu != null)
            _menu.gameObject.SetActive(true);

        if(GameManager.instance != null)
        {
            if(_menu == inGameUI)
                GameManager.instance.PauseGame(false);
            else
                GameManager.instance.PauseGame(true);
        }

    }

    public void SwitchWithKey(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        SwitchTo(_menu);
    }

   
    private void CheckForInGameUI()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_Fade>() == null)
                return;
        }
        SwitchTo(inGameUI);
    }

    public void SwitchOnEndScreen()
    {
        
        //SwitchTo(null);
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutione());
    }
    IEnumerator EndScreenCorutione()
    {
        yield return new WaitForSeconds(1);
        diedText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
    }
    
    public void RestartGameButton() => GameManager.instance.RestartScene();

    public void LoadData(GameData _data)
    {
        foreach(KeyValuePair<string, float> pair in _data.volumeSetting)
        {
            foreach (UI_VolumeSlider item in volumeSettings)
            {
                if(item.parametr == pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSetting.Clear();

        foreach(UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSetting.Add(item.parametr, item.slider.value);
        }
    }
}
