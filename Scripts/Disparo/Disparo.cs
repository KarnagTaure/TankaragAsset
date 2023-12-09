using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Disparo : MonoBehaviourPunCallbacks
{
    //public GameObject balaPrefab;

    public Transform emisorBala;
    public Transform emisorParticulas;
    public GameObject particulasBalaPrefa;
    public float potenciaTiro;
    public GameObject explosionParticulas;
    public GameObject btnFuego;
    public GameObject balaPrefab;
    private float firenex = 1.5f;
    public float tiempoDisparo = 0.5f;

    public GameObject DetonacionParticula;
    public bool puedeDisparar = true;

    private void Start()
    {
        btnFuego = GameObject.Find("Fuego");
        btnFuego.GetComponent<Button>().onClick.AddListener(Fuego);
    }

    void Update()
    {

        if (btnFuego == null)
        {
            btnFuego = GameObject.Find("Fuego");
            btnFuego.GetComponent<Button>().onClick.AddListener(Fuego);
        }
    }


    public void Fuego()
    {
        if (Time.time > firenex)
        {

            var proyectil = PhotonNetwork.Instantiate(balaPrefab.name, emisorBala.position, emisorBala.rotation);
            Instantiate(explosionParticulas, emisorBala.position, emisorBala.rotation);


            proyectil.GetComponent<Rigidbody>().velocity = proyectil.transform.forward * potenciaTiro;
            proyectil.transform.Rotate(-0, 0, 0);


            firenex = Time.time + tiempoDisparo;




            Destroy(proyectil, 1.6f);
        }


    }
  


}
