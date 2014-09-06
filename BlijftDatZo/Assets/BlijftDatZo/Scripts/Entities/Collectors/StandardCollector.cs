using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StandardCollector : MonoBehaviour
{
    private GeneratorBase generator;
    private int collectionGoal;
    private int collected;
	private SoundPlayer soundPlayer;

    public void Initialize(GeneratorBase generator, int collectionGoal)
    {
        this.generator = generator;
        this.collectionGoal = collectionGoal;
		this.soundPlayer = new SoundPlayer ();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ParticleBase particleBase = collider.gameObject.GetComponent<ParticleBase>();
        if (particleBase != null)
        {
            particleBase.HitCollector();
			this.soundPlayer.PlayA(this.audio);
            this.collected++;
            if (this.collected >= this.collectionGoal)
            {
                this.generator.CollectionGoalAchieved();
            }
        }
    }
}
