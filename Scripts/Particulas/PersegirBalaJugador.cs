using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProyectoTank.Particulas
{
    public class PersegirBalaJugador : MonoBehaviour
    {
        public GameObject bala;
       
        void Start()
        {
            bala = GameObject.FindGameObjectWithTag("Bala");
        }

       
        void FixedUpdate()
        {
            if (bala != null)
            {
                transform.position = bala.transform.position;


            }
            else
            {
                return;
            }
            
        }
    }
}