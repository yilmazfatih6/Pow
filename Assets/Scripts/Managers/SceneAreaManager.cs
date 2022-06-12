using System.Collections.Generic;
using Cube;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(fileName = "SceneAreaManager", menuName = "ScriptableObject/SceneAreaManager", order = 0)]
    public class SceneAreaManager : ScriptableObject
    {
        [Header("SO Data")] 
        [SerializeField] private GameObjectCollection sceneCubes;
        [SerializeField] private Vector3Collection sceneAreaPositions;
        [SerializeField] private GameObjectVariable sceneArea;

        [Header("Listened Events")] 
        [SerializeField] private GameObjectGameEvent onSpawn;
        [SerializeField] private GameObjectGameEvent onAdd;
        [SerializeField] private GameObjectGameEvent onUndo;

        private void OnEnable()
        {
            // Clear collection
            sceneCubes.Clear();
            
            // Add listener to events
            onSpawn.AddListener(Add);
            onAdd.AddListener(Remove);
            onUndo.AddListener(Add);
        }

        private void OnDisable()
        {
            onSpawn.RemoveListener(Add);
            onAdd.RemoveListener(Remove);
            onUndo.RemoveListener(Add);
        }

        private void Add(GameObject newCube)
        {
            sceneCubes.AddUnique(newCube);
            
            SetCubePosition(newCube);
        }

        private void Remove(GameObject newCube)
        {
            sceneCubes.Remove(newCube);
        }

        private void SetCubePosition(GameObject newCube)
        {
            // Set position.
            var randPositionIndex = Random.Range(0, sceneAreaPositions.Count - 1);
            var randPosition = sceneAreaPositions[randPositionIndex];
            
            // Remove from duplicated sceneAreaPositions.
            sceneAreaPositions.RemoveAt(randPositionIndex);
            
            var cubeMovement = newCube.GetComponent<CubeMovementController>();
            cubeMovement.SetInitialPosition(randPosition, sceneArea.Value.transform);
        }

    }
}