using UnityEngine;
using UnityEngine.EventSystems;

using Photon.Pun;

using System.Collections;

namespace ProyectoTank.Neetwork
{
  
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {


        #region Private Fields

        [Tooltip("La instancia del jugador local. Usa esto para saber si el jugador local est� representado en la Escena")]
        public static GameObject LocalPlayerInstance;
        [Tooltip("La salud actual de nuestro jugador")]
        public float Health = 1f;
        [Tooltip("Los Rayos a controlar")]
        [SerializeField]
        private GameObject beams;
        //Cierto, cuando el usuario est� disparando
        bool IsFiring;

        [Tooltip("La interfaz de usuario del jugador GameObject Prefab")]
        [SerializeField]
        public GameObject PlayerUiPrefab;
       [SerializeField]
        public GameObject controlesUI;
      
        public bool inmortal = false;
        public float tiempoInmortal = 2f;


        #endregion

        #region MonoBehaviour CallBacks
        /// <summary>
        /// Se llama al m�todo MonoBehaviour cuando el Collider 'otro' ingresa al disparador.
        /// Afecta la salud del jugador si el colisionador es un rayo
        /// Nota: al saltar y disparar al mismo, encontrar�s que el propio rayo del jugador se cruza consigo mismo
        /// Se podr�a mover el colisionador m�s lejos para evitar esto o verificar si el rayo pertenece al jugador.
        /// </summary>
        void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            // Solo estamos interesados en Beamers
            // deber�amos usar etiquetas, pero por el bien de la distribuci�n, simplemente verifiquemos por nombre.
            if (!other.name.Contains("Beam"))
            {
                return;
            }
            Health -= 0.1f;
        }
        /// <summary>
        /// Se llama al m�todo MonoBehaviour una vez por cuadro para cada 'otro' de Collider que est� tocando el gatillo.
        /// Vamos a afectar la salud mientras los rayos tocan al jugador
        /// </summary>
        /// <param name="other">Other.</param>
        void OnTriggerStay(Collider other)
        {
            //no hacemos nada si no somos el jugador local.
            if (!photonView.IsMine)
            {
                return;
            }
            // Solo estamos interesados en Beamers
            // deber�amos usar etiquetas, pero por el bien de la distribuci�n, simplemente verifiquemos por nombre.
            if (!other.name.Contains("Beam"))
            {
                return;
            }
            // afectamos lentamente la salud cuando el rayo nos golpea constantemente, por lo que el jugador debe moverse para evitar la muerte.
            Health -= 0.1f * Time.deltaTime;
        }

        /// <summary>
        /// M�todo MonoBehaviour invocado en GameObject por Unity durante la fase inicial de inicializaci�n.
        /// </summary>
        void Awake()
        {
           /* if (beams == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
            }
            else
            {
                beams.SetActive(false);
            }
            // #Important
            // utilizado en GameManager.cs: realizamos un seguimiento de la instancia de localPlayer para evitar la creaci�n de instancias
            // cuando los niveles est�n sincronizados*/
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;

            }
            // #Critical
            // marcamos como no destruir al cargar para que la instancia sobreviva a la sincronizaci�n de niveles,
            // brindando as� una experiencia perfecta cuando se cargan los niveles.
            DontDestroyOnLoad(this.gameObject);
        }
        void Start()
        {
            CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();
            //Controles();

            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> Componente CameraWork en playerPrefab.", this);
            }
            if (PlayerUiPrefab != null)
            {
                GameObject _uiGo = Instantiate(PlayerUiPrefab);
                _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab referencia en player Prefab.", this);
            }
          /* if (controlesUI != null)
            {
                GameObject _controlUI = Instantiate(controlesUI);
               // _controlUI.SendMessage("UITarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> Referencia de controlesUI en el reproductor Prefab.", this);
            }*/
#if UNITY_5_4_OR_NEWER
            // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif

        }

        /// <summary>
        /// M�todo MonoBehaviour invocado en GameObject por Unity en cada fotograma.
        /// </summary>
        void Update()
        {
            if (photonView.IsMine)
            {
               // Controles();

                if (Health <= 0f)
                {
                    GameManager.Instance.LeaveRoom();
                }
            }

            // activa el estado activo de los haces
            if (beams != null && IsFiring != beams.activeInHierarchy)
            {
                beams.SetActive(IsFiring);
            }
        }
#if !UNITY_5_4_OR_NEWER
/// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
void OnLevelWasLoaded(int level)
{
    this.CalledOnLevelWasLoaded(level);
}
#endif


        void CalledOnLevelWasLoaded(int level)
        {
            // verificar si estamos fuera de la Arena y, si es el caso, generar alrededor del centro de la arena en una zona segura
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }
            GameObject _uiGo = Instantiate(this.PlayerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
           // Controles();
        }
#if UNITY_5_4_OR_NEWER
        public override void OnDisable()
        {
            // Always call the base to remove callbacks
            base.OnDisable();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }
#endif

        #endregion
        #region IPunObservable implementation


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // Somos due�os de este jugador: env�a a los dem�s nuestros datos
                stream.SendNext(IsFiring);
                stream.SendNext(Health);
            }
            else
            {
                // Reproductor de red, recibir datos
                this.IsFiring = (bool)stream.ReceiveNext();
                this.Health = (float)stream.ReceiveNext();
            }
        }
        #endregion
        #region Custom
        public void DarVida(float vida)
        {
            Health += vida;
        }
        public void QuitarVida(float da�o)
        {
            if (inmortal) return;
            Health -= da�o;
            StartCoroutine(TiempoInmortal());
        }

        IEnumerator TiempoInmortal()
        {
            inmortal = true;
            yield return new WaitForSeconds(tiempoInmortal);
            inmortal = false;

        }
        public void Controles()
        {
            if (controlesUI != null)
            {
                GameObject _controlUI = Instantiate(controlesUI);
                // _controlUI.SendMessage("UITarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }
          
        }

        /// <summary>
        /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
        /// </summary>
        void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!IsFiring)
                {
                    IsFiring = true;
                }
            }
            if (Input.GetButtonUp("Fire1"))
            {
                if (IsFiring)
                {
                    IsFiring = false;
                }
            }
        }
#if UNITY_5_4_OR_NEWER
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }
#endif

        #endregion
    }
}