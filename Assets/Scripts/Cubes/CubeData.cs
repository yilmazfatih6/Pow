using UnityEngine;

namespace Cubes
{
    public class CubeData : MonoBehaviour
    {
        [SerializeField] Renderer renderer;
        private Vector3 _lastPosition;
        public Texture Texture => renderer.material.mainTexture;

        public void Inject(Texture texture, Vector3 pos, Quaternion rot)
        {
            renderer.material.mainTexture = texture;
            transform.position = pos;
            transform.rotation = rot;

            _lastPosition = transform.position;
        }
        
    }
}