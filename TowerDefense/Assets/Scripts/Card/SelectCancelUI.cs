using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectCancelUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    System.Action<bool> OnCardEnter;

    public void InjectDropEvents(System.Action<bool> OnCardEnter)
    {
        this.OnCardEnter = OnCardEnter;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnCardEnter?.Invoke(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnCardEnter?.Invoke(false);
    }
}
