using UnityEngine;

namespace Devices
{
    public class ColorChangeDevice : BaseDevice
    {
        public override void Operate()
        {
            var random = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            GetComponent<Renderer>().material.color = random;
        }
    }
}
