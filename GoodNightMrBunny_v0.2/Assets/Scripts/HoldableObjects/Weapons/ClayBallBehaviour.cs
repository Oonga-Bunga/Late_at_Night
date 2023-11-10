using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayBallBehaviour : MonoBehaviour
{
    #region Attributes
    
    public float baseDamage = 20f;
    [SerializeField]private float lifeTime = 5f;
    [SerializeField]private float jumpForce = 30.0f;
    private bool _canJump = false;

    #endregion

    #region Initialization

    public void Initialize(Vector3 direction, float force)
    {
        GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Impulse);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Método que controla el comportamiento de la plastilina al chochar. (Hacer daño/Trampolin/Aplastarse)
    /// </summary>
    /// <param name="collision">Objeto con el que colisiona</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
<<<<<<< Updated upstream
            collision.gameObject.GetComponent<AKillableEntity>().TakeHit(baseDamage,IKillableEntity.AttackSource.ClayBall);
=======
            //Hace enemigo al monstruo
            collision.gameObject.GetComponent<AMonster>().TakeHit(baseDamage,IKillableEntity.AttackSource.ClayBall);
>>>>>>> Stashed changes
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player") && _canJump)
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
            transform.position = new Vector3(transform.position.x, transform.position.y-transform.localScale.x/3, transform.position.z);
            _canJump = true;
            Destroy(gameObject, lifeTime);
        }
    }
    
    #endregion
}
