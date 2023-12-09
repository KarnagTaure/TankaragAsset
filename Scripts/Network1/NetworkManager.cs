using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

namespace ProyectoTank.Neetwork
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {

        public int maxJugadores = 4;
        public static NetworkManager instancia;

        private void Awake()
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);

        }
        void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("Conectados al servidor pincipal");
            PhotonNetwork.JoinLobby();
        }
        public void CrearRoom(string nombre)
        {
            RoomOptions opciones = new RoomOptions
            {
                MaxPlayers = (byte)maxJugadores
            };
            PhotonNetwork.CreateRoom(nombre, opciones);
            Debug.Log("RoomCreado");
        }

        public void Unirseroom(string nombre)
        {
            PhotonNetwork.JoinRoom(nombre);
        }

        [PunRPC]
        public void CambiarEscena(string escena)
        {
            PhotonNetwork.LoadLevel(escena);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Desconectado por " +  cause);
            PhotonNetwork.LoadLevel("Menu Principal");

        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //GameManager.instance.CheckwinCondition();
            }
        }
    }
}