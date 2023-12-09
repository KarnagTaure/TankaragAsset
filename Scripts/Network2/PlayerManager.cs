
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Photon.Pun;
using ProyectoTank.Neetwork;
using UnityEngine.UI;

namespace ProjectoTank
{
#pragma warning disable 649

    /// <summary>
    ///Administrador de jugadores.
    /// Maneja la entrada de fuego y las vigas.
    /// </summary>
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Public Fields

        [Tooltip("La instancia del jugador local. Usa esto para saber si el jugador local está representado en la Escena")]
        public static GameObject LocalPlayerInstance;

        [Header("Sistema Vida")]
        [Tooltip("La salud actual de nuestro jugador.")]
        public float Health;

        [Tooltip("La salud actual de nuestro jugador.")]
        public float HealthMax = 10f;

        [Tooltip("Canvas de gameover")]
        public GameObject gameOver;

        [Tooltip("Prefab del tanque destruido")]
        public GameObject tanqueDestruido;

        public bool inmortal = false;

        public float tiempoInmortal = 2f;

        [Header("Disparo")]

        [Tooltip("Prefab de Municion que disparar")]
        [SerializeField]
        private GameObject Municion;

        [Tooltip("Ubicacion desde donde sale la Municion")]
        public Transform emisorBala;

        [Tooltip("Prefab de Municion que disparar")]
        public Transform emisorParticulas;

        [Tooltip("Prefab de Municion que disparar")]
        public GameObject particulasBalaPrefa;

        [Tooltip("Velocidad de la Municion")]
        public float potenciaTiro;

        [Tooltip("Particulas de explosion")]
        public GameObject explosionParticulas;

        [Tooltip("Particulas detonacion ")]
        public GameObject DetonacionParticula;

        private float firenex = 1.5f;

        [Tooltip("Tiempo entre disparo y disparo")]
        public float tiempoDisparo = 1f;

        public bool puedeDisparar = true;

        [SerializeField]
        private GameObject btnFuego;

        #endregion

        #region Private Fields

        [Tooltip("La interfaz de usuario del jugador GameObject Prefab")]
        [SerializeField]
        private GameObject playerUiPrefab;







        //Cierto, cuando el usuario está disparando
        bool IsFiring;

        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// Método MonoBehaviour invocado en GameObject por Unity durante la fase inicial de inicialización. 
        /// </summary>
        public void Awake()
        {
            gameOver = GameObject.Find("GameOver");
            // #Important
            //utilizado en GameManager.cs: realizamos un seguimiento de la instancia localPlayer para evitar la instanciación cuando los niveles están sincronizados
            if (photonView.IsMine)
            {
                LocalPlayerInstance = gameObject;
            }
            this.Health = this.HealthMax;

            // #Critical
            // marcamos como no destruir al cargar para que la instancia sobreviva a la sincronización de niveles,
            // brindando así una experiencia perfecta cuando se cargan los niveles.
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Método MonoBehaviour invocado en GameObject por Unity durante la fase de inicialización.
        /// </summary>
        public void Start()
        {


            CameraWork _cameraWork = gameObject.GetComponent<CameraWork>();

            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><b>Missing</b></Color> CameraWork Component on player Prefab.", this);
            }

            // Create the UI
            if (this.playerUiPrefab != null)
            {
                GameObject _uiGo = Instantiate(this.playerUiPrefab);
                _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><b>Missing</b></Color> PlayerUiPrefab reference on player Prefab.", this);
            }
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            btnFuego = GameObject.Find("Fuego");
            btnFuego.GetComponent<Button>().onClick.AddListener(Fuego);

#if UNITY_5_4_OR_NEWER
            // Unity 5.4 tiene una nueva gestión de escenas. registre un método para llamar a CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
        }



        public override void OnDisable()
        {
            // Llame siempre a la base para eliminar las devoluciones de llamada
            base.OnDisable();


#if UNITY_5_4_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
#endif
        }


