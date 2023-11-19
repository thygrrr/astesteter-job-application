//SPDX-License-Identifier: Unlicense
using Tiger.Events;
using UnityEngine;

namespace Channels.Concrete
{
    [CreateAssetMenu(fileName="New Vector Channel", menuName="Event/Vector Channel", order=0)]
    public class Vector3Channel : DataChannel<Vector3>
    {
    }
}
