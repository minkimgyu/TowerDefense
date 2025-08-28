using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AreaData
{
    int _rowSize;
    int _colSize;
    Vector2Int _pivot;

    public AreaData(int rowSize, int colSize, Vector2Int pivot)
    {
        _rowSize = rowSize;
        _colSize = colSize;
        _pivot = pivot;
    }

    public int RowSize { get => _rowSize; }
    public int ColSize { get => _colSize; }
    public Vector2Int Pivot { get => _pivot; }

    //row, col
    static public AreaData OneXOne { get => new AreaData(1, 1, new Vector2Int(0, 0)); }
    static public AreaData TwoXTwo { get => new AreaData(2, 2, new Vector2Int(0, 0)); }
    static public AreaData TwoXThree { get => new AreaData(2, 3, new Vector2Int(0, 0)); }
    static public AreaData ThreeXTwo { get => new AreaData(3, 2, new Vector2Int(0, 0)); }
    static public AreaData ThreeXThree { get => new AreaData(1, 1, new Vector2Int(1, 1)); }
}
