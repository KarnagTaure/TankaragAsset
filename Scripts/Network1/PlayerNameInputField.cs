using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;


namespace ProyectoTank.Neetwork
{
    /// <summary>
    /// Campo de entrada del nombre del jugador. Permita que el usuario ingrese su nombre, aparecerá arriba del jugador en el juego.
    /// </summary>
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants


        // Guarde la clave PlayerPref para evitar errores tipográficos
        const string playerNamePrefKey = "PlayerName";


        #endregion


        #region MonoBehaviour CallBacks


        /// <summary>
        /// Método MonoBehaviour invocado en GameObject por Unity durante la fase de inicialización.
        /// </summary>
        private void Awake()
        {
            
       


            string defaultName = string.Empty;
            InputField _inputField = this.GetComponent<InputField>();
            if (_inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }


            PhotonNetwork.NickName = defaultName;
        }


        #endregion


        #region Public Methods


        /// <summary>
        /// Establece el nombre del jugador y lo guarda en PlayerPrefs para futuras sesiones.
        /// </summary>
        /// <param name="value">The name of the Player</param>
        public void SetPlayerName(string value)
        {
            // #Important
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("El nombre del jugador es nulo o está vacío");
                return;
            }
            PhotonNetwork.NickName = value;


            PlayerPrefs.SetString(playerNamePrefKey, value);
        }


        #endregion
    }
}