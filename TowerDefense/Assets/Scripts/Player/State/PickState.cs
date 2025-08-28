using FlowField;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PickState : BaseState<Player.State>
    {
        SelectComponent _selectComponent;
        SearchAreaComponent _searchAreaComponent;
        AreaSelector _areaSelector;

        public PickState(
            FSM<Player.State> fsm,
            SelectComponent selectComponent,
            SearchAreaComponent searchAreaComponent,
            AreaSelector areaSelector) : base(fsm)
        {
            _selectComponent = selectComponent;
            _searchAreaComponent = searchAreaComponent;
            _areaSelector = areaSelector;
            _storedIdx = new Vector2Int(0, 0);
        }

        public override void OnStateEnter(AreaData data)
        {
            _storedAreaData = data;
            _areaSelector.SetUp(data);
        }

        Vector2Int _storedIdx;
        AreaData _storedAreaData;

        public override void OnStateUpdate()
        {
            if (_selectComponent.Select(out Vector2Int idx))
            {
                if (idx == _storedIdx) return; // 이전에 선택한 위치와 같다면 무시

                Vector2Int resultIdx;
                if (_searchAreaComponent.FindEmptyArea(idx, _storedAreaData, out resultIdx))
                {
                    _areaSelector.Move(resultIdx);
                    _storedIdx = resultIdx;
                }
            }

            if(Input.GetMouseButtonUp(0))
            {
                _areaSelector.DisableSelector();
                _fsm.SetState(Player.State.Plant);
                return;
            }
        }
    }
}