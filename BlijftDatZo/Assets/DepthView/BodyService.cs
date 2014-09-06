using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System.Linq;

public class BodyService
{
    private KinectSensor sensor;

    private BodyFrameReader bodyReader;

    private Body[] bodies;

    public BodyService()
    {
        this.sensor = KinectSensor.GetDefault();

        if (this.sensor != null)
        {
            this.bodyReader = sensor.BodyFrameSource.OpenReader();

            if (!this.sensor.IsOpen)
            {
                this.sensor.Open();
            }
        }
    }

    /// <summary>
    /// Gets the position of a joint of a user in depth space coordinates
    /// </summary>
    public Vector2? GetJointPosition(int userIndex, JointType jointType)
    {
        if (this.bodies == null)
        {
            // Not initialized yet
            return null;
        }

        var body = this.bodies[userIndex];
        var joint = body.Joints[jointType];
        if (joint.TrackingState == TrackingState.NotTracked)
        {
            return null;
        }

        var depthPosition = this.sensor.CoordinateMapper.MapCameraPointToDepthSpace(joint.Position);
        return new Vector2(depthPosition.X, depthPosition.Y);
    }

    // Update is called once per frame
    public void Update()
    {
        using (var bodyFrame = this.bodyReader.AcquireLatestFrame())
        {
            if (bodyFrame != null)
            {
                if (this.bodies == null || this.bodies.Length != bodyFrame.BodyCount)
                {
                    this.bodies = new Body[6];
                }

                bodyFrame.GetAndRefreshBodyData(this.bodies);
            }
        }

        if (updateCount++ % 30 == 0 && this.bodies != null)
        {
            //Debug.Log("Bodies tracked" + bodies.Where(b => b.IsTracked).Count());
        }
    }

    public int updateCount;

    /// <summary>
    /// This is kind of like the Dispose
    /// </summary>
    private void OnApplicationQuit()
    {
        if (bodyReader != null)
        {
            bodyReader.Dispose();
            bodyReader = null;
        }

        if (sensor != null)
        {
            if (sensor.IsOpen)
            {
                sensor.Close();
            }

            sensor = null;
        }
    }
}
