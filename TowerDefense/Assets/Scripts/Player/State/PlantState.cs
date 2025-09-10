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
            // �ɱ�� �̺�Ʈ ȣ���ؼ� ���� ������Ʈ Ȱ��ȭ��Ű��
            // �߰��� Grid Pathfinder ���� ���ֱ�

            Debug.Log("�ɱ�");
            _fsm.SetState(PlayerController.State.Idle);
        }
    }
}