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

            _heap.Insert(startNode); // 시작 노드 삽입
            _visited.Add(startNode); // 방문 처리

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
                        // 대각 방향이면 가중치를 1.4배 추가해줘야한다.
                        currentWeight *= 1.4f;
                    }

                    // minNode의 지금까지의 경로 가중치 + 주변 노드의 노드 가중치
                    float pathWeight = minNode.PathWeight + currentWeight;

                    bool nowContainNearNode = _visited.Contains(nearNodes[i]);
                    if (nowContainNearNode == true && nearNodes[i].PathWeight < pathWeight) continue;
                    // 이미 방문했고 가중치가 기존 것보다 더 큰 경우 건너뛰기

                    // 가중치 업데이트
                    nearNodes[i].PathWeight = pathWeight;

                    // 이미 방문했다면 건너뛰기
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