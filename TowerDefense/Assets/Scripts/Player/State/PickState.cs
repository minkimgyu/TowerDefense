using FlowField;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PickState : BaseState<PlayerController.State>
    {
        SelectComponent _selectComponent;
        SearchAreaComponent _searchAreaComponent;
        AreaSelector _areaSelector;

        public PickState(
            FSM<PlayerController.State> fsm,
            SelectComponent selectComponent,
            SearchAreaComponent searchAreaComponent,
            AreaSelector areaSelector) : base(fsm)
        {
            _selectComponent = selectComponent;
            _searchAreaComponent = searchAreaComponent;
            _areaSelector = areaSelector;
            _storedIdx = new Vector2Int(0, 0);
        }

        public override void OnStateEnter(CardData data)
        {
            _storedAreaData = data.AreaData;
            _areaSelector.SetUp(_storedAreaData);
        }

        Vector2Int _storedIdx;
        AreaData _storedAreaData;

        public override void OnCardDragEnd(bool canPlant)
        {
            _areaSelector.DisableSelector();

            if (canPlant == true)
            {
                _fsm.SetState(PlayerController.State.Plant);
            }
            else
            {
                _fsm.SetState(PlayerController.State.Idle);
            }
        }

        public override void OnStateUpdate()
        {
            if (_selectComponent.Select(out Vector2Int idx))
            {
                if (idx != _storedIdx)// ������ ������ ��ġ�� ���ٸ� ����
                {
                    Vector2Int resultIdx;
                    if (_searchAreaComponent.FindEmptyArea(idx, _storedAreaData, out resultIdx))
                    {
                        // TODO
                        // ��ġ �ٲ� ������ Pathfinder ������ ������ ��������Ʈ����
                        // ��ǥ �������� ������ �� �ִ��� Ȯ�����ֱ�
                        // ���� �� ���ٸ� �ɴ� �� �Ұ���. �ٸ� ��ġ Ȯ��


                        _areaSelector.Move(resultIdx);
                        _storedIdx = resultIdx;
                    }
                }
            }
        }
    }
}