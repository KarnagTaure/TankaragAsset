using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


namespace ProyectoTank.MenuInicio
{
    public class MenuPrincipal : MonoBehaviour
    {
        [Header("Opciones")]
        public Slider volumenMaster;
        public Slider volumenFx;
        public Toggle mute;
        public AudioMixer mixer;
        public AudioSource fxSource;
        public AudioClip clickSound;
        private float ultimoVolumenMaster;
        private float ultimoVolumen;
        private float ultimoVolumenFX;
        private float valorSlider;

        [Header("Paneles")]
        public GameObject panelPrincipal;
        public GameObject panelOpciones;
        public GameObject panelJuego;
        public GameObject controles;

        private void Awake()
        {
            

            volumenFx.onValueChanged.AddListener(CambiarVolumenFx);
            volumenMaster.onValueChanged.AddListener(CambiarVolumenMaster);


            mixer.GetFloat("VolMaster", out ultimoVolumenMaster);
            mixer.GetFloat("VolSonido", out ultimoVolumen);
            mixer.GetFloat("VolFx", out ultimoVolumenFX);

            

        }
        
        private void Start()
        { 
            volumenMaster.value = ultimoVolumen;
            volumenFx.value = ultimoVolumenFX;
            if (ultimoVolumenMaster == - 80)
            {
                mute.isOn = true;
            }
            else
            {
                mute.isOn = false;
                mixer.SetFloat("VolMaster", 0f);
            }
            
        }
        public void SetMute()
        {
           
            
            if (mute.isOn)
            {
                mixer.GetFloat("VolMaster", out ultimoVolumenMaster);
                mixer.GetFloat("VolSonido", out ultimoVolumen);
                mixer.GetFloat("VolFx", out ultimoVolumenFX);

                mixer.SetFloat("VolMaster", -80);
            }
            else if (!mute.isOn)
            {
                mixer.SetFloat("VolMaster", 0);
            }
        }
        public void AbrirPanel(GameObject panel)
        {
            panelPrincipal.SetActive(false);
            panelOpciones.SetActive(false);
            panelJuego.SetActive(false);
            controles.SetActive(false);

            panel.SetActive(true);
            PlaySonidoBoton();
        }

        public void CambiarVolumenMaster(float v)
        {
            
            mixer.GetFloat("VolSonido", out ultimoVolumen);
            mixer.SetFloat("VolSonido", v);
        }
        public void CambiarVolumenFx(float v)
        {
            mixer.GetFloat("VolFx", out ultimoVolumenFX);
            mixer.SetFloat("VolFx", v);
        }

        public void PlaySonidoBoton()
        {
            fxSource.PlayOneShot(clickSound);
        }
       
    }
}