        /// <summary>
        /// Método MonoBehaviour invocado en GameObject por Unity en cada fotograma.
        /// Entradas de proceso si jugador local.
        /// Mostrar y ocultar las vigas
        /// Esté atento al final del juego, cuando la salud del jugador local es 0.
        /// </summary>
        public void Update()
        {

            if (btnFuego == null)
            {
                if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
                {
                    return;
                }
                btnFuego = GameObject.Find("Fuego");
                btnFuego.GetComponent<Button>().onClick.AddListener(Fuego);

            }
            // solo procesamos entradas y verificamos la salud si somos el jugador local
            if (photonView.IsMine)
            {
                this.ProcessInputs();

                if (this.Health <= 0f)
                {
                    photonView.RPC("Muerte", RpcTarget.All); 
                   
                }
            }

           
        }

       


#if !UNITY_5_4_OR_NEWER
        /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }
#endif


        /// <summary>
        /// Método MonoBehaviour llamado después de cargar un nuevo nivel de índice 'nivel'.
        /// Recreamos la interfaz de usuario del jugador porque se destruyó cuando cambiamos de nivel.
        /// También cambia la posición del jugador si está fuera de la arena actual.
        /// </summary>
        /// <param name="level">Level index loaded</param>
        void CalledOnLevelWasLoaded(int level)
        {
            // verificar si estamos fuera de la Arena y, si es el caso, generar alrededor del centro de la arena en una zona segura
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }

            GameObject _uiGo = Instantiate(this.playerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }

        #endregion

        #region Private Methods


#if UNITY_5_4_OR_NEWER
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }
#endif

        /// <summary>
        /// Procesa las entradas. Esto SÓLO DEBE UTILIZARSE cuando el jugador tiene autoridad sobre este GameObject en red (photonView.isMine == true)
        /// </summary>
        void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                // no queremos disparar cuando interactuamos con los botones de la interfaz de usuario, por ejemplo. IsPointerOverGameObject realmente significa IsPointerOver*UI*GameObject
                // observe que no usamos on on GetbuttonUp() pocas líneas hacia abajo, porque uno puede mover el mouse hacia abajo, moverse sobre un elemento de la interfaz de usuario y soltar, lo que conduciría a no bajar la bandera isFiring.
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    //	return;
                }

                if (!this.IsFiring)
                {
                    this.IsFiring = true;
                }
            }

            if (Input.GetButtonUp("Fire1"))
            {
                if (this.IsFiring)
                {
                    this.IsFiring = false;
                }
            }
        }

        public void Fuego()
        {
            if (Time.time > firenex)
            {

                var proyectil = PhotonNetwork.Instantiate(Municion.name, emisorBala.position, emisorBala.rotation);

                Instantiate(explosionParticulas, emisorBala.position, emisorBala.rotation);


                proyectil.GetComponent<Rigidbody>().velocity = proyectil.transform.forward * potenciaTiro;
                proyectil.transform.Rotate(-0, 0, 0);

                firenex = Time.time + tiempoDisparo;


                Destroy(proyectil, 1.6f);
            }


        }
        public void DarVida(float vida)
        {
            Health += vida;
        }
        [PunRPC]
        public void QuitarVida(float daño)
        {
            if (inmortal) return;
            this.Health -= daño;
            StartCoroutine(TiempoInmortal());
        }
        [PunRPC]
        public void Muerte()
        {

            Destroy(this.gameObject);
            PhotonNetwork.Instantiate(tanqueDestruido.name, transform.position, Quaternion.identity);
           
                this.gameOver.SetActive(true);
           
            StartCoroutine(Tiempomortal());

        }

        IEnumerator TiempoInmortal()
        {
            this.inmortal = true;
            yield return new WaitForSeconds(tiempoInmortal);
            this.inmortal = false;

        }
        IEnumerator Tiempomortal()
        {
           
            yield return new WaitForSeconds(6);
            GameManager.Instance.LeaveRoom();
            

        }
        #endregion

        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // Somos dueños de este jugador: envía a los demás nuestros datos
                stream.SendNext(this.IsFiring);
                stream.SendNext(this.Health);
            }
            else
            {
                // Reproductor de red, recibir datos
                this.IsFiring = (bool)stream.ReceiveNext();
                this.Health = (float)stream.ReceiveNext();
            }
        }

        #endregion
    }
}