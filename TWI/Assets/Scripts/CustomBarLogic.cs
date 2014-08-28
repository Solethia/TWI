using UnityEngine;
using System.Collections;

public class CustomBarLogic : MonoBehaviour {
	
	public float MaxWidth = 200;
	public float OverXTime = 0.1f;
	
	private float _timeMultiplier;
	private float _scrollSpeed;
	private float _newWidth;
	private Rect _newWidthAsRect;
	private float _target = 0;
	private bool _changeDetected = false;
	
	void start()
	{
		//Hack to avoid dividing by 0
		if (OverXTime == 0) OverXTime = 0.1f; Debug.LogWarning("Avoiding division by zero. OverXTime set to 0.1f.");
		// 0.1 * 10 = 1 | 1 / 0.1 = 10
		_timeMultiplier = 1.0f / OverXTime;
	}
	
	public void NewTarget(float percentage)
	{
		_target = percentage * MaxWidth;
		_changeDetected = true;
		//Calc new ScrollSpeed 0.1 * 10 = 1 | 1 / 0.1 = 10
		if (_timeMultiplier == 0) { _timeMultiplier = 1.0f / OverXTime; }
		_scrollSpeed = (_target - guiTexture.pixelInset.width) * _timeMultiplier *  Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_changeDetected)
		{
			float difference = _target - guiTexture.pixelInset.width;
			if (Mathf.Abs (_scrollSpeed) < Mathf.Abs (difference)) _newWidth = guiTexture.pixelInset.width + _scrollSpeed;
			else {_newWidth = _target; _changeDetected = false;}
			_newWidthAsRect = new Rect(guiTexture.pixelInset.x, guiTexture.pixelInset.y, _newWidth, guiTexture.pixelInset.height);
			guiTexture.pixelInset = _newWidthAsRect;
			
		}
	}
	
	
}
