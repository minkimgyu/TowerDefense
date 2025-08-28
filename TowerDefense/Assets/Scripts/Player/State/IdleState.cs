using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class IdleState : BaseState<Player.State>
    {
        SelectComponent _selectComponent;

        public IdleState(FSM<Player.State> fsm, SelectComponent selectComponent) : base(fsm)
        {
            _selectComponent = selectComponent;
        }

        public override void OnStateUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_selectComponent.Select(out Vector2Int idx))
                {
                    Debug.Log($"Select Index: {idx}");
                }
            }

            if(Input.GetKeyDown(KeyCode.P))
            {
                _fsm.SetState(Player.State.Pick, AreaData.TwoXThree);
                return;
            }
        }
    }
}