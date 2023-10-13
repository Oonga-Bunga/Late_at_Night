using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayBalls : AWeapon
{
    private float currentBallNumber;
    static public int maxBallNumber = 6;

    void Start()
    {
        holdableObjectType = IPlayerReceiver.HoldableObjectType.ClayBalls;
    }

    public override void Initialize(float ballNumber)
    {
        currentBallNumber = Mathf.Min(ballNumber, maxBallNumber);
    }
}
