using UnityEngine;

namespace TapAndRun.Audio
{
	[System.Serializable]
	public class Sound {

		[field:SerializeField] public string SoundId { get; private set; }

		[field: SerializeField, Range(0f, 1f)] public float Volume { get; private set; }
		[field:SerializeField] public AudioClip Clip { get; private set; }
	}
}
