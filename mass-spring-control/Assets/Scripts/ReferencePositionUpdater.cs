using Managers;
using UnityEngine;

public class ReferencePositionUpdater : MonoBehaviour
{
    public int index = -1;

    private void Update()
    {
        var newReferencePosition =
            new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        LoadedManagers.ReferencePositionManager.UpdateReferencePosition(index, newReferencePosition);
    }
}
