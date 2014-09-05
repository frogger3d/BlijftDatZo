using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class DepthReader : MonoBehaviour
{
	private KinectSensor sensor;

	private DepthFrameReader  depthReader;

	private bool isDirty;

	private Texture2D texture;

	private ushort[] depth;

	/// <summary>
	/// Raw data to load in the texture
	/// </summary>
	private byte[] _RawData;
    private BodyFrameReader bodyReader;

    private Body[] bodies;

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
        if(joint.TrackingState == TrackingState.NotTracked)
        {
            return null;
        }

        var depthPosition = this.sensor.CoordinateMapper.MapCameraPointToDepthSpace(joint.Position);
        return new Vector2(depthPosition.X, depthPosition.Y);
    }

	void Start ()
	{
		this.sensor = KinectSensor.GetDefault();
		
		if (this.sensor != null) 
		{
			this.depthReader = sensor.DepthFrameSource.OpenReader();
            this.bodyReader = sensor.BodyFrameSource.OpenReader();

			if (!this.sensor.IsOpen)
			{
				this.sensor.Open();
			}
		}
		//gameObject.renderer.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
	}
	
	// Update is called once per frame
	void Update ()
    {
		{
            UpdateDepth();
            UpdateBodies();
		}
	}

    private void UpdateBodies()
    {
        using (var bodyFrame = this.bodyReader.AcquireLatestFrame())
        {
            if (bodyFrame!= null)
            {
                if (this.bodies == null || this.bodies.Length != bodyFrame.BodyCount)
                {
                    this.bodies = new Body[6];
                }

                bodyFrame.GetAndRefreshBodyData(this.bodies);
            }
        }
    }

    private void UpdateDepth()
    {
        using (var depthFrame = depthReader.AcquireLatestFrame())
        {
            if (depthFrame != null)
            {
                var frameDesc = depthFrame.FrameDescription;
                if (this.texture == null)
                {
                    this.texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGBA32, false);
                    if (this.depth == null || depth.Length != frameDesc.LengthInPixels)
                    {
                        this.depth = new ushort[frameDesc.LengthInPixels];
                        this._RawData = new byte[frameDesc.LengthInPixels * 4];
                    }
                }

                depthFrame.CopyFrameDataToArray(depth);
            }
            else
            {
                Debug.Log("depthFrame is null");
            }
        }

        int min = 500;
        int max = 4500;
        if (this.depth != null)
        {
            Debug.Log("loading texture");
            int index = 0;
            foreach (var ir in depth)
            {

                float fintensity = Mathf.InverseLerp(min, max, ir);
                byte intensity = (byte)Mathf.Lerp(255, 0, fintensity);
                _RawData[index++] = intensity;
                _RawData[index++] = intensity;
                _RawData[index++] = intensity;
                _RawData[index++] = 255; // Alpha
            }
            this.texture.LoadRawTextureData(_RawData);
            this.texture.Apply();


            gameObject.renderer.material.mainTexture = this.texture;
        }
    }

    /// <summary>
    /// This is kind of like the Dispose
    /// </summary>
	private void OnApplicationQuit()
	{
		if (depthReader != null)
		{
			depthReader.Dispose();
			depthReader = null;
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
