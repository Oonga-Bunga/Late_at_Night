using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayBallBehaviour : MonoBehaviour
{
    #region Attributes
    
    public float baseDamage = 10f;
    [SerializeField]private float lifeTime = 5f;
    [SerializeField]private float jumpForce = 30.0f;
    private bool canJump = false;
    
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
    /// Método que controla el comportamiento de la plastilina al chochar. (Hacer daño/Trampolin/Aplastarse)
    /// </summary>
    /// <param name="collision">Objeto con el que colisiona</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Hace enemigo al monstruo
            collision.gameObject.GetComponent<AMonster>().TakeHit(baseDamage);
        }
        else if (collision.gameObject.CompareTag("Player") && canJump)
        {
            //Si choca contra el jugador actua como un trampolin
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                Destroy(gameObject); // Destruye el trampolín después de usarlo.
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //Si choca contra el suelo, aplastar el modelo 3D y juntarlo al suelo
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.rotation = Quaternion.Euler(Vector3.zero);;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y/2, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, transform.position.y-transform.localScale.x/2, transform.position.z);
            canJump = true;
        }
    }
    
    #endregion
}
