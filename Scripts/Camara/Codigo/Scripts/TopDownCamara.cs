using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProyectoTank.Camaras
{
    public class TopDownCamara : MonoBehaviour
    {
        #region Variables
        private Transform m_target;
        public float m_Altura = 10f;
        public float m_Distancia = 20f;
        public float m_Angulo = 45f;
        public float m_SmoothSpeed = 1f;

        public GameObject canvas;
        //public ProyectoTank.MenuInicio.MenuPrincipal canvas;

        public Vector3 VelReferencia;
       

        #endregion



        #region Main Metodos
        // Start is called before the first frame update
        void Start()
        {

            StartCoroutine("Tiempo");

        }

        // Update is called once per frame
        void Update()
        {
            
            ManejoCamara();
        }
        #endregion


        #region Metodos auxiliares
        protected void ManejoCamara()
        {
            if (!m_target) 
            {
                return;
            }
            //Construir el vector de posicion Mundial
            Vector3 worldPosition = (Vector3.forward * -m_Distancia) + (Vector3.up * m_Altura);
            //Debug.DrawLine(m_target.position, worldPosition, Color.red);

            //Construir nuestro vector de rotacion

            Vector3 rotatedVector = Quaternion.AngleAxis(m_Angulo, Vector3.up) * worldPosition;
           // Debug.DrawLine(m_target.position, rotatedVector, Color.green);

            //Mover Nuestra posicion
            Vector3 flatTargetPosition = m_target.position;
            //flatTargetPosition.y = 0f;
            Vector3 finalPosition = flatTargetPosition + rotatedVector;
            //Debug.DrawLine(m_target.position, finalPosition, Color.blue);

            transform.position = Vector3.SmoothDamp(transform.position,finalPosition, ref VelReferencia, m_SmoothSpeed) ;
            transform.LookAt(flatTargetPosition);

        }
        IEnumerator Tiempo()
        {
            

            yield return new WaitForSeconds(3.5F);
            
            m_target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            //canvas = GameObject.FindGameObjectWithTag("Panel Opciones").GetComponent<ProyectoTank.MenuInicio.MenuPrincipal>();
            ManejoCamara();
            canvas.SetActive(true);

        }
        #endregion
    }
}
