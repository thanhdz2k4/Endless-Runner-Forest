using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_EndGame : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI distance;
    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] TextMeshProUGUI score;
    void Start()
    {
        GameManager manager = GameManager.Instance;

        //Time.timeScale = 0;

        if (manager.distance <= 0)
            return;
        if (manager.coins <= 0)
            return;

        distance.text = "Distance: "+ manager.distance.ToString("#,#") + "m";
        coins.text = "Coins: "+ manager.coins.ToString("#,#");
        score.text = "Score: "+ manager.score.ToString("#,#");
    }

   
}
