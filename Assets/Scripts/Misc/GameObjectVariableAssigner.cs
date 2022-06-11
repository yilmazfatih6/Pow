using ScriptableObjectArchitecture;
using UnityEngine;

namespace Misc
{
    public class GameObjectVariableAssigner : MonoBehaviour
    {
        [SerializeField] private GameObjectVariable variable;
        [SerializeField] private GameObject target;

        private void Awake()
        {
            variable.Value = target;
        }
    }
}