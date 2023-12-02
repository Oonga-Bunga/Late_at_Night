using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _stepAudioSource;
    [SerializeField] private AudioSource _jumpAudioSource;
    [SerializeField] private AudioSource _landedAudioSource;
    [SerializeField] private AudioSource _hurtAudioSource;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private PlayerHealth _ph;
    [SerializeField] private PlayerMovement _pm;
    [SerializeField] private HeadBob _hb;

    // Start is called before the first frame update
    void Start()
    {
        _ph.OnDamaged += PlayHurtSound;
        _pm.OnJumped += PlayJumpSound;
        _pm.OnLanded += PlayLandedSound;
        _hb.OnStep += PlayStepSound;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayStepSound()
    {
        _stepAudioSource.Play();
    }

    private void PlayJumpSound()
    {
        //_jumpAudioSource.Play();
    }

    private void PlayLandedSound()
    {
        _landedAudioSource.Play();
    }

    private void PlayHurtSound()
    {
        _hurtAudioSource.Play();
    }
}
