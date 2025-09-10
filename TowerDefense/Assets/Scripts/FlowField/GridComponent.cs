using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace FlowField
{
    public struct NearNodeData
    {
        List<Node> nearNodes;
        public List<Node> NearNodes { get { return nearNodes; } }

        bool haveBlockNodeInNeighbor;
        public bool HaveBlockNodeInNeighbor { get { return haveBlockNodeInNeighbor; } }

        public NearNodeData(List<Node> nearNodes, bool haveBlockNodeInNeighbor)
        {
            this.nearNodes = nearNodes;
            this.haveBlockNodeInNeighbor = haveBlockNodeInNeighbor;
        }
    }

    public class GridComponent : MonoBehaviour
    {
        [SerializeField] GridGenerator _gridGenerator;
        [SerializeField] GridDrawer _gridDrawer;

        Node[,] _nodes; // r, c

        Grid _grid;
        const int _nodeSize = 1;

        //      (1)
        // (0)↖ ↑ ↗(2)
        // (3)←    →(4)
        // (5)↙ ↓ ↘(7)
        //      (6)
        //              의 경우
        Vector2Int[] _nearIndexes = new Vector2Int[]
        {
            new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1),

            new Vector2Int(-1, 0), new Vector2Int(1, 0),

            new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1),
        };

        public Node ReturnNode(Vector2Int grid) { return _nodes[grid.x, grid.y]; }
        public Node ReturnNode(int r, int c) { return _nodes[r, c]; }

        int[,] _nodePrefixSum; // r, c

        public void InitializePrefixSum()
        {
            _nodePrefixSum = new int[_grid.RowSize, _grid.ColSize];

            if (_nodes[0, 0].CurrentState == Node.State.Block) _nodePrefixSum[0, 0] = 1;
            else _nodePrefixSum[0, 0] = 0;

            for (int i = 1; i < _grid.RowSize; i++)
            {
                int weight = 0;
                if (_nodes[i, 0].CurrentState == Node.State.Block) weight = 1;

                _nodePrefixSum[i, 0] = _nodePrefixSum[i - 1, 0] + weight;
            }

            for (int j = 1; j < _grid.ColSize; j++)
            {
                int weight = 0;
                if (_nodes[0, j].CurrentState == Node.State.Block) weight = 1;

                _nodePrefixSum[0, j] = _nodePrefixSum[0, j - 1] + weight;
            }

            for (int i = 1; i < _grid.RowSize; i++)
            {
                for (int j = 1; j < _grid.ColSize; j++)
                {
                    int weight = 0;
                    if (_nodes[i, j].CurrentState == Node.State.Block) weight = 1;

                    _nodePrefixSum[i, j] = _nodePrefixSum[i - 1, j] + _nodePrefixSum[i, j - 1] + weight - _nodePrefixSum[i - 1, j - 1];
                }
            }
        }

        public bool IsEmptyArea(Vector2Int idx, AreaData areaData)
        {
            Vector2Int topLeftIdx = new Vector2Int(idx.x - areaData.Pivot.x, idx.y - areaData.Pivot.y);
            Vector2Int bottomRightIdx = new Vector2Int(topLeftIdx.x + areaData.RowSize - 1, topLeftIdx.y + areaData.ColSize - 1);

            if (topLeftIdx.x < 0 || topLeftIdx.y < 0 || topLeftIdx.x >= _grid.RowSize || topLeftIdx.y >= _grid.ColSize) return false;
            if (bottomRightIdx.x < 0 || bottomRightIdx.y < 0 || bottomRightIdx.x >= _grid.RowSize || bottomRightIdx.y >= _grid.ColSize) return false;

            int totalWeight = _nodePrefixSum[bottomRightIdx.x, bottomRightIdx.y];

            int left = bottomRightIdx.y - areaData.ColSize;
            if (left > 0) totalWeight -= _nodePrefixSum[bottomRightIdx.x, left];

            int up = bottomRightIdx.x - areaData.RowSize;
            if (up > 0) totalWeight -= _nodePrefixSum[up, bottomRightIdx.y];

            if (up > 0 && left > 0) totalWeight += _nodePrefixSum[up, left];

            if (totalWeight == 0) return true;
            else return false;
        }

        //public bool IsEmptyArea(Vector2Int idx, AreaData areaData)
        //{
        //    for (int i = 0; i < areaData.RowSize; i++)
        //    {
        //        for (int j = 0; j < areaData.ColSize; j++)
        //        {
        //            Vector2Int checkIdx = new Vector2Int(idx.x + i - areaData.Pivot.x, idx.y + j - areaData.Pivot.y);
        //            if (IsOutOfRange(checkIdx)) return false;
        //            if (_nodes[checkIdx.x, checkIdx.y].CurrentState == Node.State.Block) return false;
        //        }
        //    }

        //    return true;
        //}


        public void ResetNodeWeight()
        {
            for (int i = 0; i < _grid.RowSize; i++)
            {
                for (int j = 0; j < _grid.ColSize; j++)
                {
                    _nodes[i, j].PathWeight = float.MaxValue;
                }
            }
        }

        public void CalculateNodePath(Node startNode)
        {
            for (int i = 0; i < _grid.RowSize; i++)
            {
                for (int j = 0; j < _grid.ColSize; j++)
                {
                    if (startNode == _nodes[i, j])
                    {
                        startNode.DirectionToMove = Vector2.zero;
                        continue;
                    }

                    Vector2 direction;
                    List<Node> nearNodes = _nodes[i, j].MovableNodes;
                    float minWeight = float.MaxValue;
                    int minIndex = 0;

                    bool haveBlockNodeInNeighbor = _nodes[i, j].HaveBlockNodeInNeighbor;

                    for (int k = 0; k < nearNodes.Count; k++)
                    {
                        // 현재 노드 주변에 Block 노드가 있고 주변 노드의 주변에 Block 노드가 있는 경우 건너뛰기
                        if (haveBlockNodeInNeighbor && nearNodes[k].HaveBlockNodeInNeighbor == true) continue;

                        if (nearNodes[k].PathWeight < minWeight)
                        {
                            minWeight = nearNodes[k].PathWeight;
                            minIndex = k;
                        }
                    }

                    direction = (nearNodes[minIndex].WorldPos - _nodes[i, j].WorldPos).normalized;

                    _nodes[i, j].DirectionToMove = direction;
                }
            }
        }

        public Vector2 ReturnClampedRange(Vector2 pos)
        {
            Vector2 topLeftPos = ReturnNode(0, 0).WorldPos;
            Vector2 bottomRightPos = ReturnNode(_grid.RowSize - 1, _grid.ColSize - 1).WorldPos;

            // 반올림하고 범위 안에 맞춰줌
            // 이 부분은 GridSize 바뀌면 수정해야함
            float xPos = Mathf.Clamp(pos.x, topLeftPos.x, bottomRightPos.x);
            float yPos = Mathf.Clamp(pos.y, bottomRightPos.y, topLeftPos.y);

            return new Vector2(xPos, yPos);
        }

        public Vector2 ReturnNodeDirection(Vector2 worldPos)
        {
            Vector2Int index = ReturnNodeIndex(worldPos);
            Node node = ReturnNode(index);
            return node.DirectionToMove;
        }

        public Vector2Int ReturnNodeIndex(Vector2 worldPos)
        {
            Vector2 clampedPos = ReturnClampedRange(worldPos);
            Vector2 topLeftPos = ReturnNode(0, 0).WorldPos;

            int r = Mathf.RoundToInt(Mathf.Abs(topLeftPos.y - clampedPos.y) / _nodeSize);
            int c = Mathf.RoundToInt(Mathf.Abs(topLeftPos.x - clampedPos.x) / _nodeSize); // 인덱스이므로 1 빼준다.
            return new Vector2Int(r, c);
        }

        public Vector2 ReturnClampPos(Vector2 worldPos)
        {
            Vector2Int grid = ReturnNodeIndex(worldPos);
            return _nodes[grid.x, grid.y].WorldPos;
        }

        bool IsOutOfRange(Vector2Int index) { return index.x < 0 || index.y < 0 || index.x >= _grid.RowSize || index.y >= _grid.ColSize; }

        public List<Node> ReturnNearNodes(Vector2Int index)
        {
            List<Node> nearNodes = new List<Node>();

            for (int i = 0; i < _nearIndexes.Length; i++)
            {
                Vector2Int nearNodeIndex = index + _nearIndexes[i];
                bool outOfRange = IsOutOfRange(nearNodeIndex);
                if (outOfRange == true) continue;

                Node node = ReturnNode(nearNodeIndex);
                nearNodes.Add(node);
            }

            return nearNodes;
        }

        public NearNodeData ReturnMovableNodeData(Vector2Int index)
        {
            List<Node> nearNodes = new List<Node>();
            bool haveBlockNode = false;

            //      (1)
            // (0)↖ ↑ ↗(2)
            // (3)←    →(4)
            // (5)↙ ↓ ↘(7)
            //      (6)
            //              의 경우

            for (int i = 0; i < _nearIndexes.Length; i++)
            {
                Vector2Int nearNodeIndex = index + _nearIndexes[i];

                bool isOutOfRange = IsOutOfRange(nearNodeIndex);
                if (isOutOfRange == true) continue;

                bool canPass = true;
                bool topIsBlock, leftIsBlock, rightIsBlock, bottomIsBlock;

                switch (i)
                {
                    case 0:
                        topIsBlock = ReturnNode(index + _nearIndexes[1]).CurrentState == Node.State.Block;
                        leftIsBlock = ReturnNode(index + _nearIndexes[3]).CurrentState == Node.State.Block;
                        if (topIsBlock || leftIsBlock) canPass = false;

                        break;
                    case 2:
                        topIsBlock = ReturnNode(index + _nearIndexes[1]).CurrentState == Node.State.Block;
                        rightIsBlock = ReturnNode(index + _nearIndexes[4]).CurrentState == Node.State.Block;
                        if (topIsBlock || rightIsBlock) canPass = false;

                        break;
                    case 5:
                        leftIsBlock = ReturnNode(index + _nearIndexes[3]).CurrentState == Node.State.Block;
                        bottomIsBlock = ReturnNode(index + _nearIndexes[6]).CurrentState == Node.State.Block;
                        if (leftIsBlock || bottomIsBlock) canPass = false;

                        break;
                    case 7:
                        rightIsBlock = ReturnNode(index + _nearIndexes[4]).CurrentState == Node.State.Block;
                        bottomIsBlock = ReturnNode(index + _nearIndexes[6]).CurrentState == Node.State.Block;
                        if (rightIsBlock || bottomIsBlock) canPass = false;

                        break;
                    default:
                        break;
                }

                if (canPass == false) continue; // 못 가는 지역이라면 건너뛰기

                Node node = ReturnNode(nearNodeIndex);
                if (node.CurrentState == Node.State.Block)
                {
                    haveBlockNode = true;
                }

                nearNodes.Add(node);
            }

            NearNodeData nearNodeData = new NearNodeData(nearNodes, haveBlockNode);
            return nearNodeData;
        }

        public void Initialize()
        {
            _nodes = _gridGenerator.CreateGrid();

            _grid = new Grid(_nodes.GetLength(0), _nodes.GetLength(1));
            _gridDrawer.Initialize(_nodes, _grid);

            InitializePrefixSum();

            for (int i = 0; i < _grid.RowSize; i++)
            {
                for (int j = 0; j < _grid.ColSize; j++)
                {
                    NearNodeData neaNodeData = ReturnMovableNodeData(new Vector2Int(i, j));
                    _nodes[i, j].MovableNodes = neaNodeData.NearNodes;
                    _nodes[i, j].HaveBlockNodeInNeighbor = neaNodeData.HaveBlockNodeInNeighbor;

                    _nodes[i, j].NearNodes = ReturnNearNodes(new Vector2Int(i, j));
                }
            }
        }
    }
}