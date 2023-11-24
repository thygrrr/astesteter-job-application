using UnityEngine;
using UnityEngine.Scripting;

namespace Tiger.Events
{
    [Preserve]
    public abstract class AbstractDataChannel : ScriptableObject
    {
        protected internal abstract void Init();
    }
}