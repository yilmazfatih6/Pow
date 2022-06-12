using System;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Cube
{
    public class CubeMovementController : MonoBehaviour
    {
        [SerializeField] private FloatVariable movementDuration;
        [SerializeField] private Selectable selectable;
        
        private Vector3 _initialLocalPosition;
        private Transform _initialParent;

        public event Action OnMoveToOriginalPositionBegin; 
        public event Action OnMoveToOriginalPositionComplete; 

        public void Inject(Vector3 pos, Transform parent)
        {
            var t = transform;
            t.SetParent(parent);
            t.localPosition = pos;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;

            _initialLocalPosition = pos;
            _initialParent = parent;
        }

        public void MoveToOriginalPosition()
        {
            OnMoveToOriginalPositionBegin?.Invoke();
            
            var t = transform;
            Debug.Log("_initialParent " + _initialParent);
            transform.parent = _initialParent;
            t.localScale = Vector3.one;
            t.DOLocalMove(_initialLocalPosition, movementDuration.Value);
            t.DOLocalRotateQuaternion(Quaternion.identity, movementDuration.Value)
                .OnComplete(() => OnMoveToOriginalPositionComplete?.Invoke());
        }
    }
}