using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlantState : BaseState<Player.State>
    {
        public PlantState(FSM<Player.State> fsm) : base(fsm)
        {
        }

        public override void OnStateEnter()
        {
            Debug.Log("½É±â");
            _fsm.SetState(Player.State.Idle);
        }
    }
}