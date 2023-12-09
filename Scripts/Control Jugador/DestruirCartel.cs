using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestruirCartel : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var cartel = gameObject;
        Destroy(cartel, 3f);
        
    }

}
