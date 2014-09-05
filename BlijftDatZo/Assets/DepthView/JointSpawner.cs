using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JointSpawner : MonoBehaviour {

    BodyService bodyService;

    List<GameObject> bodies = new List<GameObject>();
    private GameObject prefab;

    void Awake()
    {
        this.prefab = (GameObject)Resources.Load(@"Prefabs/ParticleStandard"); 
    }

	// Use this for initialization
	void Start () {
        this.bodyService = new BodyService();

        for(int userIndex = 0; userIndex < 6; userIndex++)
        {
            var bodyObject = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            
            bodyObject.transform.parent = this.transform;           

            this.bodies.Add(bodyObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
        this.bodyService.Update();

        int width = 512;
        int height = 424;

        for(int userIndex = 0; userIndex < this.bodies.Count; userIndex++)
        {
            var position = this.bodyService.GetJointPosition(userIndex, Windows.Kinect.JointType.Head);
            if(position.HasValue)
            {
                this.bodies[userIndex].SetActive(true);
                var x = Mathf.Lerp(-1, 1, position.Value.x / width);
                var y = Mathf.Lerp(1, -1, position.Value.y / height);
                var to = new Vector3(x, y);
                Debug.Log(string.Format("Updating body position {0} to {1}", position.Value, to));
                this.bodies[userIndex].transform.position = 4*to;
            }
            else
            {
                this.bodies[userIndex].SetActive(false);
            }
        }
	}
}
