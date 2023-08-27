using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHole_Hotkey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform enemy;
    private BlackHole_Controller blackHole;
    public void SetHotKey(KeyCode key, Transform _enemy, BlackHole_Controller _blackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myHotKey = key;
        myText.text = myHotKey.ToString();

        enemy = _enemy; 
        blackHole = _blackHole; 
    }

    private void Update()
    {
        if(Input.GetKeyDown(myHotKey))
        {
            blackHole.AddEnemtToList(enemy);
            myText.color = Color.clear;
            sr.color = Color.clear;
        }    
    }
}
