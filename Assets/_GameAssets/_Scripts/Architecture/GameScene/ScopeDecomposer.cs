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
                SaveAndDecomposeAll();
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveAndDecomposeAll();
            }
        }

        private void OnApplicationQuit()
        {
            SaveAndDecomposeAll();
        }

        private void SaveAndDecomposeAll()
        {
            if (Time.time - _lastQuickSaveTime < _quickSaveTimeout)
            {
                return;
            }
            
            _dataService.Save();

            foreach (var decomposable in _decomposables)
            {
                decomposable.Decompose();
            }

            _lastQuickSaveTime = Time.time;

            Debug.Log("Scope decomposition was completed");
        }
    }
}