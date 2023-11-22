//SPDX-License-Identifier: Unlicense

using UnityEngine;

namespace Tiger.Events.Concrete
{
    [CreateAssetMenu(fileName="New Vector Channel", menuName="Event/Vector Channel", order=0)]
    public class Vector3Channel : DataChannel<Vector3>
    {
    }
}
