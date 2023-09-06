using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainMenu";


    public void NewGame()
    {
        SaveManager.instance.DeleteData();
        SceneManager.LoadScene(sceneName);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Debug.Log("exit game");
    }
}
