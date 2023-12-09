using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace ProjectoTank
{
#pragma warning disable 649

	/// <summary>
	///Administrador de juegos.
	/// Conecta y mira Photon Status, Instancia Player
	/// Se ocupa de salir de la habitación y del juego.
	/// Se ocupa de la carga de nivel (fuera de la sincronización en la habitación)
	/// </summary>
	public class GameManager : MonoBehaviourPunCallbacks
	{

		#region Public Fields

		static public GameManager Instance;

		#endregion

		#region Private Fields

		private GameObject instance;

		[Tooltip("El prefabricado a utilizar para representar al jugador.")]
		[SerializeField]
		private Transform[] posicionSpawn;

		[Tooltip("El prefabricado a utilizar para representar al jugador.")]
		[SerializeField]
		private GameObject playerPrefab;

		#endregion

		#region MonoBehaviour CallBacks

		/// <summary>
		/// Método MonoBehaviour invocado en GameObject por Unity durante la fase de inicialización.
		/// </summary>
		public override void OnEnable()
		{
			base.OnEnable();

			//CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
		}
		void Start()
		{
			Instance = this;
			

			// en caso de que comencemos esta demostración con la escena incorrecta activa, simplemente cargue la escena del menú
			if (!PhotonNetwork.IsConnected)
			{
				SceneManager.LoadScene("Menu Network");

				return;
			}
			Hashtable props = new Hashtable
			{
				{TankGame.PLAYER_LOADED_LEVEL, true}
			};
			PhotonNetwork.LocalPlayer.SetCustomProperties(props);
			if (playerPrefab == null)
			{ // #Consejo Nunca asuma que las propiedades públicas de los componentes se llenan correctamente,
			  // siempre verifique e informe al desarrollador de ello.

				Debug.LogError("<Color=Red><b>Missing</b></Color> Referencia de playerPrefab. Configúralo en GameObject'Game Manager'", this);
			}
			else
			{


				if (PlayerManager.LocalPlayerInstance == null)
				{
					Debug.LogFormat("Estamos instanciando LocalPlayer desde {0}", SceneManagerHelper.ActiveSceneName);

					// estamos en una habitación. genera un personaje para el jugador local. se sincroniza usando PhotonNetwork.Instantiate
					PhotonNetwork.Instantiate(this.playerPrefab.name, posicionSpawn[Random.Range(0, posicionSpawn.Length)].position, Quaternion.identity, 0);
				}
				else
				{

					Debug.LogFormat("Ignorando la carga de escena para {0}", SceneManagerHelper.ActiveSceneName);
				}


			}
			
		}
		public override void OnDisable()
		{
			base.OnDisable();

			//CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
		}

		/// <summary>
		/// Método MonoBehaviour invocado en GameObject por Unity en cada fotograma.
		/// </summary>
		void Update()
		{
			// El botón "atrás" del teléfono equivale a "Escape". salir de la aplicación si eso está presionado
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				QuitApplication();
			}
		}



		#endregion
		#region COROUTINES
		private IEnumerator EndOfGame()
		{
			float timer = 5.0f;

			while (timer > 0.0f)
			{
				//InfoText.text = string.Format("Player {0} won with {1} points.\n\n\nReturning to login screen in {2} seconds.", winner, score, timer.ToString("n2"));

				yield return new WaitForEndOfFrame();

				timer -= Time.deltaTime;
			}

			PhotonNetwork.LeaveRoom();
		}

		#endregion

		#region Photon Callbacks

		public override void OnDisconnected(DisconnectCause cause)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Menu Network");
		}

		/// <summary>
		/// Llamado cuando un Photon Player se conectó. Entonces necesitamos cargar una escena más grande.
		/// </summary>
		/// <param name="other">Other.</param>
		public override void OnPlayerEnteredRoom(Player other)
		{
			Debug.Log("OnPlayerEnteredRoom() " + other.NickName); // No se ve si eres el jugador conectando

			if (PhotonNetwork.IsMasterClient)
			{
				Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

				LoadArena();
			}
		}

		/// <summary>
		/// Llamado cuando un Photon Player se desconectó. Necesitamos cargar una escena más pequeña.
		/// </summary>
		/// <param name="other">Other.</param>
		public override void OnPlayerLeftRoom(Player other)
		{
			Debug.Log("OnPlayerLeftRoom() " + other.NickName); // Visto cuando otros se desconecta

			if (PhotonNetwork.IsMasterClient)
			{
				Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

				LoadArena();
			}
			 
		}

		/// <summary>
		/// Llamado cuando el jugador local salió de la habitación. Necesitamos cargar la escena del lanzador.
		/// </summary>
		public override void OnLeftRoom()
		{
			SceneManager.LoadScene("Menu Network");
			//PhotonNetwork.Disconnect();
		}

		#endregion

		#region Public Methods

		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
		}

		public void QuitApplication()
		{
			Application.Quit();
		}

		#endregion

		#region Private Methods
		private void StartGame()
		{
			Debug.Log("StartGame!");

			if (playerPrefab == null)
			{ // #Consejo Nunca asuma que las propiedades públicas de los componentes se llenan correctamente,
			  // siempre verifique e informe al desarrollador de ello.

				Debug.LogError("<Color=Red><b>Missing</b></Color> Referencia de playerPrefab. Configúralo en GameObject'Game Manager'", this);
			}
			else
			{


				if (PlayerManager.LocalPlayerInstance == null)
				{
					Debug.LogFormat("Estamos instanciando LocalPlayer desde {0}", SceneManagerHelper.ActiveSceneName);

					// estamos en una habitación. genera un personaje para el jugador local. se sincroniza usando PhotonNetwork.Instantiate
					PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
				}
				else
				{

					Debug.LogFormat("Ignorando la carga de escena para {0}", SceneManagerHelper.ActiveSceneName);
				}


			}

		}

		private void OnCountdownTimerIsExpired()
		{
			StartGame();
		}
		void LoadArena()
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				Debug.LogError("PhotonNetwork : Intentando cargar un nivel pero no somos el cliente maestro");
			}

			Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);

			//PhotonNetwork.LoadLevel("PunBasics-Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
			PhotonNetwork.LoadLevel("Nivel Network");
		}

		#endregion

	}

}