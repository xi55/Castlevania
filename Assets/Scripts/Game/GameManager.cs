using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;

    private Transform player;

    [SerializeField] private CheckPoint[] checkPoints;
    private string closestCheckpointId;

    [Header("lost currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);
        checkPoints = FindObjectsOfType<CheckPoint>();
        
    }

    private void Start()
    {
        player = PlayerManager.instance.player.transform;
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        if (scene != null)
            SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        StartCoroutine(LoadWithDelath(_data));
    }

    private void LoadCheckPoint(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoint)
        {
            foreach (CheckPoint checkPoint in checkPoints)
            {

                if (checkPoint.id == pair.Key && pair.Value == true)
                    checkPoint.ActivateCheckpoint();
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;

        if(FindClosestCheckpoint() != null)
            _data.closestCheckpointId = FindClosestCheckpoint().id;
        _data.checkpoint.Clear();
        foreach(CheckPoint checkPoint in checkPoints)
        {
            _data.checkpoint.Add(checkPoint.id, checkPoint.isActivate);
        }
    }

    private CheckPoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        CheckPoint closestCheckpoint = null;
        foreach (var checkPoint in checkPoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkPoint.transform.position);
            if(distanceToCheckpoint < closestDistance)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkPoint;
            }
        }
        return closestCheckpoint;
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;
        lostCurrencyAmount = _data.lostCurrencyAmount;

        if (lostCurrencyAmount > 0)
        {
            GameObject newLoadCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLoadCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }
        lostCurrencyAmount = 0;
    }

    IEnumerator LoadWithDelath(GameData _data)
    {
        yield return new WaitForSeconds(.1f);
        PlacePlayerAtClosestCheckpoint(_data);
        LoadLostCurrency(_data);
        LoadCheckPoint(_data);
    }

    private void PlacePlayerAtClosestCheckpoint(GameData _data)
    {
        if (_data.closestCheckpointId == null) return;
        closestCheckpointId = _data.closestCheckpointId;
        foreach (CheckPoint checkPoint in checkPoints)
        {
            if (checkPoint.id == closestCheckpointId)
                player.position = checkPoint.transform.position;
        }
    }

    public void PauseGame(bool _pause)
    {
        if (_pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

}
