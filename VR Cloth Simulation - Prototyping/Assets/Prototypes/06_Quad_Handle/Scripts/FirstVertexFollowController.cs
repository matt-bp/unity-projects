using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototypes._06_Quad_Handle.Scripts
{
    [RequireComponent(typeof(MeshFilter))]
    public class FirstVertexFollowController : MonoBehaviour
    {
        public InputActionReference controllerPositionAction;

        [SerializeField]
        private GameObject cameraFloorOffsetObject;
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
            var offset = cameraFloorOffsetObject.transform.position.y;
            position.y += offset;

            var mesh = meshFilter.mesh;
            // Local coordinate space
            var newVertices = mesh.vertices;
            
            // How do I move the position into local space?
            newVertices[0] = transform.InverseTransformPoint(position);

            mesh.vertices = newVertices;
            mesh.RecalculateBounds();
        }
    }
}
