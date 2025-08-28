using FlowField;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FlowField
{
    public class GridDrawer : MonoBehaviour
    {
        public enum ShowType
        {
            None,
            Block,
            Weight,
            Direction
        }

        [SerializeField] ShowType _showType;
        Node[,] _nodes; // r, c
        Grid _grid;

        // 스타일 지정
        GUIStyle blockStyle = new GUIStyle();
        GUIStyle emptyStyle = new GUIStyle();
        GUIStyle weightStyle = new GUIStyle();

        GUIStyle startPointStyle = new GUIStyle();

        public enum Direction
        {
            UpLeft,
            Up,
            UpRight,
            Left,
            Current,
            Right,
            DownLeft,
            Down,
            DownRight
        }

        public Dictionary<Vector2, Direction> directions = new Dictionary<Vector2, Direction>()
        {
            { new Vector2(-1, 1).normalized, Direction.UpLeft },
            { new Vector2(0, 1), Direction.Up },
            { new Vector2(1, 1).normalized, Direction.UpRight },

            { new Vector2(-1, 0), Direction.Left },
            { new Vector2(0, 0), Direction.Current },
            { new Vector2(1, 0), Direction.Right },

            { new Vector2(-1, -1).normalized, Direction.DownLeft },
            { new Vector2(0, -1), Direction.Down },
            { new Vector2(1, -1).normalized, Direction.DownRight },
        };

        public void Initialize(Node[,] nodes, Grid grid)
        {
            _nodes = nodes;
            _grid = grid;

            blockStyle.fontSize = 20;
            blockStyle.alignment = TextAnchor.MiddleCenter;
            blockStyle.normal.textColor = Color.red;

            emptyStyle.fontSize = 20;
            emptyStyle.alignment = TextAnchor.MiddleCenter;
            emptyStyle.normal.textColor = Color.blue;

            weightStyle.fontSize = 5;
            weightStyle.alignment = TextAnchor.MiddleCenter;
            weightStyle.normal.textColor = Color.white;

            startPointStyle.fontSize = 20;
            startPointStyle.alignment = TextAnchor.MiddleCenter;
            startPointStyle.normal.textColor = Color.white;
        }

        private void OnDrawGizmos()
        {
            if (_nodes == null) return;

            for (int i = 0; i < _grid.RowSize; i++)
            {
                for (int j = 0; j < _grid.ColSize; j++)
                {
                    Vector3 toV2 = new Vector3(_nodes[i, j].WorldPos.x, 0, _nodes[i, j].WorldPos.y);

                    if (_showType == ShowType.Direction)
                    {
                        if (_nodes[i, j].CurrentState == Node.State.Block)
                        {
                            Handles.Label(toV2, "X", blockStyle);
                        }
                        else
                        {
                            Direction direction = directions[_nodes[i, j].DirectionToMove];
                            switch (direction)
                            {
                                case Direction.UpLeft:
                                    Handles.Label(toV2, "↖", emptyStyle);
                                    break;
                                case Direction.Up:
                                    Handles.Label(toV2, "↑", emptyStyle);
                                    break;
                                case Direction.UpRight:
                                    Handles.Label(toV2, "↗", emptyStyle);
                                    break;
                                case Direction.Left:
                                    Handles.Label(toV2, "←", emptyStyle);
                                    break;
                                case Direction.Current:
                                    Handles.Label(toV2, "○", startPointStyle);
                                    break;
                                case Direction.Right:
                                    Handles.Label(toV2, "→", emptyStyle);
                                    break;
                                case Direction.DownLeft:
                                    Handles.Label(toV2, "↙", emptyStyle);
                                    break;
                                case Direction.Down:
                                    Handles.Label(toV2, "↓", emptyStyle);
                                    break;
                                case Direction.DownRight:
                                    Handles.Label(toV2, "↘", emptyStyle);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    else if (_showType == ShowType.Block)
                    {
                        switch (_nodes[i, j].CurrentState)
                        {
                            case Node.State.Empty:
                                Handles.Label(toV2, "○", emptyStyle);
                                break;
                            case Node.State.Block:
                                Handles.Label(toV2, "X", blockStyle);
                                break;
                            default:
                                break;
                        }
                    }
                    else if (_showType == ShowType.Weight)
                    {
                        switch (_nodes[i, j].CurrentState)
                        {
                            case Node.State.Empty:
                                Handles.Label(toV2, _nodes[i, j].PathWeight.ToString(), emptyStyle);
                                break;
                            case Node.State.Block:
                                Handles.Label(toV2, _nodes[i, j].PathWeight.ToString(), blockStyle);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}