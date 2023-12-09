using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace ProyectoTank.Neetwork
{
    public class Lanzador : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        /// <summary>
        /// El n�mero m�ximo de jugadores por sala. Cuando una sala est� llena, no pueden unirse nuevos jugadores, por lo que se crear� una nueva sala.
        /// </summary>
        [Tooltip("El n�mero m�ximo de jugadores por sala. Cuando una sala est� llena, no pueden unirse nuevos jugadores, por lo que se crear� una nueva sala.")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;
        #endregion

        #region Private Fields

        [Tooltip("El panel Ui para permitir que el usuario ingrese el nombre, se conecte y juegue")]
        [SerializeField]
        private GameObject PanelInicio;
        [Tooltip("La etiqueta de la interfaz de usuario para informar al usuario que la conexi�n est� en curso")]
        [SerializeField]
        private GameObject PanelProgreso;
        /// <summary>
        /// Mantenga un registro del proceso actual. Dado que la conexi�n es as�ncrona y se basa en varias devoluciones de llamada de Photon,
        /// necesitamos realizar un seguimiento de esto para ajustar correctamente el comportamiento cuando recibimos una llamada de Photon.
        /// Por lo general, esto se usa para la devoluci�n de llamada OnConnectedToMaster().
        /// </summary>
        bool isConnecting;
        #endregion

        #region Private Fields


        /// <summary>
        /// El n�mero de versi�n de este cliente. Los usuarios est�n separados entre s� por gameVersion (que le permite realizar cambios importantes).
        /// </summary>
        string gameVersion = "1";


        #endregion


        #region MonoBehaviourPunCallbacks Callbacks


        /// <summary>
        /// M�todo MonoBehaviour invocado en GameObject por Unity durante la fase inicial de inicializaci�n.
        /// </summary>
        void Awake()
        {
            // #Critical
            // esto asegura que podamos usar PhotonNetwork.LoadLevel() en el cliente maestro y que todos los clientes en la misma sala sincronicen su nivel autom�ticamente
            PhotonNetwork.AutomaticallySyncScene = true;
        }


        /// <summary>
        /// M�todo MonoBehaviour invocado en GameObject por Unity durante la fase de inicializaci�n.
        /// </summary>
        void Start()
        {
            PanelProgreso.SetActive(false);
            PanelInicio.SetActive(true);
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster() Fue Llamado");
            // no queremos hacer nada si no estamos intentando unirnos a una habitaci�n.
            // este caso donde isConnecting es falso es t�picamente cuando perdiste o saliste del juego, cuando se carga este nivel, se llamar� a OnConnectedToMaster, en ese caso
            // no queremos hacer nada.
            if (isConnecting)
            {
                // #Critical: Lo primero que tratamos de hacer es unirnos a una habitaci�n potencial existente. Si lo hay, bueno, de lo contrario, nos devolver�n la llamada con OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            PanelProgreso.SetActive(false);
            PanelInicio.SetActive(true);
            isConnecting = false;
            Debug.LogWarningFormat("OnDisconnected() Fue llamado conrazon de {0}", cause);
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed() Fue llamado. No hay ninguna habitaci�n aleatoria disponible, as� que creamos una.\nLlamando: PhotonNetwork.CreateRoom");

            // #Critico: fallamos al unirnos a una sala aleatoria, tal vez no exista ninguna o est�n todas llenas. No te preocupes, creamos una nueva habitaci�n.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log(" OnJoinedRoom() Fue llamado y Ahora este cliente est� en una habitaci�n.");
            // #Critical: Solo cargamos si somos el primer jugador, de lo contrario confiamos en `PhotonNetwork.AutomaticallySyncScene` para sincronizar nuestra escena de instancia.
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("Cargamos la 'Habitaci�n para 1' ");


                // #Critical
                // Cargar el nivel de la habitaci�n.
                PhotonNetwork.LoadLevel("Room for 1");
            }
        }

        #endregion


        #region Metodos Publicos


        /// <summary>
        /// Inicie el proceso de conexi�n.
        /// - Si ya est� conectado, intentamos unirnos a una habitaci�n aleatoria
        /// - si a�n no est� conectado, conecte esta instancia de aplicaci�n a Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            PanelProgreso.SetActive(true);
            PanelInicio.SetActive(false);

            // comprobamos si estamos conectados o no, nos unimos si lo estamos, sino iniciamos la conexi�n al servidor.
            if (PhotonNetwork.IsConnected)
            {
                //#Critical necesitamos en este punto intentar unirnos a una habitaci�n aleatoria. Si falla, recibiremos una notificaci�n en OnJoinRandomFailed() y crearemos uno.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, ante todo debemos conectarnos al Photon Online Server.
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        public void Salir()
        {
            Application.Quit();
        }
        #endregion


    }
}