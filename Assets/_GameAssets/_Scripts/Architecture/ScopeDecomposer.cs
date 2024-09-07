using System.Collections.Generic;
using System.Linq;
using TapAndRun.Interfaces;
using UnityEngine;
using VContainer;

namespace TapAndRun.Architecture
{
    public class ScopeDecomposer : MonoBehaviour
    {
        private List<IDecomposable> _decomposables;
        
        [Inject]
        public void Contruct(IEnumerable<IDecomposable> decomposables)
        {
            _decomposables = decomposables.ToList(); //TODO Возможности стоит сразу получать список
        }

        private void OnDisable()
        {
            foreach (var decomposable in _decomposables)
            {
                decomposable.Decompose();
            }
            
            Debug.Log("Scope decomposition was completed");
        }
    }
}