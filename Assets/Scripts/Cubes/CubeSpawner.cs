using System.Collections.Generic;
using UnityEngine;

namespace Cubes
{
    public class CubeSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject cubePrefab;
        [SerializeField] private int spawnCount;
        [SerializeField] private List<Texture> textures = new List<Texture>();
    
        private readonly List<GameObject> _cubes = new List<GameObject>();
        private readonly List<Vector3> _gridPositions = new List<Vector3>();
        private readonly List<Texture> _cubeTextures = new List<Texture>();
        private readonly List<Vector3> _cubePositions = new List<Vector3>();

        private void Start()
        {
            AssignCubePositions();
            SetTexturesToSpawn();
            SetCubeSpawnPositions();
            SpawnCubes();
        }

        private void AssignCubePositions()
        {
            var cubeRenderer = cubePrefab.GetComponent<Renderer>();
            var cubeBounds = cubeRenderer.bounds;

            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    for (var k = 0; k < 4; k++)
                    {
                        Vector3 cubePosition;
                        cubePosition.x = (cubeBounds.extents.x * 2) * i;
                        cubePosition.y = (cubeBounds.extents.y * 2) * j;
                        cubePosition.z = (cubeBounds.extents.z * 2) * k;
                        _gridPositions.Add(transform.position + cubePosition);
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
            int textureIndex = 0;
            for (int i = 0; i < _cubePositions.Count; i++)
            {
                var spawned = Instantiate(cubePrefab, _cubePositions[i], Quaternion.identity, transform);
                _cubes.Add(spawned);
                spawned.GetComponent<Renderer>().material.mainTexture = _cubeTextures[textureIndex];

                if (i != 0 && i % 3 == 0)
                    textureIndex++;
            }
        }
    }
}
