using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Main : MonoBehaviour
{
    private bool gamePaused;
    [SerializeField] private GameObject mainMenu;

    private void Start()
    {
        SwitchMenuTo(mainMenu);
        Time.timeScale = .8f;
    }
    public void SwitchMenuTo(GameObject uiMenu)
    {
        for(int i = 0; i<transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        uiMenu.SetActive(true);
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
}
