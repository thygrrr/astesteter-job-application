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

			if (Application.isPlaying)
			{
				EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);

				// Use reflection to get the generic type of the target
				System.Type targetType = target.GetType();

				// Now we need to find the Emit method and the defaultValue field
				var emitMethod = targetType.GetMethod("Emit");
				var defaultValueField = targetType.GetField("defaultValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

				if (emitMethod != null && defaultValueField != null)
				{
					var defaultValue = defaultValueField.GetValue(target);

					if (GUILayout.Button("Emit Default"))
					{
						// Invoke the Emit method with the defaultValue
						emitMethod.Invoke(target, new[] {defaultValue});
					}
				}

				if (GUILayout.Button("Emit Custom"))
				{
					// Here you would need to implement a way to let the user input a custom value to emit
					// This could be through a serialized property or a EditorGUI field
				}

				EditorGUI.EndDisabledGroup();
			}
		}
	}
}