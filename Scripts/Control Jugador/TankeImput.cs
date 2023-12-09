using UnityEngine;
using Photon.Pun;


namespace ProyectoTank.Controles
{

    public class TankeImput : MonoBehaviourPunCallbacks
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
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            if (variableJoystick == null)
            {
                variableJoystick = GameObject.FindWithTag("Izq").GetComponent<VariableJoystick>();

            }


            var h = variableJoystick.Horizontal;
            var v = variableJoystick.Vertical;
            InputVector = new Vector2(h, v);

            MousePosition = Input.mousePosition;

            
        }

    }
}
