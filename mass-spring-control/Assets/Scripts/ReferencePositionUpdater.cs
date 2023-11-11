using Managers;
using UnityEngine;

public class ReferencePositionUpdater : MonoBehaviour
{
    public int index = -1;

    private void Update()
    {
        // I need to keep track of the deltas in x, y, z for this reference position.
        // I then need to update the reference position with these deltas
        // Get the reference position for this index.
        // Apply deltas
        // Update reference position for this index with applied deltas

        // var newReferencePosition =
        //     new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        // LoadedManagers.ReferencePositionManager.UpdateReferencePosition(index, newReferencePosition);
    }
}
