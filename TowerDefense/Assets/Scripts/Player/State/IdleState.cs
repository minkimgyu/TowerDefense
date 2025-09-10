using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class IdleState : BaseState<PlayerController.State>
    {
        SelectComponent _selectComponent;

        public IdleState(FSM<PlayerController.State> fsm, SelectComponent selectComponent) : base(fsm)
        {
            _selectComponent = selectComponent;
        }

        public override void OnCardDragStart(CardData areaData) 
        {
            _fsm.SetState(PlayerController.State.Pick, areaData);
        }

        public override void OnStateUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_selectComponent.Select(out Vector2Int idx))
                {
                    // TODO
                    // 유닛 정보 보는 이벤트 추가
                    Debug.Log($"Select Index: {idx}");
                }
            }
        }
    }
}