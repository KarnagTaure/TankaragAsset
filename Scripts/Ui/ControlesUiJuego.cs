using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using System.Collections;


namespace ProyectoTank.UI
{
    public class ControlesUiJuego : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        private ProyectoTank.Neetwork.PlayerManager target;
        CanvasGroup _canvasGroup;
        Transform targetTransform;
        Renderer targetRenderer;

        void Awake()
        {
           
           
                this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);

                _canvasGroup = this.GetComponent<CanvasGroup>();
               
        }


        #endregion
        // Start is called before the first frame update
        

        // Update is called once per frame
        void Update()
        {
            
        }
        public void UITarget(ProyectoTank.Neetwork.PlayerManager _target)
        {
            if (_target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            // Cache references for efficiency
            target = _target;
           
            targetTransform = this.target.GetComponent<Transform>();
            targetRenderer = this.target.GetComponent<Renderer>();
            
           
        }
    }
}