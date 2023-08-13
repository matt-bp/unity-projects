using Simulation;
using UnityEngine;

public class UpdateClothVertex : MonoBehaviour
{
    public MassSpringCloth cloth;
    public int vertexToUpdate;
    public bool Updating { get; set; }
    
    private void Update()
    {
        if (!Updating) return;

        cloth.UpdatePose(vertexToUpdate, transform.position);
    }
}