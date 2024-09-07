using System;
using System.Collections.Generic;
using System.Linq;
using TapAndRun.Interfaces;
using TapAndRun.Services.Progress;
using UnityEngine;
using VContainer;

namespace TapAndRun.Architecture
{
    public class ScopeDecomposer : MonoBehaviour
    {        
        private IDataService _dataService;
        private List<IDecomposable> _decomposables;

        [Inject]
        public void Contruct(IEnumerable<IDecomposable> decomposables, IDataService dataService)
        {
            _decomposables = decomposables.ToList(); //TODO Возможности стоит сразу получать список
            _dataService = dataService;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            Debug.Log("OnApplicationFocus");
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            Debug.Log("OnApplicationPause");
        }

        private void OnApplicationQuit()
        {
            SaveAndDecomposeAll();
        }

        private void SaveAndDecomposeAll()
        {
            _dataService.SaveInstant();

            foreach (var decomposable in _decomposables)
            {
                decomposable.Decompose();
            }

            Debug.Log("Scope decomposition was completed");
        }
    }
}