using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlowField
{
    public class FlowField : MonoBehaviour
    {
        GridComponent _gridComponent;

        public void Initialize(GridComponent gridComponent)
        {
            _gridComponent = gridComponent;
            _heap = new Heap<Node>(1000);
            _visited = new HashSet<Node>();
        }

        Vector2 TopLeft = new Vector2(-1, 1);
        Vector2 TopRight = new Vector2(1, 1);
        Vector2 BottomLeft = new Vector2(-1, -1);
        Vector2 BottomRight = new Vector2(1, -1);

        Heap<Node> _heap;
        HashSet<Node> _visited;

        public void FindPath(Vector2Int index)
        {
            _gridComponent.ResetNodeWeight();
            Node startNode = _gridComponent.ReturnNode(index);
            startNode.PathWeight = 0;

            _heap.Insert(startNode); // ���� ��� ����
            _visited.Add(startNode); // �湮 ó��

            while (_heap.Count > 0)
            {
                Node minNode = _heap.ReturnMin();
                _heap.DeleteMin();

                List<Node> nearNodes = minNode.MovableNodes;
                for (int i = 0; i < nearNodes.Count; i++)
                {
                    float currentWeight = nearNodes[i].Weight;

                    Vector2 directionVec = nearNodes[i].WorldPos - minNode.WorldPos;
                    if (directionVec == TopLeft ||
                        directionVec == TopRight ||
                        directionVec == BottomLeft ||
                        directionVec == BottomRight)
                    {
                        // �밢 �����̸� ����ġ�� 1.4�� �߰�������Ѵ�.
                        currentWeight *= 1.4f;
                    }

                    // minNode�� ���ݱ����� ��� ����ġ + �ֺ� ����� ��� ����ġ
                    float pathWeight = minNode.PathWeight + currentWeight;

                    bool nowContainNearNode = _visited.Contains(nearNodes[i]);
                    if (nowContainNearNode == true && nearNodes[i].PathWeight < pathWeight) continue;
                    // �̹� �湮�߰� ����ġ�� ���� �ͺ��� �� ū ��� �ǳʶٱ�

                    // ����ġ ������Ʈ
                    nearNodes[i].PathWeight = pathWeight;

                    // �̹� �湮�ߴٸ� �ǳʶٱ�
                    if (nowContainNearNode == true) continue;
                    _visited.Add(nearNodes[i]);

                    _heap.Insert(nearNodes[i]);
                }
            }

            _gridComponent.CalculateNodePath(startNode);
            _heap.Clear();
            _visited.Clear();
        }
    }

}