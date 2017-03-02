using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

	private float _volume;
	public string gamePrefsName = "DefaultGame";

	public AudioClip music;
	public bool loopMusic;

	private AudioSource _source;
	private GameObject _sourceGO;

	private int _fadeState;
	private int _targetFadeState;

	private float _volumeON;
	private float _targetVolume;

	public float fadeTime = 15f;
	public bool shouldFadeInAtStart = true;

	void Start()
	{
		// We will grab the volume from PlayerPrefs when this script first starts
		_volumeON = PlayerPrefs.GetFloat(gamePrefsName + "_MusicVol");

		// Create a Game Object and add an AudioSource to it, to play music on
		_sourceGO = new GameObject("Music_AudioSource");
		_source = _sourceGO.AddComponent<AudioSource>();
		_source.name = "MusicAudioSource";
		_source.playOnAwake = true;
		_source.clip = music;
		_source.volume = _volume;

		// The script will automatically fade in if this is set
		if(shouldFadeInAtStart)
		{
			_fadeState = 0;
			_volume = 0;
		}
		else
		{
			_fadeState = 1;
			_volume = _volumeON;
		}

		// Set the default values
		_targetFadeState = 1;
		_targetVolume = _volumeON;
		_source.volume = _volume;
	}

	void Update()
	{
		// If the AudioSource is not playeing and it's suppossed to loop, play it again
		if(!_source.isPlaying && loopMusic)
		{
			_source.Play();
		}

		// Deal with volume fade in/out
		if(_fadeState != _targetFadeState)
		{
			if(_targetFadeState == 1)
			{
				if(_volume == _volumeON)
				{
					_fadeState = 1;
				}
			}
			else
			{
				if(_volume == 0)
				{
					_fadeState = 0;
				}
			}
			_volume = Mathf.Lerp(_volume, _targetVolume, Time.deltaTime * fadeTime);
			_source.volume = _volume;
		}

	}

	public void FadeIn (float fadeAmount)
	{
		_volume = 0;
		_fadeState = 0;
		_targetFadeState = 1;
		_targetVolume = _volumeON;
		fadeTime = fadeAmount;
	}

	public void FadeOut(float fadeAmount)
	{
		_volume = _volumeON;
		_fadeState = 1;
		_targetFadeState = 0;
		_targetVolume = 0;
		fadeTime = fadeAmount;
	}


}
