using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    
    public void CargaNivel(string pNombrenivel)
    {
        var tiempoInicio = 5F;
        var tiempoFinal = 0f;

        tiempoInicio += Time.deltaTime;

        if (tiempoInicio >= tiempoFinal)
        {
            SceneManager.LoadScene(pNombrenivel);
        }
    }
}
