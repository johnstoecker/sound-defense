using UnityEngine;
using System;  // Needed for Math
using System.Collections;

public class Bop : MonoBehaviour {

	public double frequency;
	public double gain = 0.03;

	private double increment;
	private double phase;
	private double sampling_frequency = 48000;

	// Use this for initialization
	void Start () {
		frequency = 440f * (this.transform.position.x / 8f + 1);
	}
	
	// Update is called once per frame
	void Update () {
		if(this.transform.position.y < 0 ) {
			if (this.GetComponentInParent<Oscilloscope>().IsHit(this.transform.position.x)) {
				print("hit!");
			} else {
				print("save!");
			}
			gain = gain - .001;
		}
		if (gain < 0) {
			Destroy (this.gameObject);
		}
	}


	void OnAudioFilterRead(float[] data, int channels)
	{
		// update increment in case frequency has changed
		increment = frequency * 2 * Math.PI / sampling_frequency;
		for (var i = 0; i < data.Length; i = i + channels)
		{
			phase = phase + increment;
			// this is where we copy audio data to make them “available” to Unity
			data[i] = (float)(gain*Math.Sin(phase));
			// if we have stereo, we copy the mono data to each channel
			if (channels == 2) data[i + 1] = data[i];
			if (phase > 2 * Math.PI) phase = 0;
		}
	}



}
