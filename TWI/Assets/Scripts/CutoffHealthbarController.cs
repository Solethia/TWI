using UnityEngine;
using System.Collections;

public class CutoffHealthbarController : MonoBehaviour {
	
	private float health;
	public float Health
	{
		set 
		{
			health = value;
			renderer.material.SetFloat("_CutOff", health);
		}
	}
	// Update is called once per frame
	void Update () 
	{

	}
}
