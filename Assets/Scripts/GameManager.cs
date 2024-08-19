using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public UI_Main ui;
    public Player player;
    [Header("Color info")]
    
    public Color platformColor;
    [Header("Score info")]
    public float distance;
    public int coins;
    public float score;
    private void Awake()
    {
        Instance = this;
        Time.timeScale = .8f;
       // LoadColor();
    }
    public void SaveColor(float r, float g, float b)
    {
        PlayerPrefs.SetFloat("ColorR", r);
        PlayerPrefs.SetFloat("ColorG", g);
        PlayerPrefs.SetFloat("ColorB", b);

    }
    private void LoadColor()
    {
       
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();

        Color newColor = new Color(PlayerPrefs.GetFloat("ColorR"),
                                   PlayerPrefs.GetFloat("ColorG"),
                                   PlayerPrefs.GetFloat("ColorB"),
                                   PlayerPrefs.GetFloat("ColorA", 1));
        spriteRenderer.color = newColor;
    }
    private void Update()
    {
        if (player.transform.position.x > distance)
        {
            distance = player.transform.position.x;
        }
    }
    public void UnlockPlayer() => player.playerUnlooker = true;
    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
       
    public void SaveInfo()
    {
        int savedCoins = PlayerPrefs.GetInt("TotalCoins");
        PlayerPrefs.SetInt("TotalCoins",savedCoins + coins);

        score = distance * coins;
        PlayerPrefs.SetFloat("LastScore", score);
        if (PlayerPrefs.GetFloat("HighScore") < score)
        {
            PlayerPrefs.SetFloat("HighScore",score);
        }
    }
    public void GameEnded()
    {
        SaveInfo();
        ui.OpenEndGameUI();
    }
}
