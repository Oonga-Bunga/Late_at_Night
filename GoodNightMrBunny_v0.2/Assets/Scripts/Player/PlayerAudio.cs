using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _stepAudioSource;
    [SerializeField] private AudioSource _jumpAudioSource;
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
        _hb.OnStep += PlayStepSound;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayStepSound()
    {
        if (_stepAudioSource.isPlaying)
        {
            return;
        }

        _stepAudioSource.Play();
    }

    private void PlayJumpSound()
    {
        if (_jumpAudioSource.isPlaying)
        {
            return;
        }

        //_jumpAudioSource.Play();
    }

    private void PlayHurtSound()
    {
        if (_hurtAudioSource.isPlaying)
        {
            return;
        }

        _hurtAudioSource.Play();
    }
}
