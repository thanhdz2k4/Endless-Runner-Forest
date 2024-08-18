using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Player player;
    [Header("Color info")]
    public Color playerColor = Color.white;
    public Color platformColor;
    [Header("Score info")]
    public float distance;
    public int coins;
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (player.transform.position.x > distance)
        {
            distance = player.transform.position.x;
        }
    }
    public void UnlockPlayer() => player.playerUnlooker = true;
    public void RestartLevel() => SceneManager.LoadScene(0);
}
