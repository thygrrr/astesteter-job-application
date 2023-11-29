using Tiger.Util;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ValidatedBehaviour), true)]
    public class ValidatedBehaviourEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (target is not ValidatedBehaviour validated) return;
            validated.Validate(out var dirty);
            if (dirty) EditorUtility.SetDirty(validated);
        }
    }
}
