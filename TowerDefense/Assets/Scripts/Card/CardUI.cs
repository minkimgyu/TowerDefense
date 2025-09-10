using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, ISpawnableUI
{
    [SerializeField] RectTransform _rectTransform;
    [SerializeField] Image _cardBackground;
    [SerializeField] Image _cardIcon;
    [SerializeField] Image _cardCountBackground;
    [SerializeField] TMP_Text _cardCountTxt;

    System.Action<CardData> OnDragStart;
    System.Action OnDragEnd;

    Vector2 _originPos;
    CardData _cardData;

    public GameObject GetObject() { return gameObject; }

    int _cardCount = 0;
    public int CardCount
    {
        get => _cardCount;
        set
        {
            _cardCount = value;
            _cardCountTxt.text = _cardCount.ToString();
        }
    }

    public void Initialize(Sprite cardIcon, CardData cardData)
    {
        _cardCount = 0;
        _cardIcon.sprite = cardIcon;
        _cardData = cardData;
    }

    public void InjectDragEvents(System.Action<CardData> OnDragStart, System.Action OnDragEnd)
    {
        this.OnDragStart = OnDragStart;
        this.OnDragEnd = OnDragEnd;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnDragStart?.Invoke(_cardData);
        _originPos = _rectTransform.position;
    }

    void ChangePos(Vector2 pos)
    {
        _rectTransform.position = pos;
    }

    void ChangeCardAlpha(float alpha)
    {
        _cardBackground.color = new Color(_cardBackground.color.r, _cardBackground.color.g, _cardBackground.color.b, alpha);
        _cardIcon.color = new Color(_cardIcon.color.r, _cardIcon.color.g, _cardIcon.color.b, alpha);
        _cardCountTxt.color = new Color(_cardCountTxt.color.r, _cardCountTxt.color.g, _cardCountTxt.color.b, alpha);
        _cardCountBackground.color = new Color(_cardCountBackground.color.r, _cardCountBackground.color.g, _cardCountBackground.color.b, alpha);
    }

    public void OnDrag(PointerEventData eventData)
    {
        ChangePos(eventData.position);
        ChangeCardAlpha(0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnDragEnd?.Invoke();
        ChangePos(_originPos);
        ChangeCardAlpha(1);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerClick);
    }
}
