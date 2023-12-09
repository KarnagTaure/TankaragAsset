using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProyectoTank.Neetwork
{
    /// <summary>
    /// Camera work. Follow a target
    /// </summary>
    public class CameraWork : MonoBehaviour
    {
        #region Private Fields
        public static CameraWork Instance;

        /* [Tooltip("La distancia en el plano x-z local al objetivo")]
         [SerializeField]
         private float distance = 7.0f;


         [Tooltip("La altura a la que queremos que esté la cámara sobre el objetivo")]
         [SerializeField]
         private float height = 3.0f;


         [Tooltip("Permita que la cámara se desplace verticalmente desde el objetivo, por ejemplo, brindando más vista del paisaje y menos terreno.")]
         [SerializeField]
         private Vector3 centerOffset = Vector3.zero;


         [Tooltip("Establezca esto como falso si un componente de un prefabricado está siendo instanciado por Photon Network, y llame manualmente a OnStartFollowing() cuando sea necesario".)]
         [SerializeField]
         private bool followOnStart = false;


         [Tooltip("The Smoothing for the camera to follow the target")]
         [SerializeField]
         private float smoothSpeed = 0.125f;*/


        // transformación en caché del objetivo
        Transform cameraTransform;


        // maintain a flag internally to reconnect if target is lost or camera is switched
        bool isFollowing;

        [Tooltip("Establezca esto como falso si un componente de un prefabricado está siendo instanciado por Photon Network, y llame manualmente a OnStartFollowing() cuando sea necesario")]
        [SerializeField]
        private bool followOnStart = false;

        [Tooltip("La altura a la que queremos que esté la cámara sobre el objetivo")]
        [SerializeField]
        public float m_Altura = 10f;

        [Tooltip("La distancia en el plano x-z local al objetivo")]
        [SerializeField]
        public float m_Distancia = 20f;

        [Tooltip("Angulo de rotacion de la camara")]
        [SerializeField]
        public float m_Angulo = 45f;

        [Tooltip("The Smoothing for the camera to follow the target")]
        [SerializeField]
        private float m_SmoothSpeed = 1f;

        public Vector3 VelReferencia;

        // Cache for camera offset
        Vector3 cameraOffset = Vector3.zero;


        #endregion


        #region MonoBehaviour Callbacks


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase
        /// </summary>
        void Start()
        {
            Instance = this;
           
            // Start following the target if wanted.
            if (followOnStart)
            {
                OnStartFollowing();
            }
        }


        void LateUpdate()
        {
            //El objetivo de transformación no puede destruirse con carga nivelada,
            // así que necesitamos cubrir los casos de esquina donde la cámara principal es diferente cada vez que cargamos una nueva escena,
            // y volver a conectarnos cuando eso suceda
            if (cameraTransform == null && isFollowing)
            {
                OnStartFollowing();
            }


            // only follow is explicitly declared
            if (isFollowing)
            {
                // Follow();
                ManejoCamara();
            }
        }


        #endregion


        #region Public Methods


        /// <summary>
        /// Plantea el evento de inicio siguiente.
        /// Utilícelo cuando no sepa en el momento de editar qué seguir, por lo general instancias administradas por la red de fotones.
        /// </summary>
        public void OnStartFollowing()
        {
            cameraTransform = Camera.main.transform;
            isFollowing = true;
            //no suavizamos nada, vamos directamente a la toma de cámara correcta
            Cut();
        }


        #endregion


        #region Private Methods


        /// <summary>
        /// Follow the target smoothly
        /// </summary>
       /*void Follow()
        {
            cameraOffset.z = -distance;
            cameraOffset.y = height;


            cameraTransform.position = Vector3.Lerp(cameraTransform.position, this.transform.position + this.transform.TransformVector(cameraOffset), smoothSpeed * Time.deltaTime);


            cameraTransform.LookAt(this.transform.position + centerOffset);
        }*/
        protected void ManejoCamara()
        {
            
            //Construir el vector de posicion Mundial
            Vector3 worldPosition = (Vector3.forward * -m_Distancia) + (Vector3.up * m_Altura);
            //Debug.DrawLine(m_target.position, worldPosition, Color.red);

            //Construir nuestro vector de rotacion

            Vector3 rotatedVector = Quaternion.AngleAxis(m_Angulo, Vector3.up) * worldPosition;
            // Debug.DrawLine(m_target.position, rotatedVector, Color.green);

            //Mover Nuestra posicion
            Vector3 flatTargetPosition = this.transform.position;
            //flatTargetPosition.y = 0f;
            Vector3 finalPosition = flatTargetPosition + rotatedVector;
            //Debug.DrawLine(m_target.position, finalPosition, Color.blue);
            
            cameraTransform.position = Vector3.Lerp(transform.position, finalPosition,  m_SmoothSpeed);
            cameraTransform.transform.LookAt(flatTargetPosition);

        }


        void Cut()
        {
            cameraOffset.z = -m_Distancia;
            cameraOffset.y = m_Altura;

            Vector3 worldPosition = (Vector3.forward * -m_Distancia) + (Vector3.up * m_Altura);
            //Debug.DrawLine(m_target.position, worldPosition, Color.red);
            Vector3 rotatedVector = Quaternion.AngleAxis(m_Angulo, Vector3.up) * worldPosition;
            // Debug.DrawLine(m_target.position, rotatedVector, Color.green);

            //Mover Nuestra posicion
            Vector3 flatTargetPosition = this.transform.position;
            //flatTargetPosition.y = 0f;
            Vector3 finalPosition = flatTargetPosition + rotatedVector;
            //Debug.DrawLine(m_target.position, finalPosition, Color.blue);

            cameraTransform.position = finalPosition;


            cameraTransform.transform.LookAt(flatTargetPosition);
        }
        #endregion
    }
}