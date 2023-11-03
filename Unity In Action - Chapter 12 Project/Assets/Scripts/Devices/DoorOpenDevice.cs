using UnityEngine;

namespace Devices
{
    public class DoorOpenDevice : BaseDevice
    {
        [SerializeField] private Vector3 offsetPosition;
        private bool _open;

        public override void Operate()
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
}
