using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProyectoTank.SistemaVida
{


    public class EnemigoSistemaVida : MonoBehaviour
    {
        public float vidaMax = 40f;
        public float vidaActual;
        public int rutina;
        public GameObject[] item;
       


        public bool inmortal = false;
        public float tiempoInmortal = 2f;
        public GameObject tanqueDestruido;

        public Slider barraVida;
       

        void Start()
        {
            vidaActual = vidaMax;
            barraVida.maxValue = vidaMax;
                
                
        }


        void Update()
        {
            if (vidaActual > vidaMax)
            {
                vidaActual = vidaMax;
            }
            if (vidaActual <= 0)
            {
                Muerte();

            }

            barraVida.value = vidaActual;

            

        }
        public void DarVida(float vida)
        {
            vidaActual += vida;
        }
        public void QuitarVida(float daño)
        {
            if (inmortal) return;
            vidaActual -= daño;
            StartCoroutine(TiempoInmortal());
        }
        public void Muerte()
        {
            SoltarObjeto();
            Destroy(this.gameObject);
            Instantiate(tanqueDestruido, transform.position, transform.rotation);

        }
        public void SoltarObjeto()
        {
            int itemAleatorio = Random.Range(0, item.Length);

            Instantiate(item[itemAleatorio], transform.position, transform.rotation);
           

        }
        IEnumerator TiempoInmortal()
        {
            inmortal = true;
            yield return new WaitForSeconds(tiempoInmortal);
            inmortal = false;

        }
    }
}