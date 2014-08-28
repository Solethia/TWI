using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		Invoke("GoToMainMenu", 51);
	}
	
	private void GoToMainMenu()
	{
		Application.LoadLevel(6);
	}
}
