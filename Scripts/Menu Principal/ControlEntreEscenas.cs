using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProyectoTank.MenuInicio
{

    public class ControlEntreEscenas : MonoBehaviour
    {
        private void Awake()
        {
            var noDestruirEntreEscenas = FindObjectsOfType<ControlEntreEscenas>();
            if (noDestruirEntreEscenas.Length>1)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }
       
    }
}