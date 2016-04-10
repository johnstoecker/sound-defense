using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(RawImage))]

public class Oscilloscope : MonoBehaviour {


	public int width; // texture width 
	public int height; // texture height 
//	public Color backgroundColor = Color.black; 
	public Color waveformColor = Color.green; 
	public Color backgroundColor = Color.clear;
	public int size; // size of sound segment displayed in texture
	public float frequency;
	public float offset;
	public GameObject bop;

	private Slider frequencySlider;
	private Slider offsetSlider;
	private Color[] blank; // blank image array 
	private Texture2D texture; 
	private Canvas canvasComponent;

	IEnumerator Start ()
	{

		frequencySlider = GameObject.Find ("FrequencySlider").GetComponent<Slider> ();
		offsetSlider = GameObject.Find ("OffsetSlider").GetComponent<Slider> ();
		frequency = frequencySlider.value;
		offset = offsetSlider.value;

		width = 450;
		height = 150;

		// create the texture and assign to the guiTexture: 
		texture = new Texture2D (width, height);

		GetComponent<RawImage>().texture = texture;
		print ("hello world");

		// create a 'blank screen' image 
		blank = new Color[width * height]; 

		for (int i = 0; i < blank.Length; i++) { 
			blank [i] = backgroundColor; 
		} 

		while (true) {
			CreateNewBop ();
			yield return new WaitForSeconds (1.0f); 
		} 
//		canvasComponent = GameObject.Find("Canvas").GetComponent<Canvas>();
	}

	void CreateNewBop()
	{
		float x = Random.Range (0, 8);
		GameObject bopthing = (GameObject)Instantiate(bop, new Vector3(x, 8, 1), Quaternion.identity);
		//			bopthing.parent = this.gameObject;
		//			bopthing.transform.SetParent(this.gameObject.transform, false);
		//			bopthing.transform.SetParent(canvasComponent.gameObject.transform, false);

	}

	void Update() 
	{
		if (frequency != frequencySlider.value || offset != offsetSlider.value){
			frequency = frequencySlider.value;
			offset = offsetSlider.value;
			UpdateFrequency();
//			print ("the new frequency is " + frequency);
		}
	}

	void UpdateFrequency ()
	{ 
		// clear the texture 
//		texture.SetPixels (blank, 0); 

		// draw the waveform 
		//TODO: store in array, use setpixels to set all at once
		//TODO: or maybe do curved line interpolations'n'stuff
		Color[] newPixels = new Color[width*height];
//		newPixels = blank.Clone;
		blank.CopyTo (newPixels, 0);

		for (int i = 0; i < (width-2); i++) { 
			int x = i;
			int y = (int)((height-2) * (Mathf.Sin((i - offset) * frequency * Mathf.PI /(width * 220f))/2f + 0.5f));
			x = Mathf.Max(Mathf.Min (x, width-3), 2);
			y = Mathf.Max(Mathf.Min (y, height - 3), 2);
			newPixels[y*width + x] = waveformColor;
			newPixels[y*width + x+1] = waveformColor;
			newPixels[y*width + x+2] = waveformColor;
			newPixels[(y+1)*width + x] = waveformColor;
			newPixels[(y+2)*width + x] = waveformColor;
//			texture.SetPixel (x, y, waveformColor);
//			texture.SetPixel (x+1, y, waveformColor);
//			texture.SetPixel (x, y+1, waveformColor);
//			texture.SetPixel (x+2, y, waveformColor);
//			texture.SetPixel (x, y+2, waveformColor);
		} // upload to the graphics card 

		texture.SetPixels (newPixels);
		texture.Apply (); 
	} 
}
