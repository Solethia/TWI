using UnityEngine;
using System.Collections;
using System.Linq;

public class FadeOut : MonoBehaviour {

	private bool fadeIn = false;

	[SerializeField]
	private float activateTime;

	private float fadeTime = 3.0f;
	private float startTime = 0.0f;

	private SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Awake () 
	{
		spriteRenderer = renderer as SpriteRenderer;
	}

	private void Start () 
	{
		Invoke("FadeOutStart", activateTime);
		Destroy(gameObject, activateTime + fadeTime);
	}
	private void Update()
	{
		if (fadeIn) {
			float timePassed = Time.time - startTime;
			float Fade =  Mathf.Clamp(((fadeTime - timePassed)/fadeTime), 0.0f, 1.0f);
			spriteRenderer.color = new Color(1f, 1f, 1f, Fade);
			Debug.Log ("Fade: " + Fade);
		}
	}
	
	private void FadeOutStart()
	{
		Debug.Log ("start");
		fadeIn = true;
		startTime = Time.time;
	}
}
