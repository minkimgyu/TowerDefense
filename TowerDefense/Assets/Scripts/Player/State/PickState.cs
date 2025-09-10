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
                if (idx != _storedIdx)// 이전에 선택한 위치와 같다면 무시
                {
                    Vector2Int resultIdx;
                    if (_searchAreaComponent.FindEmptyArea(idx, _storedAreaData, out resultIdx))
                    {
                        // TODO
                        // 위치 바뀔 때마다 Pathfinder 돌려서 실제로 스폰포인트부터
                        // 목표 지점까지 도달할 수 있는지 확인해주기
                        // 만약 못 간다면 심는 것 불가능. 다른 위치 확인


                        _areaSelector.Move(resultIdx);
                        _storedIdx = resultIdx;
                    }
                }
            }
        }
    }
}