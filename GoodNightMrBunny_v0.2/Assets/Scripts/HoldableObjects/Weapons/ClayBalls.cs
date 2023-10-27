using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayBalls : AHoldableObject
{
    #region Attributes
    
    private int currentBallNumber;
    static public int maxBallNumber = 6;
    [SerializeField] private float baseDamage = 5f;
    [SerializeField] private UpdateUIClayAmmo uiClayAmmo;
    [SerializeField] private GameObject clayBallPrefab;
    [SerializeField] private float shotForce = 20f;
    
    #endregion

    #region Methods
    void Start()
    {
        _holdableObjectType = IPlayerReceiver.HoldableObjectType.ClayBalls;
        clayBallPrefab.GetComponent<ClayBallBehaviour>().baseDamage = baseDamage;
    }

    public override void Initialize(float ballNumber)
    {
        uiClayAmmo.gameObject.SetActive(true);
        currentBallNumber = Mathf.Min((int)ballNumber, maxBallNumber);
        uiClayAmmo.setMaxBallNumber(maxBallNumber);
        uiClayAmmo.UpdateClayText(currentBallNumber);
    }

    /// <summary>
    /// Llama a la acción de disparar con el input del jugador
    /// </summary>
    /// <param name="attackInput">Input del jugador</param>
    public override void Use(IPlayerReceiver.InputType attackInput)
    {
        if (currentBallNumber < 0) return;

        if(attackInput == IPlayerReceiver.InputType.Up)
        {
            Shot();
        }
    }

    /// <summary>
    /// Método que dispara bolas de plastilina
    /// </summary>
    public void Shot()
    {
        if (currentBallNumber <= 0) return;

        GameObject clayBall = Instantiate(clayBallPrefab, this.transform.position, Quaternion.identity);
        clayBall.GetComponent<ClayBallBehaviour>().Initialize(transform.forward, shotForce);

        currentBallNumber -= 1;
        if (currentBallNumber == 0)
        {
            uiClayAmmo.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            _player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.None,false);
            return;
        }
        uiClayAmmo.UpdateClayText(currentBallNumber);
    }
    
    #endregion
}
