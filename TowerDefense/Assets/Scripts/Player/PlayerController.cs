using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public enum State
        {
            Idle,
            Pick,
            Plant,
        }

        FSM<State> _fsm;

        [SerializeField] AreaSelector _areaSelector;

        public void Initialize(FlowField.GridComponent gridComponent)
        {
            SelectComponent selectComponent = new SelectComponent(gridComponent);
            SearchAreaComponent searchAreaComponent = new SearchAreaComponent(gridComponent);
            _areaSelector.Initialize();

            _fsm = new FSM<State>();
            Dictionary<State, BaseState<State>> states = new Dictionary<State, BaseState<State>>()
            {
                { State.Idle, new IdleState(_fsm, selectComponent) },
                { State.Pick, new PickState(_fsm, selectComponent, searchAreaComponent, _areaSelector) },
                { State.Plant, new PlantState(_fsm) },
            };
            _fsm.Initialize(states, State.Idle);
        }

        public void OnCardDragStart(CardData cardData) => _fsm.OnCardDragStart(cardData);
        public void OnCardDragEnd(bool canPlant) => _fsm.OnCardDragEnd(canPlant);


        private void Update()
        {
            if(_fsm == null) return;
            _fsm.OnUpdate();
        }
    }

}