using Tiger.Events;
using Tiger.Events.Concrete;
using UnityEngine;

namespace Features.Space
{
    [RequireComponent(typeof(Light))]
    public class DependentSunlight : DataChannelResponder<BoolChannel, bool>
    {
        protected override void OnEvent(bool sunshine)
        {
            GetComponent<Light>().enabled = sunshine;
        }
    }
}
