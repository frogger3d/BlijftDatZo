using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.BlijftDatZo.Scripts;

public class GameController : MonoBehaviour 
{
    //private GameObject prefabParticleStandard;
	private List<GameObject> generatorPrefabs;
    
	public List<ParticleBase> AllActiveParticles { 
				get ;
				private set;
	}

	private void Awake()
    {
        //this.prefabParticleStandard = (GameObject)Resources.Load(@"Prefabs/StreamParticle");
		this.AllActiveParticles = new List<ParticleBase> ();
        generatorPrefabs = new List<GameObject>();
        generatorPrefabs.Add((GameObject)Resources.Load(@"Prefabs/StreamGenerator"));
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
