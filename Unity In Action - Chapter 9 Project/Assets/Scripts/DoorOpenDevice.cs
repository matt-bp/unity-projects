using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenDevice : MonoBehaviour
{
    [SerializeField] private Vector3 offsetPosition;
    private bool _open;

    public void Operate()
    {
        if (_open)
        {
            var pos = transform.position - offsetPosition;
            transform.position = pos;
        }
        else
        {
            var pos = transform.position + offsetPosition;
            transform.position = pos;
        }

        _open = !_open;
    }

    public void Activate()
    {
        if (!_open)
        {
            var pos = transform.position + offsetPosition;
            transform.position = pos;
            _open = true;
        }
    }

    public void Deactivate()
    {
        if (_open)
        {
            var pos = transform.position - offsetPosition;
            transform.position = pos;
            _open = false;
        }
    }
}
