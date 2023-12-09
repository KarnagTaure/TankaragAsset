using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProyectoTank.enemigos
{
    public class Enemigo : MonoBehaviour
    {

        public int rutina;
        public float cronometro;
        public Animator ani;
        public Quaternion angulo;
        public float grado;

        public GameObject targetEnemigo;
        public bool atacando;
        DisparoEnemigo fuego;
        public bool puedeDisparar =true;
        

        // Start is called before the first frame update
        void Start()
        {
            ani = GetComponent<Animator>();
            targetEnemigo = GameObject.FindGameObjectWithTag("Player");
            fuego = GetComponent<DisparoEnemigo>();
            

        }

        // Update is called once per frame
        void Update()
        {
            ComportamientoEnemigo();
        }
        public void ComportamientoEnemigo()
        {
            if (Vector3.Distance(transform.position, targetEnemigo.transform.position) >30)
            {
                //ani.SetBool("run", false);
                cronometro += 1 * Time.deltaTime;
                if (cronometro >= 4)
                {
                    rutina = Random.Range(0, 2);
                    cronometro = 0;
                }
                switch (rutina)
                {
                    case 0:
                       // ani.SetBool("walk", false);
                        break;

                    case 1:
                        grado = Random.Range(0, 360);
                        angulo = Quaternion.Euler(0, grado, 0);
                        rutina++;
                        break;

                    case 2:
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
                        transform.Translate(Vector3.forward * 1 * Time.deltaTime);
                        //ani.SetBool("walk", true);
                        break;

                }

            }
            else
            {
                if (Vector3.Distance(transform.position, targetEnemigo.transform.position) > 15 && !atacando)
                {
                    var lookPos = targetEnemigo.transform.position - transform.position;
                    lookPos.y = 0;
                    var rotacion = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacion, 2);
                    //ani.SetBool("walk", false);

                    //ani.SetBool("run", true);
                    transform.Translate(Vector3.forward * 3 * Time.deltaTime);
                }
                else
                {
                    if (puedeDisparar)
                    {
                        
                        fuego.Fuego();
                        Invoke("TiempoEspera",3f);
                    }
                    puedeDisparar = false;
                    

                }
            }

        }
       
        void TiempoEspera()
        {
            puedeDisparar = true;
        }
    }

}