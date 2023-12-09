using UnityEngine;
using UnityEngine.UI;

namespace ProjectoTank

{
    public class LobbyPanelArriba : MonoBehaviour
    {
        private readonly string connectionStatusMessage = "    Connection Status: ";

        [Header("UI References")]
        public Text ConnectionStatusText;

        #region UNITY

        public void Update()
        {
            ConnectionStatusText.text = connectionStatusMessage + Photon.Pun.PhotonNetwork.NetworkClientState;
        }

        #endregion
    }
}