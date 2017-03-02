using UnityEngine;
using System.Collections;

public class SoundEffect {

	public AudioSource Source;
	public GameObject SourceGameObject;
	public Transform SourceTransform;

	public AudioClip audioClip;
	public string name;

	public SoundEffect(AudioClip clip, string name, float volume)
	{
		// In this constructor we create a new audio sources
		// and store the details of the sound itself
		SourceGameObject = new GameObject("AudioSource_" + name);
		SourceTransform = SourceGameObject.transform;
		Source = SourceGameObject.AddComponent<AudioSource>();
		Source.playOnAwake = false;
		Source.clip = clip;
		Source.volume = volume;
		audioClip = clip;
		this.name = name;
	}

	public void PlaySound(Vector3 aPosition)
	{
		SourceTransform.position = aPosition;
		Source.PlayOneShot(audioClip);
	}
}
