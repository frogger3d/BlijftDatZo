using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class DepthReader : MonoBehaviour
{
	private KinectSensor _Sensor;

	private DepthFrameReader  _Reader;

	private bool isDirty;

	private Texture2D texture;

	private ushort[] depth;

	/// <summary>
	/// Raw data to load in the texture
	/// </summary>
	private byte[] _RawData;

	void Start ()
	{
		_Sensor = KinectSensor.GetDefault();
		
		if (_Sensor != null) 
		{
			_Reader = _Sensor.DepthFrameSource.OpenReader();
			//_Reader.MultiSourceFrameArrived += this.FrameArrived;
			if (!_Sensor.IsOpen)
			{
				_Sensor.Open();
			}
		}
		//gameObject.renderer.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
	}
	
	// Update is called once per frame
	void Update () {
		//if(this.isDirty)
		{
//			var multiFrame = this._Reader.AcquireLatestFrame ();
//			if(multiFrame == null)
//			{
//				Debug.Log("multiframe is null");
//				return;
//			}

			using (var depthFrame = _Reader.AcquireLatestFrame()) // multiFrame.DepthFrameReference.AcquireFrame ())
			{
				if(depthFrame != null)
				{
					var frameDesc = depthFrame.FrameDescription;
					if(this.texture == null)
					{
						this.texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGBA32, false);
						if(this.depth == null || depth.Length != frameDesc.LengthInPixels)
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
			if(this.depth != null)
			{
				Debug.Log ("loading texture");
				int index = 0;
				foreach(var ir in depth)
				{

					float fintensity = Mathf.InverseLerp (min, max, ir);
					byte intensity = (byte)Mathf.Lerp (255, 0, fintensity);
					//intensity = 0;
					_RawData[index++] = intensity;
					_RawData[index++] = intensity;
					_RawData[index++] = intensity;
					_RawData[index++] = 255; // Alpha
				}
				this.texture.LoadRawTextureData(_RawData);
				this.texture.Apply ();


				gameObject.renderer.material.mainTexture = this.texture;
			}			
		}
	}
	
	private void FrameArrived(object source, MultiSourceFrameArrivedEventArgs args)
	{
		this.isDirty = true;
	}

	void OnApplicationQuit()
	{
		if (_Reader != null)
		{
			_Reader.Dispose();
			_Reader = null;
		}
		
		if (_Sensor != null)
		{
			if (_Sensor.IsOpen)
			{
				_Sensor.Close();
			}
			
			_Sensor = null;
		}
	}
}
