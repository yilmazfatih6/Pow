using System.Collections.Generic;
using Cube;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace SceneArea
{
    public class CubeSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject cubePrefab;
        [SerializeField] private List<Texture> textures = new List<Texture>();
        
        [Header("Raised Events")]
        [SerializeField] private GameObjectGameEvent onSpawn;
        
        [Header("SO Data")]
        [SerializeField] private Vector3Collection sceneAreaPositions;

        private readonly int _spawnCount = 42;
        private readonly List<Texture> _cubeTextures = new List<Texture>();

        private void Start()
        {
            AssignGridPositions();
            SetTexturesToSpawn();
            SpawnCubes();
        }

        private void AssignGridPositions()
        {
            var cubeRenderer = cubePrefab.GetComponent<Renderer>();
            var cubeBounds = cubeRenderer.bounds;

            sceneAreaPositions.Clear();
            
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    for (var k = 0; k < 4; k++)
                    {
                        Vector3 gridPositions;
                        gridPositions.x = (cubeBounds.extents.x * 2) * i;
                        gridPositions.y = (cubeBounds.extents.y * 2) * j;
                        gridPositions.z = (cubeBounds.extents.z * 2) * k;
                        sceneAreaPositions.Add(gridPositions);
                    }
                }
            }
        }

        private void SetTexturesToSpawn()
        {
            for (int i = 0; i < _spawnCount / 3; i++)
            {
                int textureIndex = Random.Range(0, textures.Count - 1);
                Texture texture = textures[textureIndex];
                _cubeTextures.Add(texture);
                textures.Remove(texture);
            }
        }
    
        private void SpawnCubes()
        {
            int textureIndex = 0;
            for (int i = 0; i < _spawnCount; i++)
            {
                var spawned = Instantiate(cubePrefab);
                spawned.GetComponent<CubeRenderController>().Inject(_cubeTextures[textureIndex]);

                onSpawn.Raise(spawned);

                if (i % 3 == 2)
                    textureIndex++;
            }
        }
    }
}
