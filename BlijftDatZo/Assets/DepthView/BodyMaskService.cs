using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class BodyMaskService : MonoBehaviour
{
    private KinectSensor sensor;

    private BodyIndexFrameReader bodyIndexReader;

    private bool isDirty;

    private Texture2D texture;

    private byte[] maskValues;

    /// <summary>
    /// Raw data to load in the texture
    /// </summary>
    private byte[] _RawData;

    void Start()
    {
        this.sensor = KinectSensor.GetDefault();

        if (this.sensor != null)
        {
            this.bodyIndexReader = sensor.BodyIndexFrameSource.OpenReader();

            if (!this.sensor.IsOpen)
            {
                this.sensor.Open();
            }
        }
        //gameObject.renderer.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
    }

    // Update is called once per frame
    void Update()
    {
        using (var depthFrame = bodyIndexReader.AcquireLatestFrame())
        {
            if (depthFrame != null)
            {
                var frameDesc = depthFrame.FrameDescription;
                if (this.texture == null)
                {
                    this.texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGBA32, false);
                    if (this.maskValues == null || maskValues.Length != frameDesc.LengthInPixels)
                    {
                        this.maskValues = new byte[frameDesc.LengthInPixels];
                        this._RawData = new byte[frameDesc.LengthInPixels * 4];
                    }
                }
                Debug.Log(string.Format("width {0} height {1}", frameDesc.Width, frameDesc.Height));
                depthFrame.CopyFrameDataToArray(maskValues);
            }
        }

        if (this.maskValues != null)
        {
            int index = 0;
            foreach (var depth in maskValues)
            {
                if (depth > 5)
                {
                    byte intensity = 0;
                    _RawData[index++] = intensity;
                    _RawData[index++] = intensity;
                    _RawData[index++] = intensity;
                    _RawData[index++] = 0; // Alpha
                }
                else
                {
                    _RawData[index++] = 0; // Red
                    _RawData[index++] = 255; // Green
                    _RawData[index++] = 0; // Blue
                    _RawData[index++] = 255; // Alpha
                }
                
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
        if (bodyIndexReader != null)
        {
            bodyIndexReader.Dispose();
            bodyIndexReader = null;
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
