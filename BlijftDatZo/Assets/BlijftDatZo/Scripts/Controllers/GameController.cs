using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.BlijftDatZo.Scripts;

public class GameController : MonoBehaviour 
{
    private GameObject prefabParticleStandard;
	private AudioClip[] audioClipsA;
	private AudioClip[] audioClipsB;
	private List<Generator> allGenerators;

	public List<ParticleBase> AllActiveParticles { 
				get ;
				private set;
	}

	private void Awake()
    {
        this.prefabParticleStandard = (GameObject)Resources.Load(@"Prefabs/ParticleStandard");

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

		this.AllActiveParticles = new List<ParticleBase> ();
    }
    public GameObject GetParticlePrefab()
    {
        return this.prefabParticleStandard;
    }

	public AudioClip[] GetAudioClipsA()
	{
		return this.audioClipsA;
	}
	public AudioClip[] GetAudioClipsB()
	{
		return this.audioClipsB;
	}

	public void AddParticle(ParticleBase p)
	{
		this.AllActiveParticles.Add (p);
	}

	public void RemoveParticle(ParticleBase p)
	{
		this.AllActiveParticles.Remove (p);
	}
}
