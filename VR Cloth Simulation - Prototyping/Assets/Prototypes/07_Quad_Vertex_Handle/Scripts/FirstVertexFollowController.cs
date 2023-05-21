using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototypes._07_Quad_Vertex_Handle.Scripts
{
    [RequireComponent(typeof(MeshFilter))]
    public class FirstVertexFollowController : MonoBehaviour
    {
        public InputActionReference controllerPositionAction;

        [SerializeField]
        private XROrigin xrOrigin;
        private MeshFilter meshFilter;
        
        // Start is called before the first frame update
        void Start()
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        // Update is called once per frame
        void Update()
        {
            var action = controllerPositionAction.action;

            var position = action.ReadValue<Vector3>();
            position.y += xrOrigin.CameraYOffset;

            var mesh = meshFilter.mesh;
            // Local coordinate space
            var newVertices = mesh.vertices;
            
            newVertices[0] = transform.InverseTransformPoint(position);

            mesh.vertices = newVertices;
            mesh.RecalculateBounds();
        }
    }
}
