using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototypes._05_Quad_Vertex_Follow_Controller.Scripts
{
    [RequireComponent(typeof(MeshFilter))]
    public class FirstVertexFollowController : MonoBehaviour
    {
        public GameObject controller;

        private MeshFilter meshFilter;
        
        // Start is called before the first frame update
        void Start()
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        // Update is called once per frame
        void Update()
        {
            var position = controller.transform.position;

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
