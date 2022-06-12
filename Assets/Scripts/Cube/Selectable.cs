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
        [SerializeField] private GameObjectGameEvent onHintSelect;
        private bool _isActive = true;    
    
        public event Action OnFocus; 
        public event Action OnUnfocus; 
        public event Action OnClick;

        private void OnEnable()
        {
            cubeMovementController.OnMoveToOriginalPositionComplete += Activate;
            onHintSelect.AddListener(OnHintSelect);
        }

        private void OnDisable()
        {
            cubeMovementController.OnMoveToOriginalPositionComplete -= Activate;
            onHintSelect.RemoveListener(OnHintSelect);
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
            _isActive = false;
            Select();
        }

        private void Select()
        {
            onSelect.Raise(gameObject);
            OnClick?.Invoke();
        }
        
        private void OnHintSelect(GameObject value)
        {
            if (value != gameObject) return;
            Select();
        }
        
        private void Activate()
        {
            _isActive = true;
        }
    }
}