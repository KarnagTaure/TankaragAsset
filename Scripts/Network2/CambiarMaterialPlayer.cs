using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace ProjectoTank
{
    public class CambiarMaterialPlayer : MonoBehaviourPunCallbacks
    {

        public Material tankAmarillo;
        public Material tankRojo;
        public Material tankAzul;
        public Material tankVerde;
        // Start is called before the first frame update
        void Start()
        {

            CambiarMaterial(photonView.Owner.GetPlayerNumber());
        }

        public void CambiarMaterial(int materialChoice)
        {
            Renderer rend = this.GetComponent<Renderer>();
            switch (materialChoice)
            {
                case 0: rend.material = tankAmarillo; break;
                case 1: rend.material = tankRojo; break;
                case 2: rend.material = tankAzul; break;
                case 3: rend.material = tankVerde; break;
                case 4: rend.material = tankAmarillo; break;
                case 5: rend.material = tankAmarillo; break;
                case 6: rend.material = tankAmarillo; break;
                case 7: rend.material = tankAmarillo; break;
            }

            return;
        }
    }
}
