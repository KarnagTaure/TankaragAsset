using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProyectoTank.SistemaVida
{
    public class DarVida : MonoBehaviour
    {
        public float vida = 10f;


         void Update()
        {
            transform.Rotate(new Vector3(0, 20f, 0) * Time.deltaTime);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<SistemaVida>() != null)
            {
                other.gameObject.GetComponent<SistemaVida>().DarVida(vida);
                Destroy(gameObject);
            }
        }


    }
}