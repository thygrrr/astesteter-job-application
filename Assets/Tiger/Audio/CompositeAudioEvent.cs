using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tiger.Audio
{
	[CreateAssetMenu(menuName="Effect/Composite Audio")]
	[Icon("Assets/Tiger/Audio/Editor/Icons/sound.png")]
	public class CompositeAudioEvent : AudioEvent
	{
		[Serializable]
		public struct CompositeEntry
		{
			public AudioEvent item;
			public float weight;
		}

		public CompositeEntry[] entries;

		public override void Play(AudioSource source)
		{
			float totalWeight = 0;
			for (var i = 0; i < entries.Length; ++i)
			{
				totalWeight += entries[i].weight;			
			}

			var pick = Random.Range(0, totalWeight);
			for (var i = 0; i < entries.Length; ++i)
			{
				if (pick > entries[i].weight)
				{
					pick -= entries[i].weight;
					continue;
				}

				entries[i].item.Play(source);
				return;
			}
		}
	}
}