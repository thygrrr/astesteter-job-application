using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Tiger.Events.Editor
{
	[CustomEditor(typeof(DataChannel<>), true)]
	public class DataChannelEditor : UnityEditor.Editor
	{
		private bool _foldout = true;
		public override void OnInspectorGUI()
		{

			// Use reflection to get the generic type of the target
			DrawDefaultInspector();
			EditorGUILayout.Space();
			var targetType = target.GetType();
			GUILayout.Label(target.name, EditorStyles.whiteLabel);
			EditorGUILayout.LabelField($"Underlying Type", targetType.ToString());
			
			EditorGUI.BeginDisabledGroup(!Application.isPlaying || serializedObject.isEditingMultipleObjects);
			EditorGUILayout.Space();
			EditorGUILayout.BeginFoldoutHeaderGroup(_foldout, "Emit Value (Runtime Only)");
			ListEnumeratedStates(targetType);
			EditorGUILayout.EndFoldoutHeaderGroup();
			EditorGUI.EndDisabledGroup();
		}

		private void ListEnumeratedStates(System.Type targetType)
		{
			// Now we need to find the Emit method and the defaultValue field
			var emitMethod = targetType.GetMethod("Emit");
			var enumerateMethod = targetType.GetMethod("Enumerate");
			
			var defaultValueField = targetType.GetField("defaultValue", BindingFlags.Instance | BindingFlags.NonPublic);
			var defaultValue = defaultValueField?.GetValue(target);

			if (emitMethod == null || enumerateMethod == null) return;
			
			// Invoke the methods if they exist
			object[] arguments = {null, null};

			//Nothing?
			if (!(bool) enumerateMethod.Invoke(target, arguments)) return;
			
			// After invocation, the array contains the out parameter values.
			string[] enumNames = (string[]) arguments[0];
			Array enumValues = (Array) arguments[1];

			for (var i = 0; i < enumNames.Length; i++)
			{
				var enumName = enumNames[i];
				if (GUILayout.Button($"Emit {enumName}"))
				{
					// Invoke the Emit method with the enum value
					emitMethod.Invoke(target, new[] {enumValues.GetValue(i), this});
				}
			}
		}
	}
}