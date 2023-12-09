using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProyectoTank.Municion
{
    public class PoolBalasEnemigo : MonoBehaviour
    {
        public static PoolBalasEnemigo CompartirInstance;
        public List<GameObject> objetosPooled;
        public GameObject objetosToPool;
        public int cantidadToPool;


        private void Awake()
        {
            CompartirInstance = this;

        }
        private void Start()
        {
            objetosPooled = new List<GameObject>();
            for (int i = 0; i<cantidadToPool; i++)
            {
                GameObject obj = Instantiate(objetosToPool);
                obj.SetActive(false);
                objetosPooled.Add(obj);
                obj.transform.SetParent(this.transform);
            }
        }
        public GameObject CojerObjetoPooled()
        {
            for (int i =0; i<objetosPooled.Count; i++)
            {
                if (!objetosPooled[i].activeInHierarchy)
                {
                    return objetosPooled[i];
                }
            }
            return null;
        }
    }
}