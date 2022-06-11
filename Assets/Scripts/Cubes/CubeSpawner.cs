using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Cubes
{
    public class CubeSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject cubePrefab;
        [SerializeField] private GameObjectCollection sceneItems;
        [SerializeField] private int spawnCount;
        [SerializeField] private List<Texture> textures = new List<Texture>();
    
        private  List<Vector3> _gridPositions = new List<Vector3>();
        private  List<Texture> _cubeTextures = new List<Texture>();
        private  List<Vector3> _cubePositions = new List<Vector3>();

        private void Start()
        {
            AssignGridPositions();
            SetTexturesToSpawn();
            SetCubeSpawnPositions();
            SpawnCubes();
        }

        private void AssignGridPositions()
        {
            var cubeRenderer = cubePrefab.GetComponent<Renderer>();
            var cubeBounds = cubeRenderer.bounds;

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
                        _gridPositions.Add(transform.position + gridPositions);
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
    
        private void SetCubeSpawnPositions()
        {
            for (int i = 0; i < spawnCount; i++)
            {
                var rand = Random.Range(0, _gridPositions.Count - 1);
                _cubePositions.Add(_gridPositions[rand]);
                _gridPositions.RemoveAt(rand);
            }
        }
    
        private void SpawnCubes()
        {
            sceneItems.Clear();

            int textureIndex = 0;
            for (int i = 0; i < _cubePositions.Count; i++)
            {
                var spawned = Instantiate(cubePrefab, transform);
                
                Debug.Log("_cubeTextures[textureIndex] " + _cubeTextures[textureIndex].name);
                spawned.GetComponent<CubeData>().Inject(_cubeTextures[textureIndex], _cubePositions[i], Quaternion.identity);
                
                sceneItems.Add(spawned);

                if (i % 3 == 2)
                {
                    Debug.Log("increase texture index, i : " + i);
                    textureIndex++;
                }
            }
        }
    }
}
