using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProyectoTank.SistemaVida
{
    public class DatosEnemigos : MonoBehaviour
    {
        public int vidaEnemigo;
        public Slider barraVida;


        private void Update()
        {
            barraVida.value = vidaEnemigo;
        }
    }
}