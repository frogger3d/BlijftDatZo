using UnityEngine;
using System.Collections;
using Assets.BlijftDatZo.Scripts;

public class Generator : MonoBehaviour 
{
    private const float Interval = 0.1f;

    private float timeSinceLastParticle;
    private GameController gameController;
    private GameObject particleParentObject;
    private Pool<ParticleBase> particlesPool;
    private void Start()
    {
        this.timeSinceLastParticle = 0;
        this.gameController = GameSceneController.Instance.GameController;
        this.particleParentObject = new GameObject("Particles");
        this.particlesPool = new Pool<ParticleBase>(CreateParticle, 100, 50);
    }

    private void Update()
    {
        this.timeSinceLastParticle += Time.deltaTime;

        if (this.timeSinceLastParticle > Interval)
        {
            this.timeSinceLastParticle = 0;
            this.particlesPool.GetObjectFromPool();
        }
    }

    private ParticleBase CreateParticle()
    {
        GameObject particlePrefab = this.gameController.GetParticlePrefab();
        GameObject particle = (GameObject)GameObject.Instantiate(particlePrefab, Vector3.zero, Quaternion.identity);
        particle.transform.parent = this.particleParentObject.transform;
        return particle.GetComponent<ParticleBase>();
    }
}
