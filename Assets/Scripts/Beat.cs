using UnityEngine;
using System.Collections;

public class Beat : MonoBehaviour {

	//sound clip to play
	public AudioSource sound;
	//how many seconds in future after measure start to play
	public float time;

	// Use this for initialization
	void Start () {
		Invoke ("Play", time);
	}
	

	void play () {
		sound.Play();
	}

//	// Update is called once per frame
//	void Update () {
//	
//	}
}
