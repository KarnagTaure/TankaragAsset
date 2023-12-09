using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System.Collections;


namespace ProyectoTank.UI

{
    public class PlayerUI : MonoBehaviour
    {
        #region Private Fields


        [Tooltip("Texto de la interfaz de usuario para mostrar el nombre del jugador")]
        [SerializeField]
        private TextMeshProUGUI playerNameText;

        [Tooltip("UI Slider to display Player's Health")]
        [SerializeField]
        private Slider playerHealthSlider;

        [Tooltip("Pixel offset from the player target")]
        [SerializeField]
        private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

        private ProyectoTank.Neetwork.PlayerManager target;

        float characterControllerHeight = 0f;
        Transform targetTransform;
        Renderer targetRenderer;
        CanvasGroup _canvasGroup;
        Vector3 targetPosition;


        #endregion


        #region MonoBehaviour Callbacks

        void Awake()
        {
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);

            _canvasGroup = this.GetComponent<CanvasGroup>();
        }

        void Update()
        {
            // Refleja la salud del jugador
            if (playerHealthSlider != null)
            {
                playerHealthSlider.value = target.Health;
            }
            // Destruirse a sí mismo si el objetivo es nulo. Es un mecanismo de seguridad cuando Photon está destruyendo instancias de un jugador en la red.
            if (target == null)
            {
                Destroy(this.gameObject);
                return;
            }
        }
        void LateUpdate()
        {
            // No muestres la interfaz de usuario si no somos visibles para la cámara, así evitas posibles errores al ver la interfaz de usuario, pero no el reproductor en sí.
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

        public void SetTarget(ProyectoTank.Neetwork.PlayerManager _target)
        {
            if (_target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            // Cache references for efficiency
            target = _target;
            if (playerNameText != null)
            {
                playerNameText.text = target.photonView.Owner.NickName;
            }
            targetTransform = this.target.GetComponent<Transform>();
            targetRenderer = this.target.GetComponent<Renderer>();
            CharacterController characterController = _target.GetComponent<CharacterController>();
            //Obtenga datos del reproductor que no cambiarán durante la vida útil de este componente
            if (characterController != null)
            {
                characterControllerHeight = characterController.height;
            }
        }

        #endregion


    }
}