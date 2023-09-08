using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "SampleScene";
    [SerializeField] private GameObject continueButton;

    [SerializeField] private UI_Fade fadeScreen;
    private void Start()
    {
        if(!SaveManager.instance.HasSaveData())
            continueButton.SetActive(false);

    }

    public void NewGame()
    {
        SaveManager.instance.DeleteData();
        StartCoroutine(LoadScreenWithFadeEffect(1.5f));
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadScreenWithFadeEffect(1.5f));
    }

    public void ExitGame()
    {
        Debug.Log("exit game");
    }

    IEnumerator LoadScreenWithFadeEffect(float _duration)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_duration);
        SceneManager.LoadScene(sceneName);
    }

}
