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
        // XZ ��� (y = 0)
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out float enter) == false)
        {
            idx = Vector2Int.zero;
            return false;
            // ���⿡ ���� �� ���ų� ���� �ƴ� ��� �����ϵ��� ���� �ʿ�
        }

        Vector3 hitPoint = ray.GetPoint(enter); // XZ ��� ���� ���콺 ��ǥ
        idx = _gridComponent.ReturnNodeIndex(new Vector2(hitPoint.x, hitPoint.z));

        return true; 
    }
}
