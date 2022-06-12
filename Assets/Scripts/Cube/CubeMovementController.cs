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

        public event Action OnMoveToOriginalPositionComplete; 

        public void SetInitialPosition(Vector3 pos, Transform parent)
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
            var t = transform;
            t.parent = _initialParent;
            t.localScale = Vector3.one;
            t.DOLocalMove(_initialLocalPosition, movementDuration.Value);
            t.DOLocalRotateQuaternion(Quaternion.identity, movementDuration.Value)
                .OnComplete(() => OnMoveToOriginalPositionComplete?.Invoke());
        }

        public Tween MoveToMatchArea(Transform targetTransform)
        {
            var t = transform;
            t.SetParent(targetTransform);
            t.localScale = Vector3.one;
            t.DOMove(targetTransform.position, movementDuration.Value);
            return transform.DORotateQuaternion(targetTransform.rotation, movementDuration.Value);
        }

        public Tween Merge(Transform targetTransform)
        {
            var t = transform;
            return t.DOMove(targetTransform.position, movementDuration.Value);
        }
    }
}