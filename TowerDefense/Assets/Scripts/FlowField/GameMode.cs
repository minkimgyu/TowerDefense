using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Profiling;
using UnityEngine;

namespace FlowField
{
    public class GameMode : MonoBehaviour
    {
        [SerializeField] GridComponent _gridComponent;
        [SerializeField] GridGenerator _gridGenerator;
        [SerializeField] GridDrawer _gridDrawer;
        [SerializeField] FlowField _dijkstra;

        private void Start()
        {
            _gridComponent.Initialize(_gridGenerator, _gridDrawer, _dijkstra);
        }
    }
}