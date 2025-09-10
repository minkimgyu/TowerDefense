using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AreaData
{
    [JsonProperty("rowSize")] int _rowSize;
    [JsonProperty("colSize")] int _colSize;
    [JsonProperty("pivot")] Vector2Int _pivot;

    public AreaData(int rowSize, int colSize, Vector2Int pivot)
    {
        _rowSize = rowSize;
        _colSize = colSize;
        _pivot = pivot;
    }

    [JsonIgnore] public int RowSize { get => _rowSize; }
    [JsonIgnore] public int ColSize { get => _colSize; }
    [JsonIgnore] public Vector2Int Pivot { get => _pivot; }

    //row, col
    [JsonIgnore] static public AreaData OneXOne { get => new AreaData(1, 1, new Vector2Int(0, 0)); }
    [JsonIgnore] static public AreaData TwoXTwo { get => new AreaData(2, 2, new Vector2Int(0, 0)); }
    [JsonIgnore] static public AreaData TwoXThree { get => new AreaData(2, 3, new Vector2Int(0, 0)); }
    [JsonIgnore] static public AreaData ThreeXTwo { get => new AreaData(3, 2, new Vector2Int(0, 0)); }
    [JsonIgnore] static public AreaData ThreeXThree { get => new AreaData(3, 3, new Vector2Int(1, 1)); }
}
