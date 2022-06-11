using Managers;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject glow;
    [SerializeField] private float movementDuration = 0.5f;
    [SerializeField] private MatchAreaItemManager matchAreaItemManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        glow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        glow.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool isAdded = matchAreaItemManager.AddItem(gameObject);

        if (!isAdded) return;

        glow.SetActive(false);

        SetLayer();
        
        enabled = false;
    }

    private void SetLayer()
    {
        foreach (var t in GetComponentsInChildren<Transform>())
        {
            t.gameObject.layer = 5;
        }
    }
}