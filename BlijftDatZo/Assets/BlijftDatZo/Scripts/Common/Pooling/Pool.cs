using System;
using System.Collections.Generic;

public class Pool<T> where T: IPoolable
{
    public delegate T CreateObjectDelegate();

    private Queue<T> objects;
    private CreateObjectDelegate createObject;
    private int increaseCount;

    public Pool(CreateObjectDelegate createObject, int initialCount, int increaseCount)
    {
        this.createObject = createObject;
        this.increaseCount = increaseCount;
        this.objects = new Queue<T>();

        CreateAndAddObjects(initialCount);
    }

    public void AddObjectToPool(T obj)
    {
        obj.EnterPool();
        this.objects.Enqueue(obj);
    }

    public T GetObjectFromPool()
    {
        if (this.objects.Count < 1)
        {
            CreateAndAddObjects(this.increaseCount);
        }

        T obj = this.objects.Dequeue();
        obj.ExitPool();
        return obj;
    }

    private void CreateAndAddObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            AddObjectToPool(this.createObject());
        }
    }
}
