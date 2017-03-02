using UnityEngine;
using System.Collections;

public class AutomaticDestroyObject : MonoBehaviour {

	// Presented by Seconds
	public float DestroyAfterTime;

	// Use this for initialization
	void Start () {
		Invoke("DestroyMe", DestroyAfterTime);
	}
	
	void DestroyMe()
	{
		Destroy(gameObject);
	}
}
