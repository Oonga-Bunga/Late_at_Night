using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayBallBehaviour : MonoBehaviour
{
    private float lifeTime = 5f;
    private float baseDamage = 10f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            collision.gameObject.GetComponent<AMonster>().TakeHit(baseDamage);
        }
        Destroy(gameObject);
    }
}
