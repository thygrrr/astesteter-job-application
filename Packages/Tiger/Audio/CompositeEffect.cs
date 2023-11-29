using System.Collections.Generic;
using UnityEngine;

namespace Tiger.Audio
{
	/// <summary>
	/// A composite effect is a collection of AudioEvents that are played simultaneously.
	/// For example, a gun in a FPS has a shot sound, a shell eject sound, a weapon mechanics sound, and a bass rumble / echo.
	/// </summary>
	[CreateAssetMenu(menuName="Audio/Composite Audio")]
	[Icon("Assets/Tiger/Audio/Editor/Icons/sound.png")]
	public class CompositeEffect: ScriptableObject
	{
		[Header("Simultaneous Effects")]
		public List<AudioEvent> simultaneous = new();
		
		//TODO: Find a nicer way to implement this, this clashes with the AudioEvent interface so we use our own.
		public void Play(IList<AudioSource> ringBuffer)
		{
			if (ringBuffer.Count < simultaneous.Count)
			{
				Debug.LogWarning($"Composite effect {name} has more entries than in the audio source buffer. It will interrupt itself.", this);
			}
			
			foreach (var audio in simultaneous) audio.Play(ringBuffer);
		}
		
		public void Play(AudioPool pool) => Play(pool.sources);

		private void OnValidate()
		{
			simultaneous.RemoveAll(s => !s);
		}
	}
}