using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	private float cameraSpeed = 5;
	private float horzSpeed;
	private float vertSpeed;
	private Vector3 camTranslate;

	private float mapX;
	private float mapY;
	
	private float minX;
	private float maxX;
	private float minY;
	private float maxY;

	private float vertExtent;
	private float horzExtent;

	private float boundarySize = 2;

	private Vector3 clampPosition;

	private Transform thisTransform;
	
	void Start() 
	{
		thisTransform = transform;

		mapX = GameRef.GridWidth;
		mapY = GameRef.GridHeight;

		vertExtent = Camera.main.camera.orthographicSize;
		horzExtent = vertExtent * Screen.width / Screen.height;
		
		// Calculations assume map is position at the origin
		minX = 0 + horzExtent - boundarySize;
		maxX = mapX - horzExtent + boundarySize;
		minY = 0 + vertExtent - boundarySize;
		maxY = mapY - vertExtent + boundarySize;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Move Camera here
		horzSpeed = Input.GetAxis("Horizontal") * Time.deltaTime * cameraSpeed;
		vertSpeed = Input.GetAxis("Vertical") * Time.deltaTime * cameraSpeed;
		camTranslate = new Vector3(horzSpeed, vertSpeed, 0);
		//Debug.Log (camTranslate);
		thisTransform.Translate(camTranslate);
	}

	private void LateUpdate()
	{
		//Clamp Camera position here
		clampPosition = thisTransform.position;
		clampPosition.x = Mathf.Clamp(clampPosition.x, minX, maxX);
		clampPosition.y = Mathf.Clamp(clampPosition.y, minY, maxY);
		//Debug.Log ("X: (" + minX +", " +maxX + ") Y"+ minY +", " + maxY + ")");
		thisTransform.position = clampPosition;
	}
}
