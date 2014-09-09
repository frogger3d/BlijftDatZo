using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.BlijftDatZo.Scripts;

public class GameController : MonoBehaviour 
{
    //private GameObject prefabParticleStandard;
	private List<GameObject> generatorPrefabs;
	private AudioClip[] audioClipsA;
	private AudioClip[] audioClipsB;

	public List<ParticleBase> AllActiveParticles { 
				get ;
				private set;
	}

    private List<string> textureResources = new List<string>()
    {
        @"glitter-pictures-1.jpg",
        @"gold-glitter-wallpaper-7.jpg",
        @"hd-texture-farbige-zuckerlinsen-11LR0053S.png",
        @"hs-2010-13-g-xlarge_web.jpg",
        @"img100.jpg",
        @"wood-texture-abstract-hd-wallpaper-1920x1200-5488"
    };

	private void Awake()
    {
        // Start body mask background for each user
        var maskPrefab = Resources.Load<GameObject>(@"Prefabs/Mask");
        var scalar = GameObject.FindGameObjectWithTag(Constants.TagScalar);
        for (int i = 0; i < 6; i++)
        {
            var maskObject = (GameObject)GameObject.Instantiate(maskPrefab);
            var texture = (Texture2D)Resources.Load(@"Masks/" + textureResources[i]);
            maskObject.renderer.material.mainTexture = texture;
            var maskService = maskObject.GetComponent<BodyMaskService>();
            maskService.BodyIndex = i;
            maskObject.transform.parent = scalar.transform;
        }

        // Instantiate particle factory
        //this.prefabParticleStandard = (GameObject)Resources.Load(@"Prefabs/StreamParticle");
        this.AllActiveParticles = new List<ParticleBase>();
        generatorPrefabs = new List<GameObject>();
        generatorPrefabs.Add((GameObject)Resources.Load(@"Prefabs/StreamGenerator"));

        // Load audio
		this.audioClipsA = new AudioClip[6];
		for (int i = 0; i < this.audioClipsA.Length; i++) 
		{
			this.audioClipsA [i] = (AudioClip)Resources.Load (@"Sounds/A" + (i + 1));
		}
		
		this.audioClipsB = new AudioClip[10];
		for (int i = 0; i < this.audioClipsB.Length; i++) 
		{
			this.audioClipsB [i] = (AudioClip)Resources.Load (@"Sounds/B" + (i + 1));
		}
    }

    private void Start()
    {
        InstantiateRandomGenerator();
    }

    private void InstantiateRandomGenerator()
    {
        GameObject generatorObject = (GameObject) GameObject.Instantiate(this.generatorPrefabs[Random.Range(0, this.generatorPrefabs.Count - 1)]);
        GeneratorBase generator = generatorObject.GetComponent<GeneratorBase>();
        generator.Initialize(this);
    }

	public AudioClip[] GetAudioClipsA()
	{
		return this.audioClipsA;
	}

	public AudioClip[] GetAudioClipsB()
	{
		return this.audioClipsB;
	}

    public void GameStateFinished()
    {
        InstantiateRandomGenerator();
    }

	public void AddParticle(ParticleBase p)
	{
				this.AllActiveParticles.Add (p);
		}

	public void RemoveParticle(ParticleBase p)
	{
				this.AllActiveParticles.Remove (p);
	}

    public void ClearParticles()
    {
        this.AllActiveParticles.Clear();
    }
}
