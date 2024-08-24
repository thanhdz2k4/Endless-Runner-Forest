using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    Player player;
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] TextMeshProUGUI coinsText;

    [SerializeField] Image heartEmpty;
    [SerializeField] Image heartFull;
    [SerializeField] Image slideIcon;

    private float distance;
    private float coins;

    private void Start()
    {
        player = GameManager.Instance.player;
        InvokeRepeating("UpdateInfo", 0, .2f);
    }
    private void UpdateInfo()
    {
        slideIcon.enabled = player.slideCoolDownCounter < 0;
        distance = GameManager.Instance.distance;
        coins = GameManager.Instance.coins;
        if (distance > 0)
        {
            distanceText.text = distance.ToString("#,#") + " m";
        }
        if (coins > 0)
        {
            coinsText.text = coins.ToString("#,#");
        }
        heartEmpty.enabled = !player.extraLife;
        heartFull.enabled = player.extraLife;
        
    }
}
