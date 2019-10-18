using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ConversationTrigger : MonoBehaviour
{
    private void OnTriggerStay (Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            if (Input.GetMouseButtonDown (0) || Input.GetButtonDown ("Fire1"))
            {
                Signals.ShowText ();
            }
        }
    }
}
