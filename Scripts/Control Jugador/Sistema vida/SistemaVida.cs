using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ProyectoTank.SistemaVida
{
    

    public class SistemaVida : MonoBehaviour
    {
        public float vidaMax = 100f;
        public float vidaActual;
        public int rutina;
      


        public bool inmortal = false;
        public float tiempoInmortal = 2f;

        public Slider barraVida;
        public GameObject gameOver;
        public GameObject tanqueDestruido;


       
        void Start()
        {
            vidaActual = vidaMax;
            
        }

        
        void Update()
        {
            if(vidaActual> vidaMax)
            {
                vidaActual = vidaMax;
            }
            if(vidaActual<= 0)
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
            
            Destroy(this.gameObject);
            Instantiate(tanqueDestruido, transform.position, transform.rotation);
            gameOver.SetActive(true);

        }
       
        IEnumerator TiempoInmortal()
        {
            inmortal = true;
            yield return new WaitForSeconds(tiempoInmortal);
            inmortal = false;

        }
    }
}