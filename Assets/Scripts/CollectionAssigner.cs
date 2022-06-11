using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class CollectionAssigner : MonoBehaviour
{
    [SerializeField] private GameObjectCollection collection;
    [SerializeField] private List<GameObject> elements;

    private void Awake()
    {
        collection.Clear();
        foreach (var slot in elements)
        {
            collection.Add(slot);
        }
    }
}