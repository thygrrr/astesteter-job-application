using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Tiger.Events.Editor
{
	[CustomEditor(typeof(DataChannel<>), true)]
	public class DataChannelEditor : UnityEditor.Editor
	{

		public override void OnInspectorGUI()
		{
			DrawHeader();
			
			DrawDefaultInspector();

				EditorGUI.BeginDisabledGroup(Application.isPlaying || serializedObject.isEditingMultipleObjects);

				// Use reflection to get the generic type of the target
				var targetType = target.GetType();

				// Now we need to find the Emit method and the defaultValue field
				var emitMethod = targetType.GetMethod("Emit");
				var defaultValueField = targetType.GetField("defaultValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

				if (emitMethod != null && defaultValueField != null)
				{
					var defaultValue = defaultValueField.GetValue(target);

					if (GUILayout.Button("Emit Default"))
					{
						// Invoke the Emit method with the defaultValue
						emitMethod.Invoke(target, new[] {defaultValue, this});
					}
				}
				EditorGUI.EndDisabledGroup();
		}
	}
}