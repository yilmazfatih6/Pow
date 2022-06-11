using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(fileName = "StarMultiplierManager", menuName = "ScriptableObject/StarMultiplierManager", order = 0)]
    public class StarMultiplierManager : ScriptableObject
    {
        [SerializeField] private float duration = 7;
        [SerializeField] private FloatVariable timerDurationPerfect;
        [SerializeField] private IntVariable starMultiplier;
        [SerializeField] private GameEvent onMerge;
        private Tween _tween;
        
        private void OnEnable()
        {
            onMerge.AddListener(IncreaseMultiplier);
            starMultiplier.Value = 1;
        }

        private void OnDisable()
        {
            onMerge.RemoveListener(IncreaseMultiplier);
        }
        
        private void IncreaseMultiplier()
        {
            starMultiplier.Value++;
            ResetTimer();
        }
        
        private void ResetMultiplier()
        {
            starMultiplier.Value = 1;
            ResetTimer();
        }

        private void ResetTimer()
        {
            _tween?.Kill();

            if (starMultiplier.Value == 1) return;
            
            _tween = DOVirtual.Float(1, 0, duration, (x) =>
            {
                timerDurationPerfect.Value = x;
            }).OnComplete(ResetMultiplier);
        }
    }
}