using Simulation;
using UnityEngine;

public class UpdateClothVertex : MonoBehaviour
{
    public MassSpringCloth cloth;
    public int vertexToUpdate;
    public bool Updating { get; set; }
    
    // Update is called once per frame
    private void Update()
    {
        if (!Updating) return;
        
        Debug.Assert(vertexToUpdate >= 0 && vertexToUpdate < meshFilter.mesh.vertexCount);

        DoUpdate(transform.position);
    }

    private void DoUpdate(Vector3 position)
    {
        var mesh = meshFilter.mesh;
        var newVertices = mesh.vertices;

        newVertices[vertexToUpdate] = meshFilter.gameObject.transform.InverseTransformPoint(position);

        mesh.vertices = newVertices;
        mesh.RecalculateBounds();
    }
}