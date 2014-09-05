using UnityEngine;
using System.Collections;
using Assets.BlijftDatZo.Scripts;

public class GameController : MonoBehaviour 
{
    private GameObject prefabParticleStandard;
    private void Awake()
    {
        this.prefabParticleStandard = (GameObject)Resources.Load(@"Prefabs/ParticleStandard");
    }
    public GameObject GetParticlePrefab()
    {
        return this.prefabParticleStandard;
    }
}
