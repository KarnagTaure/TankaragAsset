using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace ProyectoTank.Neetwork
{

    public class MenuControladorNetwork : MonoBehaviourPunCallbacks, ILobbyCallbacks
    {
        [Header("Pantallas")]
        [SerializeField] private GameObject menuPrincipal;
        [SerializeField] private GameObject crearRoomPantalla;
        [SerializeField] private GameObject lobbyPantalla;
        [SerializeField] private GameObject lobbyNavegadorPantalla;

        [Header("Menu Principal")]
        [SerializeField] private Button btnCrearRoom;
        [SerializeField] private Button btnEncontrarRoom;

        [Header("Looby")]
        [SerializeField] private TextMeshProUGUI txListaJugadores;
        [SerializeField] private TextMeshProUGUI txInforoom;
        [SerializeField] private Button btnIniciarjuego;

        [Header("Lobby Navegador")]
        [SerializeField] private RectTransform RoomContenedor;
        [SerializeField] private GameObject roomElementoPrefab;



         private List<GameObject> roomElementos = new List<GameObject>();
         private List<RoomInfo> listaRoom = new List<RoomInfo>();
        private void Start()
        {
            btnCrearRoom.interactable = false;
            btnEncontrarRoom.interactable = false;

            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.CurrentRoom.IsVisible = true;
                PhotonNetwork.CurrentRoom.IsOpen = true;
            }
        }

        void SetPantalla(GameObject screen)
        {
            menuPrincipal.SetActive(false);
            crearRoomPantalla.SetActive(false);
            lobbyPantalla.SetActive(false);
            lobbyNavegadorPantalla.SetActive(false);

            screen.SetActive(true);

           if (screen == lobbyNavegadorPantalla)
              ActualizarLobbyNavegador();
        }
        public void OnNombreJugadorCambia(TMP_InputField inpJugadorNombre)
        {
            PhotonNetwork.NickName = inpJugadorNombre.text;
           // Debug.Log({ PhotonNetwork.NickName});
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("Conectado");
            btnCrearRoom.interactable = true;
            btnEncontrarRoom.interactable = true;
        }
        public void OnCrearRoomClicked()
        {
            SetPantalla(crearRoomPantalla);
        }
        public void OnEncontrarRoomClicked()
        {
            SetPantalla(lobbyNavegadorPantalla);
        }
        public void OnRegresarClicked()
        {
            SetPantalla(menuPrincipal);
        }
        public void OnCrearRoomBoton(TMP_InputField nombre)
        {
            Debug.Log("OncrearRoomBoton");
            NetworkManager.instancia.CrearRoom(nombre.text);
        }
        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoindRoom");
            SetPantalla(lobbyPantalla);
            photonView.RPC("ActualizarLobby", RpcTarget.All);
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
           ActualizarLobby();
        }
        
        [PunRPC]
        void ActualizarLobby()
        {
            btnIniciarjuego.interactable = PhotonNetwork.IsMasterClient;

            txListaJugadores.text = "";

            foreach(Player p in PhotonNetwork.PlayerList)
            {
                txListaJugadores.text += p.NickName + "\n";

            }

            txInforoom.text = string.Format(@"<b>Nombre Room: <b>{0}{1}", "\n", PhotonNetwork.CurrentRoom.Name);

        }
        public void OnIniciarJuegoClicked()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            NetworkManager.instancia.photonView.RPC("CambiarEscena", RpcTarget.All, "JuegoNetwork");
        }
        public void OnSalirLobby()
        {
            Debug.Log("Salio del Lobby");
            PhotonNetwork.LeaveLobby();
            SetPantalla(menuPrincipal);
        }
        private GameObject CrearRoomBoton()
        {
            //Debug.Log("crearRoomBoton");
            GameObject obj = Instantiate(roomElementoPrefab, RoomContenedor.transform);
            roomElementos.Add(obj);
            return obj;
        }
        void ActualizarLobbyNavegador()
        {
            foreach (GameObject b in roomElementos)
            {
                b.SetActive(false);
            }

            for (int x=0; x < listaRoom.Count; x++)
            {
                GameObject boton = x >= roomElementos.Count ? CrearRoomBoton() : roomElementos[x];

                boton.SetActive(true);

                boton.transform.Find("txtNombreRoom").GetComponent<TextMeshProUGUI>().text = listaRoom[x].Name;
                boton.transform.Find("txtCantidadJugadores").GetComponent<TextMeshProUGUI>().text = listaRoom[x].PlayerCount + "/" + listaRoom[x].MaxPlayers;

                Button b1 = boton.GetComponent<Button>();
                string nombre = listaRoom[x].Name;
                b1.onClick.RemoveAllListeners();
                b1.onClick.AddListener(() => { OnUnirseRoomClicked(nombre); });



            }
            
            
        }
        public void OnRefrescarClicked()
        {
            ActualizarLobbyNavegador();
        }
        private void OnUnirseRoomClicked(string nombre)
        {
                NetworkManager.instancia.Unirseroom(nombre);
        }
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            listaRoom = roomList;
        }
       /* public void CargarMenu()
        {
            SceneManager.LoadScene("Menu");
        }*/
    }

    

}