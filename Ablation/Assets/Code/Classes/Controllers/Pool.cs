using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Pool
{
    /// The pool of active objects currently in scene.
    public List<IPoolable> ActivePool { get { return _ActivePool; } }
    /// The pool of inactive objects currently in scene.
    public List<IPoolable> InactivePool { get { return _InactivePool; } }

    [Tooltip ("The number of objects to spawn into the pool.")]
    [SerializeField] private int _PoolSize = 0;
    [Tooltip ("The prefab objects to pool.")]
    [SerializeField] private GameObject[] _Prefabs = null;
    [Tooltip ("The pool of currently active objects in scene.")]
    [SerializeField] private List<IPoolable> _ActivePool = new List<IPoolable> ();
    [Tooltip ("The pool of currently inactive objects in scene.")]
    [SerializeField] private List<IPoolable> _InactivePool = new List<IPoolable> ();

    /// The name of the current pool instance.
    private string _PoolName = "Pool";
    /// The name of object to deal with.
    private string _ObjectName = "Object";

    /// <summary>Initialises the pool for later use. (Faux Constructor.)</summary>
    /// <param name="poolName">The name of the pool in the editor.</param>
    /// <param name="objectName">The name of the object that will be pooled.</param>
    public void Constructor (string poolName, string objectName)
    {
        _PoolName = poolName;
        _ObjectName = objectName;

        GeneratePool ();
    }

    private void GeneratePool ()
    {
        for (int i = 0; i < _PoolSize; i++)
            for (int j = 0; j < _Prefabs.Length; j++)
                _InactivePool.Add (SpawnPoolObject (_Prefabs[j], i));
    }

    /// <summary>Spawns a defaulted object, set up and ready to be used.</summary>
    /// <param name="obj">The gameobject to spawn.</param>
    /// <param name="index">The current index of the object.</param>
    /// <returns></returns>
    private IPoolable SpawnPoolObject (GameObject obj, int index)
    {
        GameObject go = Object.Instantiate (obj, Vector3.zero, Quaternion.identity);
        go.transform.SetParent (GetPoolHolder ());
        go.name = _ObjectName + ": " + index;
        go.SetActive (false);

        var poolObj = (IPoolable)go.GetComponent (typeof (IPoolable));
        poolObj.SetPool (this);

        return poolObj;
    }

    /// <summary>Retrieves the pool holder for this pool.</summary>
    /// <returns></returns>
    private Transform GetPoolHolder ()
    {
        if (!GameObject.Find (_PoolName))
            return new GameObject (_PoolName).transform;

        return GameObject.Find (_PoolName).transform;
    }

    /// <summary>Retrieves an object from the pool ready to go.</summary>
    /// <returns></returns>
    public IPoolable RetrieveFromPool ()
    {
        if (_InactivePool.Count > 0)
        {
            var poolObj = _InactivePool[0];
            _InactivePool.Remove (poolObj);
            _ActivePool.Add (poolObj);

            poolObj.GameObject.SetActive (true);

            return poolObj;
        }

        return null;
    }

    /// <summary>Retrieves a random object from the pool ready to go.</summary>
    /// <returns></returns>
    public IPoolable RetrieveFromPoolRandom ()
    {
        if (_InactivePool.Count > 0)
        {
            var poolObj = _InactivePool[Random.Range (0, _InactivePool.Count - 1)];
            _InactivePool.Remove (poolObj);
            _ActivePool.Add (poolObj);

            poolObj.GameObject.SetActive (true);

            return poolObj;
        }

        return null;
    }

    /// <summary>Returns the object back to the pool and resets it.</summary>
    /// <param name="poolObj">The pool object to return.</param>
    public void ReturnToPool (IPoolable poolObj)
    {
        _ActivePool.Remove (poolObj);
        _InactivePool.Add (poolObj);

        poolObj.GameObject.SetActive (false);
        poolObj.GameObject.transform.position = Vector3.zero;
        poolObj.GameObject.transform.rotation = Quaternion.Euler (Vector3.zero);
    }
}