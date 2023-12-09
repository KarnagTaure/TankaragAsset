using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMirilla : MonoBehaviour
{
    [SerializeField]
    private float velocidad = 10f;
    [SerializeField]
    private float rangoX =1f;
    [SerializeField]
    private float rangoY = 1f;
    

    private Rigidbody2D mirillaRb;
    private Vector2 moveImput;
    
    public VariableJoystick variableJoystick;

    // Start is called before the first frame update
    void Start()
    {
        mirillaRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       RangoMovimiento();
        float movex = variableJoystick.Horizontal;
        float movey = variableJoystick.Vertical;
       
        /*float movex = Input.GetAxisRaw("Horizontal");
        float movey = Input.GetAxisRaw("Vertical");
        */
        moveImput = new Vector2(movex, movey).normalized;
    }
    private void FixedUpdate()
    { 
        mirillaRb.MovePosition(mirillaRb.position + moveImput * velocidad * Time.deltaTime);
    }
    private void RangoMovimiento()
    {
        
        if (transform.localPosition.x < -rangoX)
        {
            transform.localPosition = new Vector2(-rangoX, transform.localPosition.y);
        }
        if (transform.localPosition.x > rangoX)
        {
            transform.localPosition = new Vector2(rangoX, transform.localPosition.y);
        }
        if (transform.localPosition.y < -rangoY)
        {
            transform.localPosition = new Vector2( transform.localPosition.x, -rangoY);
        }
        if (transform.localPosition.y > rangoY)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, rangoY);
        }
    }
}
