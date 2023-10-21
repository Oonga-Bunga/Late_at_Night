using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
    public GameObject punto;
    public GameObject Spawn;
    private float tiempo = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tiempo += Time.deltaTime;
        if(tiempo >= 5){
            tiempo = 0;
            Instantiate(Spawn,punto.transform.position,Quaternion.identity);
        }
    }
}
