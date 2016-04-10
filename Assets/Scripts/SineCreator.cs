using UnityEngine;
using System;  // Needed for Math
using UnityEngine.UI;

public class SineCreator : MonoBehaviour {
	// un-optimized version
	public double frequency;
	public double offset;
	public double gain = 0.05;

	private double increment;
	private double modulation;
	private double phase;
	private double phaseOffset;
	private Slider frequencySlider;
	private Slider offsetSlider;
	private double sampling_frequency = 48000;

	void Start()
	{
		frequencySlider = GameObject.Find ("FrequencySlider").GetComponent<Slider> ();
		frequency = frequencySlider.value;
		offsetSlider = GameObject.Find ("OffsetSlider").GetComponent<Slider> ();
		offset = offsetSlider.value;
//		print ("the frequency is " + frequency);
	}

	void Update()
	{
		frequency = frequencySlider.value;
		offset = offsetSlider.value + 440;
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}	
	}

	//saw wave
	void OnAudioFilterRead(float[] data, int channels)
	{
		// update increment in case frequency has changed
		modulation = offset * 2 * Math.PI / sampling_frequency;
		increment = frequency / sampling_frequency;
		for (var i = 0; i < data.Length; i = i + channels)
		{
			phaseOffset = phaseOffset + modulation;
			phase = phase + increment;
			// this is where we copy audio data to make them “available” to Unity
			data[i] = (float) (gain*phase - 0.5f + 0.03f*Math.Sin(phaseOffset));
			// if we have stereo, we copy the mono data to each channel
			if (channels == 2) data[i + 1] = data[i];
			if (phase > 1) phase = 0;
			if (phaseOffset > 2 * Math.PI) phaseOffset = 0;
		}
	}


	//FM synthesis
//	void OnAudioFilterRead(float[] data, int channels)
//	{
//		// update increment in case frequency has changed
//		modulation = offset * 2 * Math.PI / sampling_frequency;
//		increment = frequency * 2 * Math.PI / sampling_frequency;
//		for (var i = 0; i < data.Length; i = i + channels)
//		{
//			phaseOffset = phaseOffset + modulation;
//			phase = phase + increment;
//			// this is where we copy audio data to make them “available” to Unity
//			data[i] = (float)(gain*Math.Sin(phase + .5f*Math.Sin(phaseOffset)));
//			// if we have stereo, we copy the mono data to each channel
//			if (channels == 2) data[i + 1] = data[i];
//			if (phase > 2 * Math.PI) phase = 0;
//			if (phaseOffset > 2 * Math.PI) phaseOffset = 0;
//		}
//	}

	//sine wave
//	void OnAudioFilterRead(float[] data, int channels)
//	{
//		// update increment in case frequency has changed
//		increment = frequency * 2 * Math.PI / sampling_frequency;
//		for (var i = 0; i < data.Length; i = i + channels)
//		{
//			
//			phase = phase + increment;
//			// this is where we copy audio data to make them “available” to Unity
//			data[i] = (float)(gain*Math.Sin(phase));
//			// if we have stereo, we copy the mono data to each channel
//			if (channels == 2) data[i + 1] = data[i];
//			if (phase > 2 * Math.PI) phase = 0;
//		}
//	}
}
