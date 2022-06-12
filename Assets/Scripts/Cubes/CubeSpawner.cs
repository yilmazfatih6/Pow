using System.Collections.Generic;
using Cube;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Cubes
{
    public class CubeSpawner : MonoBehaviour
    {
        [SerializeField] private Transform center;
        [SerializeField] private GameObject cubePrefab;
        [SerializeField] private int spawnCount;
        [SerializeField] private List<Texture> textures = new List<Texture>();
        [SerializeField] private GameObjectGameEvent onSpawn;
        [SerializeField] private Vector3Collection sceneAreaPositions;
    
        private  List<Texture> _cubeTextures = new List<Texture>();
        private  List<Vector3> _cubePositions = new List<Vector3>();

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
            for (int i = 0; i < spawnCount / 3; i++)
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
            for (int i = 0; i < spawnCount; i++)
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
