using System.Collections;
using System.Collections.Generic;
using Cube;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(fileName = "HintManager", menuName = "ScriptableObject/HintManager", order = 0)]
    public class HintManager : ScriptableObject
    {
        [Header("Properties")]
        [SerializeField] private float selectionInterval = 0.1f;
        
        [Header("Listened Events")] 
        [SerializeField] private GameEvent onHintButtonClick;
        
        [Header("Raised Events")] 
        [SerializeField] private GameObjectGameEvent onHintSelect;
        
        [Header("SO Data")] 
        [SerializeField] private GameObjectCollection sceneAreaCubes;
        [SerializeField] private GameObjectCollection matchAreaCubes;

        private List<GameObject> _targetCubes = new List<GameObject>();
        
        private void OnEnable()
        {
            _targetCubes.Clear();
            
            onHintButtonClick.AddListener(Hint);
        }
        
        private void OnDisable()
        {
            onHintButtonClick.RemoveListener(Hint);
        }

        private void Hint()
        {
            if (sceneAreaCubes.Count == 0) return;

            if (matchAreaCubes.Count == 0)
                Match();
            else
                Highlight();
        }

        private void Highlight()
        {
            // Highlight.
            var firstMatchAreaCubeTexture = matchAreaCubes[0].GetComponent<CubeRenderController>().Texture;
            foreach (var cube in sceneAreaCubes)
            {
                var renderController = cube.GetComponent<CubeRenderController>();
                if (firstMatchAreaCubeTexture == renderController.Texture) renderController.Mark();
            }
        }

        private void Match()
        {
            _targetCubes.Clear();
            
            _targetCubes.Add(sceneAreaCubes[0]);

            var firstSceneAreaCubeTexture = sceneAreaCubes[0].GetComponent<CubeRenderController>().Texture;

            // Find other instances as well.
            for (int i = 1; i < sceneAreaCubes.Count; i++)
            {
                var cube = sceneAreaCubes[i];
                var renderController = cube.GetComponent<CubeRenderController>();
                if (firstSceneAreaCubeTexture == renderController.Texture)
                    _targetCubes.Add(cube);
            }
        
            Select();
        }

        private void Select()
        {
            var target = _targetCubes[0];
            onHintSelect.Raise(target);
            
            _targetCubes.RemoveAt(0);

            if(_targetCubes.Count > 0) DOVirtual.DelayedCall(selectionInterval, Select);
        }
    }
}