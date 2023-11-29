using UnityEngine;

namespace Tiger.Util
{
    public abstract class ValidatedBehaviour : MonoBehaviour
    {
        public abstract void Validate(out bool dirty);
    }
}
