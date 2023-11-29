//SPDX-License-Identifier: Unlicense

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tiger.Audio.Editor
{
	internal static class AssetCreationMenus
	{
		[MenuItem("Assets/Create/Sound/Sound Effect", false)]
		public static void CreateSoundEffect()
		{
			var targets = Selection.GetFiltered<AudioClip>(SelectionMode.Assets);
			Array.Sort(targets, (clip1, clip2) => string.Compare(clip1.ToString(), clip2.ToString(), StringComparison.Ordinal));

			var asset = ScriptableObject.CreateInstance<SoundEffect>();
			asset.clips = new List<AudioClip>(targets);

			const string fileName = "New Sound Effect.asset";
			ProjectWindowUtil.CreateAsset(asset, fileName);
		}

		[MenuItem("Assets/Create/Sound/Composite Effect", false)]
		public static void CreateCompositeEffect()
		{
			var targets = Selection.GetFiltered<AudioEvent>(SelectionMode.Assets);
			Array.Sort(targets, (clip1, clip2) => string.Compare(clip1.ToString(), clip2.ToString(), StringComparison.Ordinal));

			var asset = ScriptableObject.CreateInstance<CompositeEffect>();
			asset.simultaneous = new List<AudioEvent>(targets);

			const string fileName = "New Composite Effect.asset";
			ProjectWindowUtil.CreateAsset(asset, fileName);
		}
	}
}