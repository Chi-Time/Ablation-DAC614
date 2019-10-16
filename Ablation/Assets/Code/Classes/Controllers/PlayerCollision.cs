using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Collider))]
class PlayerCollision : MonoBehaviour
{
    [SerializeField] private string _Tag = "Obstacle";

    private void OnTriggerEnter (Collider collision)
    {
        if (collision.gameObject.CompareTag (_Tag))
        {
            print ("Yee");
        }
    }
}
