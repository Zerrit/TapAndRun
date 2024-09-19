using System.Collections.Generic;
using System.Linq;
using TapAndRun.Interfaces;
using TapAndRun.Services.Data;
using UnityEngine;
using VContainer;

namespace TapAndRun.Architecture.GameScene
{
    public class ScopeDecomposer : MonoBehaviour
    {
        [Header("Время между сохранениями (сек.)")]
        [SerializeField] private float _quickSaveTimeout;

        private float _lastQuickSaveTime;

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
            if (!hasFocus)
            {
                QuickSave();
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                QuickSave();
            }
        }

        private void OnApplicationQuit()
        {
            QuickSave();
            DecomposeScope();
        }

        private void DecomposeScope()
        {
            foreach (var decomposable in _decomposables)
            {
                decomposable.Decompose();
            }

            Debug.Log("Scope decomposition was completed");
        }

        private void QuickSave()
        {
            if (Time.time - _lastQuickSaveTime < _quickSaveTimeout && _lastQuickSaveTime != 0)
            {
                return;
            }
 
            _lastQuickSaveTime = Time.time;
            _dataService.Save();

            Debug.Log("QuickSave was completed");
        }
    }
}