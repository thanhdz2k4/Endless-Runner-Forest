using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public struct ColorToSell
{
    public Color color;
    public int price;

}
public enum ColorType
{ 
    playerColor,
    platformColor

}


public class UI_Shop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI notifyText;
    [Space] 

    [Header("Platform colors")]
    [SerializeField] ColorToSell[] platformColors;
    [SerializeField] GameObject platformColorButton;
    [SerializeField] Transform platformColorParent;
    [SerializeField] Image platformDisplay;

    [Header("Player colors")]
    [SerializeField] ColorToSell[] playerColors;
    [SerializeField] GameObject playerColorButton;
    [SerializeField] Transform playerColorParent;
    [SerializeField] Image playerDisplay;
    void Start()
    {
        coinsText.text = PlayerPrefs.GetInt("TotalCoins").ToString("#,#");
        for (int i = 0; i < platformColors.Length; i++)
        {
            Color color = platformColors[i].color;
            int price = platformColors[i].price;
            GameObject newButton = Instantiate(platformColorButton, platformColorParent);

            newButton.transform.GetChild(0).GetComponent<Image>().color = color;
            newButton.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = price.ToString("#,#");
            newButton.GetComponent<Button>().onClick.AddListener(() => PurchaseColor(color,price,ColorType.platformColor));
        }

        for (int i = 0; i < playerColors.Length; i++)
        {
            Color color = playerColors[i].color;
            int price = playerColors[i].price;
            GameObject newButton = Instantiate(playerColorButton, playerColorParent);

            newButton.transform.GetChild(0).GetComponent<Image>().color = color;
            newButton.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = price.ToString("#,#");
            newButton.GetComponent<Button>().onClick.AddListener(() => PurchaseColor(color, price, ColorType.playerColor));
        }
    }
    public void PurchaseColor(Color color, int price, ColorType colorType)
    {
        AudioManager.Instance.PlaySFX(4);
        if (EnoughMoney(price))
        {
            if (colorType == ColorType.platformColor)
            {
                GameManager.Instance.platformColor = color;
                platformDisplay.color = color;
            }
            else if (colorType == ColorType.playerColor)
            {
                GameManager.Instance.player.GetComponent<SpriteRenderer>().color = color;
                GameManager.Instance.SaveColor(color.r, color.g, color.b);
                playerDisplay.color = color;
            }

            StartCoroutine(Notify("Purchased successful", 1));
            
        }
       else
        {
            StartCoroutine(Notify("Not enough money!", 1));
        }

    }
    private bool EnoughMoney(int price)
    {
        int myCoins = PlayerPrefs.GetInt("TotalCoins");
        if (myCoins > price)
        {
            int newAmountOfCoins = myCoins - price;
            PlayerPrefs.GetInt("TotalCoins",newAmountOfCoins);
            coinsText.text = PlayerPrefs.GetInt("TotalCoins").ToString("#,#");
            return true;

        }
        return false;
    }

    IEnumerator Notify(string text, float seconds)
    {
        notifyText.text = text;
        
        yield return new WaitForSeconds(seconds);

        notifyText.text = "Click to buy";
    }
}
