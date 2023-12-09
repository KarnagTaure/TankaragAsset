using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProyectoTank.enemigos
{

    public class LogicaTorreta : MonoBehaviour
    {
        public GameObject targetEnemigo;

        void Start()
        {
            targetEnemigo = GameObject.FindGameObjectWithTag("Player");
        }

        
        void Update()
        {
           
                var lookPos = targetEnemigo.transform.position - transform.position;
                lookPos.y = 0;
                var rotacion = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacion, 2);
                //ani.SetBool("walk", false);

                //ani.SetBool("run", true);
                
            
        }
    }
}