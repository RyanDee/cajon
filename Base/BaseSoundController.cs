using UnityEngine;
using System.Collections;

public class BaseSoundController : MonoBehaviour {

	public static BaseSoundController Instance;

	public AudioClip[] GameSounds;

	private int _totalSounds;
	private ArrayList _soundObjectList;
	private SoundEffect tmpSoundOjb;

	public float volume = 1;
	public string GamePrefsName = "DefaultGame";

	public void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		// We will grab the volume from PlayerPrebs when this scripts first starts
		volume = PlayerPrefs.GetFloat(GamePrefsName + "_SFXVol");
		Debug.Log("BaseSoundController gets volume from prefs");
		_soundObjectList = new ArrayList();

		// Make sound object for all of the sounds in GameSounds array
		foreach (AudioClip item in GameSounds) {
			tmpSoundOjb = new SoundEffect(item, item.name, volume);
			_soundObjectList.Add(tmpSoundOjb);
			_totalSounds++;
		}
	}

	public void PlaySoundByIndex(int indexNumber, Vector3 position)
	{
		// Make sure we're not trying to play a sound indexed higher than exists in the array
		if(indexNumber > _soundObjectList.Count)
		{
			Debug.LogWarning("BaseSoundController > Trying to do PlaySoundByIndex with invalid index number");
			indexNumber = _soundObjectList.Count - 1;
		}
		tmpSoundOjb = (SoundEffect)_soundObjectList[indexNumber];
		tmpSoundOjb.PlaySound(position);
	}

}
