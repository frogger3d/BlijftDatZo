using UnityEngine;
using System.Collections;

public class ForceJoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		// (0, 0) - (screen.width, screen.height)
		var mousepos = Input.mousePosition;

		// normalized (0, 1)
		Vector2 normalized = new Vector2 (mousepos.x / Screen.width, mousepos.y / Screen.height);

		// normalized (-1, - 1)
		normalized = normalized * 2 - new Vector2(1, 1);

		// adjust position of the game object to correspond to mouse position
		Camera cam = Camera.main;
		float verticalHalfSize = cam.orthographicSize;
		float horizontalHalfSize = cam.aspect * verticalHalfSize;

		var newx = horizontalHalfSize * normalized.x;
		var newy = verticalHalfSize * normalized.y;
		this.gameObject.transform.position = new Vector2 (newx, newy);
	}
}
