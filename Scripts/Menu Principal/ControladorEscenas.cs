using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProyectoTank.Escenas
{

    public class ControladorEscenas : MonoBehaviour
    {
        public ProyectoTank.MenuInicio.MenuPrincipal panelOpciones;
        public ProyectoTank.MenuInicio.MenuPrincipal panelControles;

        // Start is called before the first frame update
        void Start()
        {
            panelOpciones = GameObject.FindGameObjectWithTag("Panel Opciones").GetComponent<ProyectoTank.MenuInicio.MenuPrincipal>();
            panelControles = GameObject.FindGameObjectWithTag("Panel Opciones").GetComponent<ProyectoTank.MenuInicio.MenuPrincipal>();
        }

       public void MostrarPanelOpciones()
        {
            panelOpciones.panelOpciones.SetActive(true);

        }
        public void MostrarPanelControles()
        {
            panelControles.controles.SetActive(true);

        }
    }
}