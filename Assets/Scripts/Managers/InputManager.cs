using System;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Properties
        [Header("Properties")]
        [SerializeField] private bool enableTap;
        [SerializeField] private bool enableSwipe;

        [Header("SO References")]
        [SerializeField] private FloatVariable horizontalSwipe;
        [SerializeField] private FloatVariable verticalSwipe;

        [Header("Broadcasting On")]
        [SerializeField] private GameEvent onTap;
        [SerializeField] private GameEvent onRelease;

        private Vector3 _previousTouchPosition;
        private Vector3 _currentTouchPosition;
        private bool _isFirstTouch = true;
        private bool _isSwipeActive;
        private bool _isTapActive;
        private bool _isTapped;
        #endregion

        #region Unity Methods

        private void Start()
        {
            Enable();
        }

        private void Update()
        {
            if (Input.touchCount > 1)
            {
                return;
            }
            
            if (!_isTapped && UnityEngine.Input.GetMouseButtonDown(0))
            {
                _isTapped = true;

                if (_isSwipeActive)
                {
                    // Debug.Log("Set as first touch");
                    _isFirstTouch = true;
                }
                if (_isTapActive)
                {
                    // Debug.Log("On Tap");
                    onTap.Raise();
                }
            }
            else if (_isTapped && UnityEngine.Input.GetMouseButton(0))
            {
                if (_isSwipeActive)
                {
                    // Debug.Log("Detect swipe");
                    DetectSwipe();
                }
            }
            else if (_isTapped && UnityEngine.Input.GetMouseButtonUp(0))
            {
                _isTapped = false;

                // Reset swipe
                horizontalSwipe.Value = 0;
                verticalSwipe.Value = 0;
                // Debug.Log("Reset swipe");
                
                // Raise event.
                if (_isTapActive)
                {
                    // Debug.Log("On Release");
                    onRelease.Raise();
                }
            }
        }
        #endregion

        #region Private Methods

        private void DetectSwipe()
        {
            // Set previous touch position.
            if (_isFirstTouch)
            {
                // Debug.Log("First touch.");
                _previousTouchPosition = UnityEngine.Input.mousePosition;
                _isFirstTouch = false;
            }
            else
            {
                // Debug.Log("Not first touch");
                _previousTouchPosition = _currentTouchPosition;
            }

            // Set current touch position.
            _currentTouchPosition = UnityEngine.Input.mousePosition;

            // Set Horizontal Swipe
            horizontalSwipe.Value = _currentTouchPosition.x - _previousTouchPosition.x;
            // Debug.Log("horizontalSwipe.Value  " + horizontalSwipe.Value);

            // Set Vertical Swipe
            verticalSwipe.Value = _currentTouchPosition.y - _previousTouchPosition.y;
            // Debug.Log("verticalSwipe.Value  " + verticalSwipe.Value);
        }

        private void Enable()
        {
            if (enableSwipe)
            {
                _isSwipeActive = true;
            }

            if (enableTap)
            {
                _isTapActive = true;
            }
        }

        private void Disable()
        {
            _isSwipeActive = false;
            _isTapActive = false;
        }

        #endregion
    }
}