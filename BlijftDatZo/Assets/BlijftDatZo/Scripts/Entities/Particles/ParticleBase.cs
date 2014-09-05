using System;

using UnityEngine;

public abstract class ParticleBase : MonoBehaviour, IPoolable
{
    protected Generator Generator { get; private set; }

    public void Initialize(Generator generator)
    {
        this.Generator = generator;
    }

    public void Setup(Vector2 position, Quaternion rotation, Vector2 velocity)
    {
        this.gameObject.transform.position = position;
        this.gameObject.transform.rotation = rotation;
        this.gameObject.rigidbody2D.velocity = velocity;
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
