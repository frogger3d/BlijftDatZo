using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JointSpawner : MonoBehaviour {

    BodyService bodyService;

    List<GameObject> bodies = new List<GameObject>();

	// Use this for initialization
	void Start () {
        this.bodyService = new BodyService();

        for(int userIndex = 0; userIndex < 2; userIndex++)
        {
            var bodyObject = new GameObject();
            bodyObject.transform.parent = this.transform;
            

            this.bodies.Add(bodyObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
        this.bodyService.Update();

        int width = 540;
        int height = 440;

        for(int i = 0; i < this.bodies.Count; i++)
        {
            var position = this.bodyService.GetJointPosition(0, Windows.Kinect.JointType.Head);
            if(position.HasValue)
            {
                this.bodies[i].SetActive(true);
                var x = (position.Value.x / width) * 2 - 1;
                var y = (position.Value.y / height) * 2 - 1;
                this.bodies[0].transform.position = new Vector3(x, y);
            }
            else
            {
                this.bodies[i].SetActive(false);
            }
        }
	}
}
