using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProyectoTank.SistemaVida
{
    public class SistemaDaño : MonoBehaviour
    {
        public float daño = 10f;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.GetComponent<SistemaVida>() != null)
            {
                other.gameObject.GetComponent<SistemaVida>().QuitarVida(daño);
            }
            else if (other.gameObject.GetComponent<EnemigoSistemaVida>() != null)
            {
                other.gameObject.GetComponent<EnemigoSistemaVida>().QuitarVida(daño);
            }
        }
        
    }
}