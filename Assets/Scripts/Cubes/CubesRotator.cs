using ScriptableObjectArchitecture;
using UnityEngine;

namespace Cubes
{
    public class CubesRotator : MonoBehaviour
    {
        [SerializeField] private FloatVariable horizontalSwipe;
        [SerializeField] private FloatVariable verticalSwipe;

        private void OnEnable()
        {
            horizontalSwipe.AddListener(OnHorizontalSwipeChange);
            verticalSwipe.AddListener(OnVerticalSwipeChange);
        }

        private void OnDestroy()
        {
            horizontalSwipe.RemoveListener(OnHorizontalSwipeChange);
            verticalSwipe.RemoveListener(OnVerticalSwipeChange);
        }

        private void OnHorizontalSwipeChange(float value)
        {
            transform.Rotate(Vector3.up, -horizontalSwipe.Value, Space.World);
        }
        
        private void OnVerticalSwipeChange(float value)
        {
            transform.Rotate(Vector3.forward, verticalSwipe.Value, Space.World);
        }
    }
}