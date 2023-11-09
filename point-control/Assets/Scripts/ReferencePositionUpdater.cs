using System;
using Managers;
using UnityEngine;

public class ReferencePositionUpdater : MonoBehaviour
{
    public int index = -1;

    private void Update()
    {
        LoadedManagers.Rpm.UpdateReferencePosition(index, this.gameObject.transform.position.x);
    }
}
