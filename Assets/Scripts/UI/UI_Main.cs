using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UI_Main : MonoBehaviour
{
    private bool gamePaused;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject endGame;
    [Space]

    [SerializeField] private TextMeshProUGUI lastScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI coinsText;


    private void Start()
    {
        SwitchMenuTo(mainMenu);
      

        lastScoreText.text ="Last score: "+ PlayerPrefs.GetFloat("LastScore").ToString("#,#");
        highScoreText.text ="High score: "+ PlayerPrefs.GetFloat("HighScore").ToString("#,#");
    }
    public void SwitchMenuTo(GameObject uiMenu)
    {
        for(int i = 0; i<transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        uiMenu.SetActive(true);

        coinsText.text = PlayerPrefs.GetInt("TotalCoins").ToString("#,#");
    }
    public void StartGameButton() => GameManager.Instance.UnlockPlayer();
    public void PauseGameButton()
    {
        if(gamePaused)
        {
            Time.timeScale = 1.0f;
            gamePaused = false;
        }
        else
        {
            Time.timeScale = 0;
            gamePaused = true;
        }
    }
    public void RestartGameButton() => GameManager.Instance.RestartLevel();

    public void OpenEndGameUI()
    {
        SwitchMenuTo(endGame);
    }
}
