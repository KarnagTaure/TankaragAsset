using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace ProyectoTank.Controles.torreta
{
    public class ControlTorretaPlayer : MonoBehaviourPunCallbacks
    {
        private TorretaInput _input;

        [SerializeField]
        private float moveSpeed;
        [SerializeField]
        private float rotateSpeed;
        [SerializeField]
        private bool rotateHaciaRaton;

        // [SerializeField]
        private Camera camara;

        // [SerializeField]
        public Transform m_mirilla;

        //public Transform mirilla;

        private void Awake()
        {
            
           // m_mirilla = GameObject.Find("Mirilla").GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            if(m_mirilla == null)
            {
                //m_mirilla = GameObject.Find("Mirilla").GetComponent<Transform>();

            }
            if(_input == null)
            {
                _input = GetComponent<TorretaInput>();

            }
            if( camara == null)
            {
                camara = GameObject.Find("Main Camera").GetComponent<Camera>();
            
            }

            

            var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);

            //Movimiento en direccion donde aputnamos
            var movementVector = MovehaciaTarget(targetVector);
            var movimientoTorre = Movetorre(targetVector);

            if (!rotateHaciaRaton)
            {
                RotateHaciaMovementVector(movementVector);
            }
            if (rotateHaciaRaton)
            {
                //RotateHaciaMovementVector(movimientoTorre);
                RotateFromMouseVector();
            }


        }

        private void RotateFromMouseVector()
        {
            Ray ray = camara.ScreenPointToRay(m_mirilla.transform.position);


            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
            {
                var target = hitInfo.point;

                target.y = transform.position.y;


                transform.LookAt(target);
            }

        }
       private void RotateHaciaMovementVector(Vector3 movementVector)
        {
            if (movementVector.magnitude == 0) { return; }
            var rotation = Quaternion.LookRotation(movementVector);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);

        }
        private Vector3 MovehaciaTarget(Vector3 targetVector)
        {
            var speed = moveSpeed * Time.deltaTime;

            //El movimiento respecto a la posicion de la camara
            targetVector = Quaternion.Euler(0, camara.gameObject.transform.eulerAngles.y, 0) * targetVector;

            var targetPosition = transform.position + targetVector * speed;
            transform.position = targetPosition;



            return targetVector;
        }
        private Vector3 Movetorre(Vector3 targetVector)
        {
            var speed = moveSpeed * Time.deltaTime;

            //El movimiento respecto a la posicion de la camara
            targetVector = Quaternion.Euler(0, camara.gameObject.transform.eulerAngles.y, 0) * targetVector;

            var targetPosition = transform.position;
            transform.position = targetPosition;



            return targetVector;
        }
       
    }
}
