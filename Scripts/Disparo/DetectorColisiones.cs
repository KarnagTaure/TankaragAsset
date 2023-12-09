using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorColisiones : MonoBehaviour
{

    public ParticleSystem detonacion;
    




    private void Update()
    {
        StartCoroutine(TiempoSinChocar());
    }


    private void OnTriggerEnter(Collider other)
    {
   
      

        gameObject.SetActive(false);
        Instantiate(detonacion, transform.position, detonacion.transform.rotation);

        
    }
    IEnumerator TiempoSinChocar()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
        Instantiate(detonacion, transform.position, detonacion.transform.rotation);
    }

}
