using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public enum State
        {
            Idle,
            Pick,
            Plant,
        }

        FSM<State> _fsm;

        [SerializeField] FlowField.GridComponent _gridComponent;
        [SerializeField] AreaSelector _areaSelector;

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            SelectComponent selectComponent = new SelectComponent(_gridComponent);
            SearchAreaComponent searchAreaComponent = new SearchAreaComponent(_gridComponent);
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

        private void Update()
        {
            _fsm.OnUpdate();
        }
    }

}