using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace ProyectoTank.Controles
{

    public class TorretaInput : MonoBehaviourPunCallbacks
    {

        public VariableJoystick variableJoystick;

        public Vector2 InputVector { get; private set; }

        public Vector3 MousePosition { get; private set; }

        private void Awake()
        {
            
        }
        // Update is called once per frame
        void Update()
        {
            
            if (photonView.IsMine)
            {
                if (variableJoystick == null)
                {
                   variableJoystick = GameObject.FindWithTag("Drch").GetComponent<VariableJoystick>();

                }

                var h = variableJoystick.Horizontal;
                var v = variableJoystick.Vertical;
                InputVector = new Vector2(h, v);

                MousePosition = Input.mousePosition;
            }
            else
            {
                return;
            }
        }
    }
}