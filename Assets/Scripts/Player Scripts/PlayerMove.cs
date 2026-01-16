using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public enum state
    {
        grounded, jumping, midair, dashing, walled
    }

    [Header("Movement Variables")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float jumpcut;
    [SerializeField] private bool hasDashed;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashDuration;

    [Header("Wall Variables")]
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private float wallJumpXForce;
    [SerializeField] private float wallJumpMult;
    [SerializeField] private bool detectWalls;
    private float wallTimer;


    // References
    private Rigidbody2D _rb;
    PlayerEnvironment _collision;
    PlayerInput _inputs;

    //misc shit
    private state playerState;
    private state prevState;
    private float storedSpeed;
    public state currentState => playerState;
    private bool facingLeft;

    private float dashTimer;
    private float startingGravity;
    private float dashDurationTimer;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _inputs = GetComponent<PlayerInput>();
        _collision = GetComponent<PlayerEnvironment>();
        startingGravity = _rb.gravityScale;
    }

    void Update()
    {
        DirectionFacing();
    }

    private void FixedUpdate()
    {
        Action();
    }

    private void Action()
    {
        switch (playerState)
        {
            case state.grounded:
                MovementCalc();

                if (_inputs.saysJump)
                {
                    UpdateState(state.jumping);
                }
                if (_collision.FloorDetect() == false) //Covers if going straight from ground to airborne
                {
                    if (_rb.velocity.y > 0)
                    {
                        UpdateState(state.jumping, false);
                    }
                    else
                    {
                        UpdateState(state.midair);
                    }
                }
                if (_inputs.saysDash)
                {
                    UpdateState(state.dashing);
                    _inputs.Consume(PlayerInput.Action.dash);
                }
                break;
            case state.jumping:
                MovementCalc();

                if (_inputs.jumpCutRec)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, jumpcut * _rb.velocity.y);
                }
                if (_inputs.saysDash)
                {
                    UpdateState(state.dashing);
                }
                if (_rb.velocity.y <= 0f)
                {
                    UpdateState(state.midair);
                }
                if (_collision.FloorDetect())
                {
                    UpdateState(state.grounded);
                }
                if (_collision.WallDirectionDetect() != 0 && _collision.WallDirectionDetect() != 3)
                {
                    UpdateState(state.walled);
                }
                break;
            case state.midair:
                MovementCalc();

                if (_rb.velocity.y <= -maxFallSpeed)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, -maxFallSpeed);
                }
                if (_collision.FloorDetect())
                {
                    UpdateState(state.grounded);
                }
                if (_inputs.saysDash)
                {
                    UpdateState(state.dashing);
                }
                if (_collision.WallDirectionDetect() != 0 && _collision.WallDirectionDetect() != 3)
                {
                    UpdateState(state.walled);
                }
                break;
            case state.dashing:
                MovementCalc();

                //FUCK

                if (_collision.FloorDetect() && _rb.velocity.y <= 0)
                {
                    UpdateState(state.grounded);
                }
                if (_collision.WallDirectionDetect() != 0 && _collision.WallDirectionDetect() != 3)
                {
                    UpdateState(state.walled);
                }
                break;
            case state.walled:
                WallMovement();

                if (_rb.velocity.y <= -maxFallSpeed)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, -maxFallSpeed);
                }
                if (_collision.WallDirectionDetect() == 0)
                {
                    UpdateState(state.midair);
                }
                if (_collision.FloorDetect())
                {
                    UpdateState(state.grounded);
                }
                if (_inputs.saysJump)
                {
                    UpdateState(state.jumping);
                }
                break;
        }
    }
    
    private void UpdateState(state newstate, bool doAction = true)
    {
        Debug.Log(newstate.ToString() + " state");
        prevState = playerState;
        playerState = newstate;
        if (doAction)
        {
            StateAction(newstate);
        }
    }
    private void StateAction(state newstate)
    {
        switch (newstate)
        {
            case state.jumping:
                if (prevState == state.walled)
                {
                    _rb.velocity = new Vector2(-_collision.WallDirectionDetect() * wallJumpXForce * Time.fixedDeltaTime, wallJumpMult * jumpForce * Time.fixedDeltaTime); //wall jump
                }
                else if (prevState == state.midair)
                {
                    _collision.DetectWalls = true;
                }
                else
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, jumpForce * Time.fixedDeltaTime); //jump
                }
                _inputs.Consume(PlayerInput.Action.jump);
                break;
            case state.grounded:
                hasDashed = false;
                break;
            case state.dashing:
                Dash();
                break;
            case state.walled:
                if (prevState != state.walled)
                    _rb.velocity = new Vector2(0, _rb.velocity.y);
                break;
            case state.midair:
                _collision.DetectWalls = true;
                break;
        }
        if (prevState == state.walled) _collision.DetectWalls = false;
        if (prevState == state.dashing) _rb.gravityScale = startingGravity;
    }
    private void DirectionFacing()
    {
        if (_rb.velocity.x < 0)
        {
            facingLeft = true;
        }
        else if (_rb.velocity.x > 0)
        {
            facingLeft = false;
        }
    }
    private void MovementCalc()
    {
        if (_inputs.RawDirections != Vector2.zero)
        {
            float targetSpeed = _inputs.RawDirections.x * maxSpeed; //reflects left/right input
            if (playerState == state.grounded)
            {
                _rb.velocity = new Vector2(targetSpeed, _rb.velocity.y);
            }
            else
            {
                _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x + targetSpeed * 0.2f, -Mathf.Abs(targetSpeed), Math.Abs(targetSpeed)), _rb.velocity.y);
            }
        }
    }
    private void WallMovement()
    {
        if (_collision.WallDirectionDetect() == _inputs.RawDirections.x && _rb.velocity.y < 0) //wall slide
        {
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else //move away from wall
        {
            MovementCalc();
        }
    }
    private void Dash()
    {
        hasDashed = true;
        _rb.velocity = dashPower * _inputs.RawDirections;
    }
}
