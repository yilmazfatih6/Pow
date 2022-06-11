using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace UI
{
    public class LevelStatusPanelController : MonoBehaviour
    {
        [SerializeField] private float uiSwitchDelay = 1.5f;

        [Header("Listened Events")]
        [SerializeField] private GameEvent onLevelComplete;
        [SerializeField] private GameEvent onLevelFail;
        
        [Header("UI References")]
        [SerializeField] private GameObject levelFailUI;
        [SerializeField] private GameObject levelCompleteUI;
        [SerializeField] private GameObject gameUI;
        
        private void OnEnable()
        {
            onLevelFail.AddListener(DisplayLevelFailUI);
            onLevelComplete.AddListener(DisplayLevelCompleteUI);
        }
        
        private void OnDisable()
        {
            onLevelFail.RemoveListener(DisplayLevelFailUI);
            onLevelComplete.RemoveListener(DisplayLevelCompleteUI);
        }

        private void DisplayLevelFailUI()
        {
            DOVirtual.DelayedCall(uiSwitchDelay, () =>
            {
                gameUI.SetActive(false);
                levelFailUI.SetActive(true);
            });
        }

        private void DisplayLevelCompleteUI()
        {
            DOVirtual.DelayedCall(uiSwitchDelay, () =>
            {
                gameUI.SetActive(false);
                levelCompleteUI.SetActive(true);
            });
        }
    }
}