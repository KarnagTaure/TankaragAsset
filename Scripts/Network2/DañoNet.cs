using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace ProjectoTank
{
    public class DañoNet : MonoBehaviourPunCallbacks
    {
        public float daño = 10f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<PlayerManager>() != null)
            {
                other.gameObject.GetComponent<PlayerManager>().photonView.RPC("QuitarVida", RpcTarget.All, daño);

            }
           
        }
    }
}
