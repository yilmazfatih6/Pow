using UnityEngine;

namespace Cube
{
    public class CubeRenderController : MonoBehaviour
    {
        [SerializeField] private CubeMovementController cubeMovementController;
        [SerializeField] private Selectable selectable;
        [SerializeField] private new Renderer renderer;
        [SerializeField] private GameObject glow;
        public Texture Texture => renderer.material.mainTexture;

        private void OnEnable()
        {
            cubeMovementController.OnMoveToOriginalPositionComplete += SetDefaultLayer;
            selectable.OnFocus += EnableGlow;
            selectable.OnUnfocus += DisableGlow;
            selectable.OnClick += DisableGlow;
            selectable.OnClick += SetUILayer;
        }

        private void OnDisable()
        {
            cubeMovementController.OnMoveToOriginalPositionComplete -= SetDefaultLayer;
            selectable.OnFocus -= EnableGlow;
            selectable.OnUnfocus -= DisableGlow;
            selectable.OnClick -= DisableGlow;
            selectable.OnClick -= SetUILayer;
        }

        public void Inject(Texture texture)
        {
            renderer.material.mainTexture = texture;
        }

        public void OnSelect()
        {
            glow.SetActive(true);
        }

        private void EnableGlow()
        {
            glow.SetActive(true);
        }

        private void DisableGlow()
        {
            glow.SetActive(false);
        }

        private void SetUILayer()
        {
            foreach (var t in GetComponentsInChildren<Transform>())
                t.gameObject.layer = 5;
        }

        private void SetDefaultLayer()
        {
            foreach (var t in GetComponentsInChildren<Transform>())
                t.gameObject.layer = 0;
        }
    }
}