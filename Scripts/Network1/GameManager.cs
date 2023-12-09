using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


namespace ProyectoTank.Neetwork
{
    public class GameManager : MonoBehaviourPunCallbacks
    {


        #region Photon Callbacks


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }


        #endregion


        #region Public Methods
        [Tooltip("El prefabricado a utilizar para representar al jugador.")]
        public GameObject playerPrefab;
        

        public static GameManager Instance;

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        void Start()
        {
            Instance = this;

            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> Referencia de playerPrefab. Configúralo en GameObject 'Game Manager'", this);
            }
            else
            {
               
                    if (PlayerManager.LocalPlayerInstance == null)
                    {
                        Debug.LogFormat("Estamos instanciando LocalPlayer desde {0}", SceneManagerHelper.ActiveSceneName);
                        // estamos en una habitación. genera un personaje para el jugador local. se sincroniza usando PhotonNetwork.Instantiate
                        PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                       // playerPrefab.SetActive(true);
                    }
                    else
                    {
                        Debug.LogFormat("Ignorando la carga de escena para {0}", SceneManagerHelper.ActiveSceneName);
                    }
               
            }
        }


        #endregion

        #region Private Methods


        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Intentando cargar un nivel pero no somos el cliente maestro");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }


        #endregion

        #region Photon Callbacks


        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // No se ve si eres el jugador conectando


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // llamado antes de OnPlayerLeftRoom


                LoadArena();
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // Visto cuando otros se desconecta

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // llamado antes de OnPlayerLeftRoom


                LoadArena();
            }
        }


        #endregion
    }
}