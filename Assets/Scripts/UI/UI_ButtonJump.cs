using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ButtonJump : MonoBehaviour , IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData) => GameManager.Instance.player.JumpButton();
}
