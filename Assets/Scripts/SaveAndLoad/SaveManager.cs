using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [SerializeField] private string fileName;
    [SerializeField] private GameData gameData;

    [SerializeField] private bool encryptData;

    private List<ISaveManager> saveManagers;
    private FlieDataHandle dataHandle;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);
    }
    private void Start()
    {
        dataHandle = new FlieDataHandle(Application.persistentDataPath, fileName, encryptData);
        saveManagers = FindAllSaveManagers();
        LoadGame();
    }
    [ContextMenu("Delete Data")]
    public void DeleteData()
    {
        dataHandle = new FlieDataHandle(Application.persistentDataPath, fileName, encryptData);
        dataHandle.DeleteData();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandle.Load();
        if(gameData == null)
            NewGame();

        foreach(ISaveManager manager in saveManagers)
        {
            manager.LoadData(gameData);
        }
        Debug.Log("LoadGame: " + gameData.currency);
    }

    public void SaveGame()
    {
        foreach(ISaveManager manager in saveManagers)
        {
            manager.SaveData(ref gameData);
        }
        dataHandle.Save(gameData);
        Debug.Log("SaveGame: " + gameData.currency);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }

}
