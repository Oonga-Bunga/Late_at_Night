using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayBallBehaviour : MonoBehaviour
{
    #region Attributes
    
    private float lifeTime = 4f;
    public float baseDamage = 10f;
    
    #endregion

    #region Methods
    /// <summary>
    /// Destruye las bolas de plastilina tras su tiempo de vida
    /// </summary>
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    /// <summary>
    /// Cuando impacta con un enemigo llama a la funcion takeHit del enemigo
    /// </summary>
    /// <param name="collision">Objeto con el que colisiona</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<AMonster>().TakeHit(baseDamage);
        }
    }
    
    #endregion
}
