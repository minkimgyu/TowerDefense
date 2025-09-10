using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlantState : BaseState<PlayerController.State>
    {
        public PlantState(FSM<PlayerController.State> fsm) : base(fsm)
        {
        }

        public override void OnStateEnter()
        {
            // TODO
            // 심기면 이벤트 호출해서 실제 오브젝트 활성화시키기
            // 추가로 Grid Pathfinder 재계산 해주기

            Debug.Log("심기");
            _fsm.SetState(PlayerController.State.Idle);
        }
    }
}