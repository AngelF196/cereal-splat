using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    PlayerInput _inputs;
    PlayerMove _movement;

    void Start()
    {
        _inputs = GetComponent<PlayerInput>();
        _movement = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        if (_inputs.saysAttack) 
        { 
            if(_movement.currentState == PlayerMove.state.grounded) { GroundAttacks(); }
            else if (_movement.currentState == PlayerMove.state.midair || _movement.currentState == PlayerMove.state.jumping) { AirAttacks(); }
            else if (_movement.currentState == PlayerMove.state.dashing) { DashAttack(); }
        }
    }

    private void DashAttack()
    {

    }

    private void AirAttacks()
    {
        switch (_inputs.attackDir)
        {
            case (PlayerInput.AttackDirections.Up):
                Debug.Log("Air, Up");
                break;
            case (PlayerInput.AttackDirections.Down):
                Debug.Log("Air, Down");
                break;
            case (PlayerInput.AttackDirections.Left):
                Debug.Log("Air, Left");
                break;
            case (PlayerInput.AttackDirections.Right):
                Debug.Log("Air, Right");
                break;
            case (PlayerInput.AttackDirections.Neutral):
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
                Debug.Log("Ground, Up");
                break;
            case (PlayerInput.AttackDirections.Down):
                Debug.Log("Ground, Down");
                break;
            case (PlayerInput.AttackDirections.Left):
                Debug.Log("Ground, Left");
                break;
            case (PlayerInput.AttackDirections.Right):
                Debug.Log("Ground, Right");
                break;
            case (PlayerInput.AttackDirections.Neutral):
                Debug.Log("Ground, Neutral");
                break;
        }    
        _inputs.Consume(PlayerInput.Action.attack);
    }
}
