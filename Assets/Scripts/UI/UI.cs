using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    public GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;


    public UI_ToolTip itemTip;
    public UI_StatToolTip statTip;
    public UI_CraftWindow craftWindow;
    public UI_SkillToolTip skillTip;
    // Start is called before the first frame update
    private void Awake()
    {
        SwitchTo(skillTreeUI);
        SwitchTo(characterUI);
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
            transform.GetChild(i).gameObject.SetActive(false);
        }
        if (_menu != null)
            _menu.gameObject.SetActive(true);
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
            if (transform.GetChild(i).gameObject.activeSelf)
                return;
        }
        SwitchTo(inGameUI);
    }
}
