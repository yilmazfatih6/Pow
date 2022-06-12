using System;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(fileName = "LevelManager", menuName = "ScriptableObject/LevelManager", order = 0)]
    public class LevelManager : ScriptableObject
    {
        [SerializeField] private GameObjectCollection sceneItems;
        [SerializeField] private GameObjectCollection matchAreaItems;

        [Header("Raised Events")]
        [SerializeField] private GameEvent onLevelComplete;
        [SerializeField] private GameEvent onLevelFail;
        
        [Header("Listened Events")]
        [SerializeField] private GameEvent onNotMerge;
        [SerializeField] private GameEvent onMergeAnimationComplete;   
        
        private bool _isLevelOver;

        private void OnEnable()
        {
            _isLevelOver = false;
            onNotMerge.AddListener(CheckFail);
            onMergeAnimationComplete.AddListener(CheckComplete);
        }
        
        private void OnDisable()
        {
            onNotMerge.RemoveListener(CheckFail);
            onMergeAnimationComplete.RemoveListener(CheckComplete);
        }

        private void CheckFail()
        {
            if (matchAreaItems.Count == 7)
            {
                Fail();
            }
        }
        
        private void CheckComplete()
        {
            if (sceneItems.Count == 0)
            {
                Complete();
            }
        }

        private void Fail()
        {
            if (_isLevelOver) return;
            _isLevelOver = true;
            onLevelFail.Raise();
        }

        private void Complete()
        {
            if (_isLevelOver) return;
            _isLevelOver = true;
            onLevelComplete.Raise();
        }
    }
}