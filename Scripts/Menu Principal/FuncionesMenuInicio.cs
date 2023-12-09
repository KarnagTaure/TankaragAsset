using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FuncionesMenuInicio : MonoBehaviour
{
    public GameObject panelInicio;
    

    // Update is called once per frame
    void Update()
    {

    }
    public void Panel()
    {
        panelInicio.SetActive(true);
    }
    public void CargaNivel(string pNombrenivel)
    {

        
            
       SceneManager.LoadScene(pNombrenivel);
    }
    public void IniciarNivel(string pNombrenivel)
    {
        StartCoroutine("Cargar");
    }
    IEnumerator Cargar(string pNombrenivel)
    {
        yield return new WaitForSeconds(1.5f);

        CargaNivel( pNombrenivel);

    }
}
