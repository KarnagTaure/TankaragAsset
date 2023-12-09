using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProyectoTank.Particulas
{
    public class PersegirBalaEnemigo : MonoBehaviour
    {
        public GameObject bala;
       
        void Start()
        {
           bala = GameObject.FindGameObjectWithTag("Bala Enemigo");
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