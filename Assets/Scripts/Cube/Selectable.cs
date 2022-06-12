using System;
using Managers;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cube
{
    public class Selectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private CubeMovementController cubeMovementController;
        [SerializeField] private GameObject glow;
        [SerializeField] private GameObjectGameEvent onSelect;
        private bool _isActive = true;    
    
        public event Action OnFocus; 
        public event Action OnUnfocus; 
        public event Action OnClick;

        private void OnEnable()
        {
            cubeMovementController.OnMoveToOriginalPositionComplete += Activate;
        }

        private void OnDisable()
        {
            cubeMovementController.OnMoveToOriginalPositionComplete -= Activate;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isActive) return;
        
            OnFocus?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isActive) return;

            OnUnfocus?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isActive) return;

            onSelect.Raise(gameObject);

            OnClick?.Invoke();
        
            _isActive = false;
        }

        private void Activate()
        {
            _isActive = true;
        }
    }
}