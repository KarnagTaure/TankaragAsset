using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

namespace ProjectoTank
{
#pragma warning disable 649

	/// <summary>
	/// IU del jugador. Restrinja la interfaz de usuario para seguir un GameObject de PlayerManager en el mundo,
	/// Afecta un control deslizante y texto para mostrar el nombre y la salud del jugador
	/// </summary>
	public class PlayerUI : MonoBehaviour
	{
		#region Private Fields

		[Tooltip("Desplazamiento de píxeles desde el objetivo del jugador")]
		[SerializeField]
		private Vector3 screenOffset = new Vector3(0f, 45f, 0f);

		[Tooltip("Texto de la interfaz de usuario para mostrar el nombre del jugador")]
		[SerializeField]
		private TextMeshProUGUI playerNameText;

		[Tooltip("Control deslizante de interfaz de usuario para mostrar la salud del jugador")]
		[SerializeField]
		private Slider playerHealthSlider;

		PlayerManager target;

		float characterControllerHeight;

		Transform targetTransform;

		Renderer targetRenderer;

		CanvasGroup _canvasGroup;

		Vector3 targetPosition;

		#endregion

		#region MonoBehaviour Messages

		/// <summary>
		/// Método MonoBehaviour invocado en GameObject por Unity durante la fase de inicialización temprana
		/// </summary>
		void Awake()
		{

			_canvasGroup = this.GetComponent<CanvasGroup>();

			this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
			
		}

        /// <summary>
        /// Método MonoBehaviour invocado en GameObject por Unity en cada fotograma.
        /// actualice el control deslizante de salud para reflejar la salud del jugador
        /// </summary>
        private void Start()
        {
			playerHealthSlider.maxValue = target.HealthMax;
		}
        void Update()
		{
			// Destruirse a sí mismo si el objetivo es nulo.
			// Es un mecanismo de seguridad cuando Photon está destruyendo instancias de un jugador en la red.
			if (target == null)
			{
				Destroy(this.gameObject);
				return;
			}


			// Reflect the Player Health
			
			
				playerHealthSlider.value = target.Health;
			
		}

		/// <summary>
		/// Se llama al método MonoBehaviour después de que se hayan llamado todas las funciones de actualización. 
		/// Esto es útil para ordenar la ejecución del script.
		/// En nuestro caso, dado que estamos siguiendo un GameObject en movimiento, 
		/// debemos continuar después de que el jugador se haya movido durante un cuadro en particular.
		/// </summary>
		void LateUpdate()
		{

			// No muestres la interfaz de usuario si no somos visibles para la cámara,
			// así evitas posibles errores al ver la interfaz de usuario, pero no el reproductor en sí.
			if (targetRenderer != null)
			{
				this._canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
			}

			// #Critical
			// Siga el GameObject objetivo en la pantalla.
			if (targetTransform != null)
			{
				targetPosition = targetTransform.position;
				targetPosition.y += characterControllerHeight;

				this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
			}

		}




		#endregion

		#region Public Methods

		/// <summary>
		/// Asigna un objetivo de jugador para seguir y representar.
		/// </summary>
		/// <param name="target">Target.</param>
		public void SetTarget(PlayerManager _target)
		{

			if (_target == null)
			{
				Debug.LogError("<Color=Red><b>Missing</b></Color> Destino de PlayMakerManager para PlayerUI.SetTarget.", this);
				return;
			}

			// Caché de referencias por eficiencia porque las vamos a reutilizar.
			this.target = _target;
			targetTransform = this.target.GetComponent<Transform>();
			targetRenderer = this.target.GetComponentInChildren<Renderer>();


			CharacterController _characterController = this.target.GetComponent<CharacterController>();

			// Obtenga datos del reproductor que no cambiarán durante la vida útil de este componente
			if (_characterController != null)
			{
				characterControllerHeight = _characterController.height;
			}

			if (playerNameText != null)
			{
				playerNameText.text = this.target.photonView.Owner.NickName;
			}
		}

		#endregion

	}
}