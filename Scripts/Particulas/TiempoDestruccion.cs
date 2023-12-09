using UnityEngine;
using System.Collections;


public class TiempoDestruccion : MonoBehaviour
{
	public float tiempoEspera= 0.2f;


	private void Start()
    {


		Destroy(gameObject, tiempoEspera);
	}
	
	
}
