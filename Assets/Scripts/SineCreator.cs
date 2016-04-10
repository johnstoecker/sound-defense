using UnityEngine;
using System;  // Needed for Math
using UnityEngine.UI;

enum waveType{
	Sine = 0,
	Saw = 1,
	Synth = 2
}

public class SineCreator : MonoBehaviour {
	// un-optimized version
	public double frequency;
	public double offset;
	public double gain = 0.05;
	private waveType soundWave;

	private double increment;
	private double modulation;
	private double phase;
	private double phaseOffset;
	[SerializeField] private Slider frequencySlider;
	[SerializeField] private Slider offsetSlider;
	[SerializeField] private AudioLowPassFilter lowPassFilter;
	private double sampling_frequency = 48000;

	void Start()
	{
		frequency = frequencySlider.value;
		print (frequency);
		offset = offsetSlider.value;
		soundWave = waveType.Saw;
	}

	void Update()
	{
		frequency = frequencySlider.value;
		offset = offsetSlider.value;
		if (soundWave == waveType.Saw) {
			lowPassFilter.cutoffFrequency = (float)(offset * 50) + 2000;
		}
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}	
	}

	void OnAudioFilterRead(float[] data, int channels)
	{
		if (soundWave == waveType.Sine) {
			playSine (data, channels);
		} else if (soundWave == waveType.Saw) {
			playSaw (data, channels);
		} else if (soundWave == waveType.Synth) {
			playSynth(data, channels);
		}
	}


	//saw wave
	void playSaw(float[] data, int channels)
	{
		// update increment in case frequency has changed
		increment = frequency / sampling_frequency;
		for (var i = 0; i < data.Length; i = i + channels)
		{
			phase = phase + increment;
			// this is where we copy audio data to make them “available” to Unity
			data[i] = (float) (gain*phase - 0.5f);
			// if we have stereo, we copy the mono data to each channel
			if (channels == 2) data[i + 1] = data[i];
			if (phase > 1) phase = 0;
		}
	}


	//FM synthesis
	void playSynth(float[] data, int channels)
	{
		// update increment in case frequency has changed
		modulation = offset * 2 * Math.PI / sampling_frequency;
		increment = frequency * 2 * Math.PI / sampling_frequency;
		for (var i = 0; i < data.Length; i = i + channels)
		{
			phaseOffset = phaseOffset + modulation;
			phase = phase + increment;
			// this is where we copy audio data to make them “available” to Unity
			data[i] = (float)(gain*Math.Sin(phase + .5f*Math.Sin(phaseOffset)));
			// if we have stereo, we copy the mono data to each channel
			if (channels == 2) data[i + 1] = data[i];
			if (phase > 2 * Math.PI) phase = 0;
			if (phaseOffset > 2 * Math.PI) phaseOffset = 0;
		}
	}

	//sine wave
	void playSine(float[] data, int channels)
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
