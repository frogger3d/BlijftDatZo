using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForceJoint : MonoBehaviour {

	private const float ForceRange = 5;
    private const float ForceMultiplier = 20;

	private GameController gameController;

	// Use this for initialization
	void Start () {
		this.gameController = GameSceneController.Instance.GameController;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
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

		// apply force to all particle gameobjects
		List<ParticleBase> particles = this.gameController.AllActiveParticles;
		if (particles.Count > 0) 
		{
			//Debug.Log("#p: " + particles.Count);
			foreach(ParticleBase p in particles)
			{
				// x positief als particle rechts van dit ding
				// y positief als particle boven dit ding
				Vector2 diff = p.rigidbody2D.position - this.gameObject.rigidbody2D.position;
				float distSquared = diff.sqrMagnitude;
				if (distSquared < RangeSquared)
				{
					float multiplier = 1f - (Mathf.Sqrt(distSquared) / ForceRange);
					Vector2 force = diff * multiplier * ForceMultiplier;
					//Vector2 force = diff * 20f;
					p.rigidbody2D.AddForce(force, ForceMode2D.Force);
					//Debug.Log("applied force " + force);
				}
			}
		}
	}

	private float RangeSquared
	{
		get { return Mathf.Pow(ForceRange, 2); }
	}
}
