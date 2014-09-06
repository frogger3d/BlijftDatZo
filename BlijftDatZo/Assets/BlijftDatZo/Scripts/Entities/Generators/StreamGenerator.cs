using UnityEngine;
using System.Collections;
using Assets.BlijftDatZo.Scripts;

public class StreamGenerator : GeneratorBase
{
    private const float Interval = 0.1f;

    private float timeSinceLastParticle;
    private GameObject particlesParent;
    private GameObject prefabParticle;
    private GameObject prefabCollector;
    private GameController gameController;
    private Pool<ParticleBase> particlesPool;
    private GameObject collectorObject;
    private StandardCollector collector;

    public override void Initialize(GameController gameController)
    {
        this.gameController = gameController;

        this.prefabParticle = (GameObject)Resources.Load(@"Prefabs/StreamParticle");
        this.prefabCollector = (GameObject)Resources.Load(@"Prefabs/StandardCollector");

        this.particlesParent = new GameObject("Particles");
        
        this.transform.position = CommonUtils.GetRandomPositionInLevel(Constants.WorldWidth * 0.9f, Constants.WorldHeight * 0.8f) +
			new Vector2(0, Constants.WorldHeight * 0.2f);
        this.collectorObject = (GameObject) GameObject.Instantiate(
            prefabCollector,
            CommonUtils.GetRandomPositionInLevel(Constants.WorldWidth * 0.9f, Constants.WorldHeight * 0.9f), 
            Quaternion.identity);
        this.collector = this.collectorObject.GetComponent<StandardCollector>();
        this.collector.Initialize(this, 10);

        this.particlesPool = new Pool<ParticleBase>(CreateParticle, 100, 50);
    }

    public void CollectParticle(ParticleBase particle)
    {
        this.particlesPool.AddObjectToPool(particle);
		this.gameController.RemoveParticle (particle);
    }

    public override void CollectionGoalAchieved()
    {
        DestroyEverything();
        this.gameController.GameStateFinished();
    }

    private void Update()
    {
        this.timeSinceLastParticle += Time.deltaTime;

        if (this.timeSinceLastParticle > Interval)
        {
            this.timeSinceLastParticle = 0;
            ParticleBase particle = this.particlesPool.GetObjectFromPool();
            particle.Setup(this.gameObject.transform.position, Quaternion.identity, new Vector2(0, 0));
			this.gameController.AddParticle (particle);
        }
    }

    private ParticleBase CreateParticle()
    {
        GameObject particle = (GameObject)GameObject.Instantiate(this.prefabParticle, Vector3.zero, Quaternion.identity);
        particle.transform.parent = this.particlesParent.transform;
        ParticleBase particleBase = particle.GetComponent<ParticleBase>();
        particleBase.Initialize(this);
		return particleBase;
    }

    private void DestroyEverything()
    {
        this.gameController.ClearParticles();
        GameObject.Destroy(this.particlesParent);
        GameObject.Destroy(this.collectorObject);
        GameObject.Destroy(this.gameObject);
    }
}
