using ScriptableObjectArchitecture;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MultiplierBarUI : MonoBehaviour
    {
        [Header("SO Data")]
        [SerializeField] private FloatVariable timerDurationPerfect;
        [SerializeField] private IntVariable starMultiplier;

        [Header("UI References")]
        [SerializeField] private Image processBar;
        [SerializeField] private TextMeshProUGUI multiplierText;
        
        private void OnEnable()
        {
            timerDurationPerfect.AddListener(OnTimerDurationPerfectChange);
            starMultiplier.AddListener(OnStarMultiplierChange);
        }

        private void OnDisable()
        {
            timerDurationPerfect.RemoveListener(OnTimerDurationPerfectChange);
            starMultiplier.RemoveListener(OnStarMultiplierChange);
        }

        private void OnTimerDurationPerfectChange(float value)
        {
            processBar.fillAmount = value;
        }

        private void OnStarMultiplierChange(int value)
        {
            multiplierText.text = "X" + value;
        }
    }
}