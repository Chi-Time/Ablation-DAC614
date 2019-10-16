using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof (Collider), typeof (Rigidbody))]
class Obstacle : MonoBehaviour, IPoolable
{
    public GameObject GameObject => this.gameObject;

    [Tooltip ("The min and max forces that can be applied to the obstacle.")]
    [SerializeField] private Range _ForceRange = new Range (250f, 350f);
    [Tooltip ("The Z position in the world at which this object culls itself and returns back to the pool.")]
    [SerializeField] private float _CullingPoint = -25f;

    private Pool _Pool = null;
    private Rigidbody _Rigidbody = null;

    public void SetPool (Pool pool)
    {
        _Pool = pool;
    }

    public void Cull ()
    {
        _Pool.ReturnToPool (this);
    }

    private void Awake ()
    {
        GetComponent<Collider> ().isTrigger = true;

        _Rigidbody = GetComponent<Rigidbody> ();
        _Rigidbody.isKinematic = false;
        _Rigidbody.useGravity = false;
        _Rigidbody.freezeRotation = true;

        this.tag = "Obstacle";
    }

    private void FixedUpdate ()
    {
        if (_Rigidbody.position.z <= _CullingPoint)
            Cull ();
    }

    private void OnEnable ()
    {
        ApplyForce ();
    }

    private void ApplyForce ()
    {
        float force = Random.Range (_ForceRange.Min, _ForceRange.Max);
        _Rigidbody.AddForce (Vector3.back * force, ForceMode.Force);
    }

    private void OnTriggerEnter (Collider collision)
    {
        if (collision.gameObject.CompareTag ("Player"))
            Cull ();
    }

    private void OnDisable ()
    {
        _Rigidbody.velocity = Vector3.zero;
    }
}
