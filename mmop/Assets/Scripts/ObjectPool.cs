using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private List<T> pool = new List<T>();
    private T prototype;

    public ObjectPool(T itemToPool, int numToPool)
    {
        prototype = itemToPool;

        for(int i = 0; i < numToPool; i++)
        {
            AddNewToPool();
        }
    }

    public T Get()
    {
        T retItem = null;

        for(int i = 0; i < pool.Count; i++)
        {
            if(pool[i].gameObject.activeSelf)
            {
                retItem = pool[i];

                break;
            }
        }

        if(retItem == null)
        {
            retItem = AddNewToPool();
        }

        return retItem;
    }

    public void Pool(T obj)
    {
        bool itemFound = false;

        foreach(var item in pool)
        {
            if(item.GetInstanceID() == obj.GetInstanceID())
            {
                item.gameObject.SetActive(false);

                itemFound = true;

                break;
            }
        }

        if(!itemFound)
        {
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }
    }

    private T AddNewToPool()
    {
        var obj = MonoBehaviour.Instantiate(prototype);
        obj.gameObject.SetActive(false);
        pool.Add(obj);

        return obj;
    }
}
