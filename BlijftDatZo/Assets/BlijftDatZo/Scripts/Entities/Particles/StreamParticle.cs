using UnityEngine;
using System.Collections;
using Assets.BlijftDatZo.Scripts;

public class StreamParticle : ParticleBase
{
    private void Update()
    {
        if (this.gameObject.transform.position.y < Constants.WorldHeight * -0.5f)
        {
            this.Generator.CollectParticle(this);
        }
    }
}
