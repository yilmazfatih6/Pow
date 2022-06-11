using System;
using DG.Tweening;
using ScriptableObjectArchitecture;
using TMPro;
using UnityEngine;

namespace UI
{
    public class StarUI : MonoBehaviour
    {
        [Header("SO Data")]
        [SerializeField] private IntVariable totalStarCount;
        [SerializeField] private IntVariable starMultiplier;
        [SerializeField] private FloatVariable starAnimationDuration;
        [SerializeField] private GameObjectCollection mergedItems;

        [Header("Properties")]
        [SerializeField] private float starSpawnInterval;

        [Header("UI References")]
        [SerializeField] private GameObject starsUI;
        [SerializeField] private GameObject starToSpawn;
        [SerializeField] private TextMeshProUGUI text;
        
        [Header("Listened Events")]        
        [SerializeField] private GameEvent onMergeAnimationComplete;

        private void Start()
        {
            UpdateUI();
        }

        private void OnEnable()
        {
           onMergeAnimationComplete.AddListener(DisplayStar);
        }

        private void OnDisable()
        {
            onMergeAnimationComplete.RemoveListener(DisplayStar);
        }

        private void DisplayStar()
        {
            PlayMoneyEarnAnimation();
            
            DOVirtual.DelayedCall(starAnimationDuration.Value, () =>
            {
                UpdateUI();
            });
        }
        
        private void UpdateUI()
        {
            text.text = totalStarCount.Value.ToString();
        }
        
        private void PlayMoneyEarnAnimation()
        {
            // Get spawnTarget screen position. 
            Vector3 origin = mergedItems[0].transform.position;

            // Get destination screen position.
            Vector3 destination = starsUI.GetComponent<RectTransform>().position;

            for (int i = 0; i < starMultiplier; i++)
            {
                DOVirtual.DelayedCall(starSpawnInterval * i, () => SpawnMoney(origin, destination));
            }
        }

        private void SpawnMoney(Vector3 origin, Vector3 destination)
        {
            // Spawn money.
            GameObject spawned = Instantiate(starToSpawn);

            // Get rect transform from spawned object.
            RectTransform spawnedRectTransform = spawned.GetComponent<RectTransform>();

            // Set initial position
            spawnedRectTransform.position = origin;
            spawnedRectTransform.transform.SetParent(transform);
            spawnedRectTransform.localScale = Vector3.one;

            // Play tween.
            spawnedRectTransform.DOMove(destination, starAnimationDuration.Value).OnComplete(() =>
            {
                Destroy(spawned, 0.05f);
            });
        }
    }
}