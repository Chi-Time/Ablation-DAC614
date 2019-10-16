using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ScenerySet : MonoBehaviour, IPoolable
{
    public GameObject GameObject => this.gameObject;

    public float Speed { get; set; }
    public float CullingPoint { get; set; }

    private Pool _Pool = null;
    private Transform _Transform = null;

    private void Awake ()
    {
        _Transform = GetComponent<Transform> ();
    }

    private void FixedUpdate ()
    {
        _Transform.Translate (Vector3.back * Speed * Time.deltaTime);

        if (_Transform.position.z < CullingPoint)
            Cull ();
    }

    public void SetPool (Pool pool)
    {
        _Pool = pool;
    }

    public void Cull ()
    {
        _Pool.ReturnToPool (this);
    }
}
