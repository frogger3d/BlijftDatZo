using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.BlijftDatZo.Scripts;

public class GameController : MonoBehaviour 
{
    private GameObject prefabParticleStandard;
	private List<Generator> allGenerators;

	public List<ParticleBase> AllActiveParticles { 
				get ;
				private set;
	}

	private void Awake()
    {
        this.prefabParticleStandard = (GameObject)Resources.Load(@"Prefabs/ParticleStandard");
		this.AllActiveParticles = new List<ParticleBase> ();
    }
    public GameObject GetParticlePrefab()
    {
        return this.prefabParticleStandard;
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
