using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    // int는 보유 중인 개수
    Dictionary<CardData.Name, CardUI> _ownedCards;

    const int _maxObtainedCardCount = 4;

    System.Action<CardData> OnCardDragStart;
    System.Action OnCardDragEnd;

    [SerializeField] Transform _cardParent;

    Dictionary<CardData.Name, CardData> _cardDataAssets;
    CardUIFactory _cardUIFactory;

    [SerializeField] SelectCancelUI _dropUI;
    [SerializeField] GameObject _cancelPanel;
    [SerializeField] GameObject _infoPanel;
    [SerializeField] TMPro.TMP_Text _infoTitle;
    [SerializeField] TMPro.TMP_Text _infoDescription;

    bool _canPlant = false;

    public void Initialize(Dictionary<CardData.Name, CardData> cardDataAssets, CardUIFactory cardUIFactory)
    {
        _cardDataAssets = cardDataAssets;
        _cardUIFactory = cardUIFactory;
        _ownedCards = new Dictionary<CardData.Name, CardUI>();

        _dropUI.InjectDropEvents((cardEntered) => { _canPlant = !cardEntered; });
        ChangeOnDragEnd();
    }

    void ChangeOnDragEnd()
    {
        _cancelPanel.SetActive(false);
        _infoPanel.SetActive(false);
    }

    void ChangeOnDragStart(string title, string description)
    {
        _infoPanel.SetActive(true);
        _cancelPanel.SetActive(true);
        _infoTitle.text = title;
        _infoDescription.text = description;
    }

    public void InjectDragEvents(System.Action<CardData> OnCardDragStart, System.Action<bool> OnCardDragEnd)
    {
        this.OnCardDragStart = (data) => { OnCardDragStart?.Invoke(data); ChangeOnDragStart(data.CardName, data.CardDescription); };
        this.OnCardDragEnd = () => { OnCardDragEnd?.Invoke(_canPlant); ChangeOnDragEnd(); };
    }

    CardData.Name GetRandomCardName()
    {
        CardData.Name[] enums = (CardData.Name[])System.Enum.GetValues(typeof(CardData.Name));
        int randomIndex = Random.Range(0, enums.Length);
        return enums[randomIndex];
    }

    [ContextMenu("GenerateRandomCards")]
    public void GenerateRandomCards()
    {
        if (_ownedCards.Count >= _maxObtainedCardCount) return;

        CardData.Name name = GetRandomCardName();

        bool alreadyContained = _ownedCards.ContainsKey(name);
        if(alreadyContained == true)
        {
            _ownedCards[name].CardCount += 1;
        }
        else
        {
            CardUI cardUI = _cardUIFactory.Create(name, _cardDataAssets[name]);
            cardUI.InjectDragEvents(OnCardDragStart, OnCardDragEnd);
            cardUI.transform.SetParent(_cardParent);
        }
    }

    // TODO
    // 카드 생성 기능 추가
    // 어드레서블 추가
    // 버프 선택지를 위한 Visitor 패턴 추가
    // 유닛 AI 추가
    // 도발 유닛 추가
    // 지름길 발판 추가
    // 텔레포트 발판 추가

    // 선택지에 따라 카드 선택
    // -> 카드 팩토리에서 생성 후 카드 컨트롤러로 넘겨줌
    // -> 이벤트 할당받음
    // -> 
}
