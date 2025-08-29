using FlowField;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchAreaComponent
{
    GridComponent _gridComponent;
    Queue<Node> _queue;
    HashSet<Node> _visited;

    public SearchAreaComponent(GridComponent gridComponent)
    {
        _gridComponent = gridComponent;
        _queue = new Queue<Node>();
        _visited = new HashSet<Node>();
    }

    public bool FindEmptyArea(Vector2Int idx, AreaData areaData, out Vector2Int resultIdx)
    {
        _queue.Clear();
        _visited.Clear();

        bool isEmpty;

        // �� �ڸ��� �� ������ �ִ��� Ȯ��
        isEmpty = _gridComponent.IsEmptyArea(idx, areaData);
        if (isEmpty == true) // �ִٸ� ���� ��ġ ��ȯ
        {
            resultIdx = idx;
            return true;
        }

        Node startNode = null;

        try
        {
            // ���ٸ� BFS Ž�� ����
            startNode = _gridComponent.ReturnNode(idx);
            if (startNode == null)
            {
                resultIdx = Vector2Int.zero;
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.Log(idx);
        }
        

        _queue.Enqueue(startNode);
        _visited.Add(startNode);

        while (_queue.Count > 0)
        {
            Node node = _queue.Dequeue();

            for (int i = 0; i < node.NearNodes.Count; i++)
            {
                if (node.NearNodes[i] == null) continue;
                //if (node.NearNodes[i].CurrentState == Node.State.Block) continue;
                if (_visited.Contains(node.NearNodes[i]) == true) continue;

                isEmpty = _gridComponent.IsEmptyArea(node.NearNodes[i].Index, areaData);
                if (isEmpty == true)
                {
                    resultIdx = node.NearNodes[i].Index;
                    return true;
                }

                _queue.Enqueue(node.NearNodes[i]);
                _visited.Add(node.NearNodes[i]);
            }
        }

        resultIdx = Vector2Int.zero;
        return false;
    }
}
