using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardUIFactory
{
    GameObject _cardUIPrefab;
    Dictionary<CardData.Name, Sprite> _cardIconSprites;

    public CardUIFactory(ISpawnableUI cardUI, Dictionary<CardData.Name, Sprite> cardIconSprites)
    {
        _cardUIPrefab = cardUI.GetObject();
        _cardIconSprites = cardIconSprites;
    }

    public CardUI Create(CardData.Name cardName, CardData cardData)
    {
        GameObject cardGO = Object.Instantiate(_cardUIPrefab);
        CardUI ui = cardGO.GetComponent<CardUI>();
        ui.Initialize(_cardIconSprites[cardName], cardData);

        return ui;
    }
}