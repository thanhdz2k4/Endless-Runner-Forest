using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ButtonSlide : MonoBehaviour , IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData) => GameManager.Instance.player.SlideButton();
}
