using System;

using UnityEngine;

public abstract class ParticleBase : MonoBehaviour, IPoolable
{
    protected StreamGenerator Generator { get; private set; }

    public void Initialize(StreamGenerator generator)
    {
        this.Generator = generator;
    }

    public void Setup(Vector2 position, Quaternion rotation, Vector2 velocity)
    {
        this.gameObject.transform.position = position;
        this.gameObject.transform.rotation = rotation;
        this.gameObject.rigidbody2D.velocity = velocity;
        this.gameObject.rigidbody2D.angularVelocity = 0;
    }

    public void HitCollector()
    {
        this.Generator.CollectParticle(this);
    }

    public virtual void EnterPool()
    { 
        this.gameObject.SetActive(false);
    }
    public virtual void ExitPool()
    {
        this.gameObject.SetActive(true);
    }
}
