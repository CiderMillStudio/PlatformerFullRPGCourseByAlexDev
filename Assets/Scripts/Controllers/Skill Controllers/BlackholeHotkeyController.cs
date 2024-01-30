using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackholeHotkeyController : MonoBehaviour
{

    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private BlackholeSkillController blackhole;

    public void SetupHotKey(KeyCode _myNewHotKey, Transform _myEnemy, 
        BlackholeSkillController _myBlackhole)
    {
        myHotKey = _myNewHotKey;
        sr = GetComponent<SpriteRenderer>();

        myText = GetComponentInChildren<TextMeshProUGUI>();
        myText.text = myHotKey.ToString();

        myEnemy = _myEnemy;
        blackhole = _myBlackhole;

    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            blackhole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }


}
