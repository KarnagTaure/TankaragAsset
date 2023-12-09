using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salir : MonoBehaviour
{
    // Start is called before the first frame update
   public void SalirJuego()
    {
        Application.Quit();
        Debug.Log("Se salio del juego");
    }
}
