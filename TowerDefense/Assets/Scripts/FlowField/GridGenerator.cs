using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FlowField
{
    public struct Grid
    {
        private int _rowSize;
        private int _colSize;

        public int RowSize { get => _rowSize; set => _rowSize = value; }
        public int ColSize { get => _colSize; set => _colSize = value; }

        public Grid(int rowSize, int colSize)
        {
            _rowSize = rowSize;
            _colSize = colSize;
        }
    }

    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] GameObject _wallTileParent;
        [SerializeField] GameObject _groundTileParent;

        Node[,] _nodes; // r, c

        Grid _grid;
        Dictionary<Vector2Int, bool> _isWall = new Dictionary<Vector2Int, bool>();

        public Node[,] CreateGrid()
        {
            Transform[] groundTiles = _groundTileParent.GetComponentsInChildren<Transform>();

            Vector2Int minBound = new Vector2Int(int.MaxValue, int.MaxValue);
            Vector2Int maxBound = new Vector2Int(int.MinValue, int.MinValue);

            foreach (Transform t in groundTiles)
            {
                if (t == _groundTileParent.transform) continue;

                Vector3Int cellPos = new Vector3Int((int)t.position.x, (int)t.position.y, (int)t.position.z);
                if (cellPos.x < minBound.x) minBound.x = cellPos.x;
                if (cellPos.z < minBound.y) minBound.y = cellPos.z;
                if (cellPos.x > maxBound.x) maxBound.x = cellPos.x;
                if (cellPos.z > maxBound.y) maxBound.y = cellPos.z;
            }

            _grid = new Grid(maxBound.y - minBound.y + 1, maxBound.x - minBound.x + 1);
            _nodes = new Node[_grid.RowSize, _grid.ColSize];

            Transform[] wallTiles = _wallTileParent.GetComponentsInChildren<Transform>();

            foreach (Transform t in wallTiles)
            {
                if (t == _wallTileParent.transform) continue;

                Vector2Int pos = new Vector2Int((int)t.position.x, (int)t.position.z);
                _isWall[pos] = true;
            }

            for (int i = 0; i < _grid.RowSize; i++)
            {
                for (int j = 0; j < _grid.ColSize; j++)
                {
                    Vector2Int pos = new Vector2Int(j, -i);
                    Vector2Int idx = new Vector2Int(i, j);

                    Node node;
                    if (_isWall.ContainsKey(pos) == true && _isWall[pos] == true)
                    {
                        node = new Node(pos, idx, Node.State.Block);
                    }
                    else
                    {
                        node = new Node(pos, idx, Node.State.Empty);
                    }

                    _nodes[i, j] = node;
                    // 타일이 없다면 바닥
                    // 타일이 존재한다면 벽
                }
            }

            return _nodes;
        }
    }
}