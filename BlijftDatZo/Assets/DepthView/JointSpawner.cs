using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;

public class JointSpawner : MonoBehaviour
{
    const int WIDTH = 512;
    const int HEIGHT = 424;

    BodyService bodyService;

    List<GameObject> bodies = new List<GameObject>();
    private GameObject prefab;

    List<JointType> trackedJoints = new List<JointType>();
    List<JointType> possibleJoints = new List<JointType>() {
        JointType.Head, 
        JointType.HandLeft, 
        JointType.HandRight,
        JointType.ElbowLeft,
        JointType.ElbowRight,
        JointType.KneeLeft,
        JointType.KneeRight,
    };

    void Awake()
    {
        this.prefab = (GameObject)Resources.Load(@"Prefabs/ForceJoint"); 
    }

	// Use this for initialization
    void Start()
    {
        this.bodyService = new BodyService();
        this.ReshuffleJoints();
    }

    private void ClearBodies()
    {
        foreach(var body in this.bodies)
        {
            GameObject.Destroy(body);
        }
        this.bodies.Clear();
    }

    private void CreateBodies(IList<JointType> trackedJoints)
    {

        for (int userIndex = 0; userIndex < 6; userIndex++)
        {
            // Add one game object per body
            var bodyObject = new GameObject("body:" + userIndex);
            bodyObject.transform.parent = this.transform;
            bodyObject.transform.localScale = new Vector3(1, 1, 1);
            this.bodies.Add(bodyObject);

            foreach (JointType jt in trackedJoints)
            {
                GameObject jointObj = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
                jointObj.name = jt.ToString();
                jointObj.transform.parent = bodyObject.transform;
            }
        }
    }

    System.Random random = new System.Random();

    private void ReshuffleJoints()
    {
        this.ClearBodies();
        this.trackedJoints = this.possibleJoints.OrderBy(j => random.Next()).Take(2).ToList();
        this.CreateBodies(this.trackedJoints);
    }
	
	// Update is called once per frame
    void Update()
    {
        this.bodyService.Update();

        if(this.bodyService.updateCount % 1500 == 0)
        {
            this.ReshuffleJoints();
        }

        for (int userIndex = 0; userIndex < this.bodies.Count; userIndex++)
        {
            this.bodies[userIndex].SetActive(true);
            foreach (JointType jt in this.trackedJoints)
            {
                this.UpdatePosition(userIndex, jt);
            }
        }
    }

    private void UpdatePosition(int userIndex, JointType jointType)
    {
        var position = this.bodyService.GetJointPosition(userIndex, jointType);
        var bodyObject = this.bodies[userIndex];
        var jointObject = bodyObject.transform.FindChild(jointType.ToString()).gameObject;

        if (position.HasValue)
        {
            jointObject.SetActive(true);
            var x = Mathf.Lerp(-.5f, .5f, position.Value.x / WIDTH);
            var y = Mathf.Lerp(.5f, -.5f, position.Value.y / HEIGHT);
            var to = new Vector3(x, y);
            jointObject.transform.localPosition = to;
        }
        else
        {
            jointObject.SetActive(false);
        }
    }
}
