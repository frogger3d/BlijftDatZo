using System;

using UnityEngine;

public abstract class ParticleBase : MonoBehaviour, IPoolable
{
    public Pool<IPoolable> Pool_ { get; set; }

    public virtual void EnterPool()
    { 
        this.gameObject.SetActive(false);
    }
    public virtual void ExitPool()
    {
        this.gameObject.SetActive(true);
    }
}
