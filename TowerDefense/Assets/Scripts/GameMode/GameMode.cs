using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Profiling;
using UnityEngine;
using Player;

namespace FlowField
{
    public class GameMode : MonoBehaviour
    {
        [SerializeField] GridComponent _gridComponent;
        [SerializeField] PlayerController _playerController;
        [SerializeField] CardController _cardController;

        FlowField _flowField;

        private void Start()
        {
            AddressableLoader addressableLoader = FindObjectOfType<AddressableLoader>();
            if (addressableLoader == null) return;

            _gridComponent.Initialize();
            _flowField = new FlowField(_gridComponent);
            _playerController.Initialize(_gridComponent);

            CardUIFactory cardUIFactory = new CardUIFactory(
                addressableLoader.SpawnableUIAssets[ISpawnableUI.Name.CardUI],
                addressableLoader.CardIconSprites);

            _cardController.Initialize(addressableLoader.CardDataAssets, cardUIFactory);
            _cardController.InjectDragEvents(_playerController.OnCardDragStart, _playerController.OnCardDragEnd);
        }
    }
}