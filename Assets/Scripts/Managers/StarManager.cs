using System;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(fileName = "StarManager", menuName = "ScriptableObject/StarManager", order = 0)]
    public class StarManager : ScriptableObject
    {
        [Header("Listened Events")]        
        [SerializeField] private GameEvent onMergeAnimationComplete;
        
        [Header("SO Data")]
        [SerializeField] private IntVariable totalStarCount;
        [SerializeField] private IntVariable starMultiplier;

        private void OnEnable()
        {
            onMergeAnimationComplete.AddListener(EarnStar);
        }

        private void OnDisable()
        {
            onMergeAnimationComplete.RemoveListener(EarnStar);
        }

        private void EarnStar()
        {
            totalStarCount.Value += starMultiplier.Value;
        }
    }
}