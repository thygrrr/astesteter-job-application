using Tiger.Events;
using UnityEngine;

namespace Features.Game
{
    [CreateAssetMenu(fileName="New Vector Channel", menuName="Event/Vector Channel", order=0)]
    public class VectorChannel : DataChannel<Vector3>
    {
    }
}
