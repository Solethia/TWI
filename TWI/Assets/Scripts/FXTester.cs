using UnityEngine;
using System.Collections;

public class FXTester : MonoBehaviour {

	[SerializeField]
	private GameObject EffectToSpawn;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			spawnPosition.z = 9;
			GameObject.Instantiate(EffectToSpawn, spawnPosition, Quaternion.identity);
		}
	}
}
