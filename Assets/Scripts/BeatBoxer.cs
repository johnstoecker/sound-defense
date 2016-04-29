using UnityEngine;
using System.Collections;

public class BeatBoxer : MonoBehaviour {

	public AudioSource normalKick;
	public AudioSource synthKick;


	private Beat[] beats;

	//Beat stores:
	//what kind of animation, if any, to play on oscilloscope

	private int currentBeat;
	private int maxBeats = 8;

	// Use this for initialization
	void Start () {
		currentBeat = 0;
		InvokeRepeating ("nextBeat", 0, 0.25f);
	}

	void nextBeat () {
		if (++currentBeat == maxBeats) {
			currentBeat = 0;
		}

		if (currentBeat == 0) {
			normalKick.Play ();
		}
	}


}
