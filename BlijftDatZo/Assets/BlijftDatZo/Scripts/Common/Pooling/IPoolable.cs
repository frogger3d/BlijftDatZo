using System;

public interface IPoolable
{
    Pool<IPoolable> Pool_ { get; set; }
    void EnterPool();
    void ExitPool();
}
