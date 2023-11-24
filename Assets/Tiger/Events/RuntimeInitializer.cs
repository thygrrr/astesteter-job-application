using UnityEngine;

namespace Tiger.Events
{
    public static class RuntimeInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RuntimeInit()
        {
            foreach (var channel in Resources.FindObjectsOfTypeAll<AbstractChannel>())
            {
                Debug.LogFormat($"Init {channel}", channel);
                channel.Init();
            }
        }
    }
}
