using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class AreaSelector : MonoBehaviour
{
    [SerializeField] Transform _pivotTransform;
    [SerializeField] GameObject _indicatorPrototype;
    List<Transform> _indicators;

    public void DisableSelector()
    {
        for (int i = 1; i < _indicators.Count; i++)
        {
            Destroy(_indicators[i].gameObject);
        }
        _indicators.Clear();
        _pivotTransform.gameObject.SetActive(false);
    }

    public void Move(Vector2Int idx)
    {
        transform.position = new Vector3(idx.y, 0.01f, -idx.x);
    }

    public void Initialize()
    {
        _indicators = new List<Transform>();
    }

    public void SetUp(AreaData data)
    {
        int size = data.RowSize * data.ColSize;
        _indicators.Add(_indicatorPrototype.transform);

        for (int i = 1; i < size; i++)
        {
            _indicators.Add(Instantiate(_indicatorPrototype, _pivotTransform).transform);
        }

        int count = 0;
        for (int i = 0; i < data.RowSize; i++)
        {
            for (int j = 0; j < data.ColSize; j++)
            {
                _indicators[count].localPosition = new Vector3(j, 0, -i);
                count++;
            }
        }

        // ÇÇ¹þ ¼³Á¤
        _pivotTransform.localPosition = new Vector3(-data.Pivot.x, 0, data.Pivot.y);
        _pivotTransform.gameObject.SetActive(true);
    }
}
