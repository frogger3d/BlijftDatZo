using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System.Collections.Generic;
using System.Linq;

public struct ByteColor
{
    public byte Red;
    public byte Green;
    public byte Blue;

    public static ByteColor FromColor(Color color)
    {
        return new ByteColor()
        {
            Red = (byte)(color.linear.r * 255),
            Green = (byte)(color.linear.g * 255),
            Blue = (byte)(color.linear.b * 255),
        };
    }
}

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

    private List<ByteColor> colors;

    /// <summary>
    /// The index of the body mask to fill in
    /// </summary>
    public int BodyIndex;
    public Texture2D MaskTexture;

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

        this.colors = new Color[]
        {
            Color.red,
            // orange
            new Color(255,128,0),
            Color.blue,
            // light green
            new Color(153, 255, 153),
            new Color(153,51,255),
            Color.yellow,
        }
        .Select(c => ByteColor.FromColor(c))
        .ToList();
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
                //Debug.Log(string.Format("width {0} height {1}", frameDesc.Width, frameDesc.Height));
                depthFrame.CopyFrameDataToArray(maskValues);
            }
        }

        if (this.maskValues != null)
        {
            int index = 0;
            foreach (var depth in maskValues)
            {
                if (depth == this.BodyIndex)
                {
                    byte intensity = 0;
                    _RawData[index++] = intensity;
                    _RawData[index++] = intensity;
                    _RawData[index++] = intensity;
                    _RawData[index++] = 255; // Alpha
                }
                else
                {
                    var color = new ByteColor(); // this.colors[depth];
                    _RawData[index++] = color.Red; // Red
                    _RawData[index++] = color.Green; // Green
                    _RawData[index++] = color.Blue; // Blue
                    _RawData[index++] = 0; // Alpha
                }
                
            }
            this.texture.LoadRawTextureData(_RawData);
            this.texture.Apply();
            
            gameObject.renderer.material.SetTexture("_Alpha", this.texture);
            gameObject.renderer.material.SetTexture("_MainTex", this.MaskTexture);
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
