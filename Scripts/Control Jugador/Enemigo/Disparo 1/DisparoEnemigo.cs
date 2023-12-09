using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoEnemigo : MonoBehaviour
{
    //public GameObject balaPrefab;
    
    public Transform emisorBala;
    public Transform emisorParticulas;
    public GameObject particulasBalaPrefa;
    public float potenciaTiro;
    public GameObject explosionParticulas;

    public GameObject DetonacionParticula;
    public bool puedeDisparar = true;


    public void Fuego()
    {
       
       // var proyectil = (GameObject)Instantiate(balaPrefab, emisorBala.position, emisorBala.rotation);
        
        GameObject proyectil = ProyectoTank.Municion.PoolBalasEnemigo.CompartirInstance.CojerObjetoPooled();



        if (proyectil != null)
        {
            var particulasBala = Instantiate(particulasBalaPrefa, emisorParticulas.position, emisorParticulas.rotation);
            Instantiate(explosionParticulas, emisorBala.position, emisorBala.rotation);

            proyectil.SetActive(true);
            proyectil.transform.position = emisorBala.position;
            proyectil.transform.rotation = emisorBala.rotation;
            proyectil.GetComponent<Rigidbody>().velocity = proyectil.transform.forward * potenciaTiro;
            proyectil.transform.Rotate(-0, 0, 0);

            Destroy(particulasBala, 1.6f);


        }
        else return;

        

        //Destruccion de la bala en el tiempo

        //Destroy(proyectil, 1.6f);
        

        
    }
    
       

}
