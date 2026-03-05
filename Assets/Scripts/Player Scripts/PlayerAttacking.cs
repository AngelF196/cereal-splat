using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    PlayerInput _inputs;
    PlayerMove _movement;
    Animator _animator;

    void Start()
    {
        _inputs = GetComponent<PlayerInput>();
        _movement = GetComponent<PlayerMove>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (_movement.currentState == PlayerMove.state.grounded)
        {
            _animator.SetBool("Grounded", true);
        }
        else
        {
            _animator.SetBool("Grounded", false);
        }
        if (_inputs.saysAttack)
        {
            if (_movement.currentState == PlayerMove.state.grounded && _inputs.attackDir != PlayerInput.AttackDirections.None)
            { GroundAttacks(); }
            else if (_movement.currentState == PlayerMove.state.midair || _movement.currentState == PlayerMove.state.jumping)
            {
                if (_inputs.attackDir != PlayerInput.AttackDirections.None)
                {
                    AirAttacks();
                }
            }
        }
    }

    private void AirAttacks()
    {
        switch (_inputs.attackDir)
        {
            case (PlayerInput.AttackDirections.Up):
                _animator.SetTrigger("Uair");
                Debug.Log("Air, Up");
                break;
            case (PlayerInput.AttackDirections.Down):
                _animator.SetTrigger("Dair");
                Debug.Log("Air, Down");
                break;
            case (PlayerInput.AttackDirections.Left):
                _animator.SetTrigger("Fair");
                Debug.Log("Air, Left");
                break;
            case (PlayerInput.AttackDirections.Right):
                _animator.SetTrigger("Fair");
                Debug.Log("Air, Right");
                break;
            case (PlayerInput.AttackDirections.Neutral):
                _animator.SetTrigger("Nair");
                Debug.Log("Air, Neutral");
                break;
        }
        _inputs.Consume(PlayerInput.Action.attack);

    }

    private void GroundAttacks()
    {
        switch (_inputs.attackDir)
        {
            case (PlayerInput.AttackDirections.Up):
                _animator.SetTrigger("Utilt");
                Debug.Log("Ground, Up");
                break;
            case (PlayerInput.AttackDirections.Down):
                _animator.SetTrigger("Dtilt");
                Debug.Log("Ground, Down");
                break;
            case (PlayerInput.AttackDirections.Left):
                _animator.SetTrigger("Ftilt");
                Debug.Log("Ground, Left");
                break;
            case (PlayerInput.AttackDirections.Right):
                _animator.SetTrigger("Ftilt");
                Debug.Log("Ground, Right");
                break;
            case (PlayerInput.AttackDirections.Neutral):
                _animator.SetTrigger("Jab");
                Debug.Log("Ground, Neutral");
                break;
        }    
        _inputs.Consume(PlayerInput.Action.attack);
    }
}
