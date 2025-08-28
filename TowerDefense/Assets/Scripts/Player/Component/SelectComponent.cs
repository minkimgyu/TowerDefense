using FlowField;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class SelectComponent
{
    GridComponent _gridComponent;

    public SelectComponent(GridComponent gridComponent)
    {
        _gridComponent = gridComponent;
    }

    public bool Select(out Vector2Int idx)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // XZ 평면 (y = 0)
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out float enter) == false)
        {
            idx = Vector2Int.zero;
            return false;
            // 여기에 심을 수 없거나 땅이 아닌 경우 무시하도록 변경 필요
        }

        Vector3 hitPoint = ray.GetPoint(enter); // XZ 평면 위의 마우스 좌표
        idx = _gridComponent.ReturnNodeIndex(new Vector2(hitPoint.x, hitPoint.z));

        return true; 
    }
}
