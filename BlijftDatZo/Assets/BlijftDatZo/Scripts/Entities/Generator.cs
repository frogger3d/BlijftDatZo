using UnityEngine;
using System.Collections;
using Assets.BlijftDatZo.Scripts;

public class Generator : MonoBehaviour 
{
    private const float Interval = 0.1f;

    private float timeSinceLastParticle;
    private GameController gameController;
    private GameObject particleParentObject;
    private void Start()
    {
        this.timeSinceLastParticle = 0;
        this.gameController = GameSceneController.Instance.GameController;
        this.particleParentObject = new GameObject("Particles");
    }

    private void Update()
    {
        this.timeSinceLastParticle += Time.deltaTime;

        if (this.timeSinceLastParticle > Interval)
        {
            this.timeSinceLastParticle = 0;
            CreateParticle();
        }
    }

    private void CreateParticle()
    {
        GameObject particlePrefab = this.gameController.GetParticlePrefab();
        GameObject particle = (GameObject)GameObject.Instantiate(particlePrefab, Vector3.zero, Quaternion.identity);
        particle.transform.parent = this.particleParentObject.transform;
    }
}
