using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private enum state
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


    [Header("Wall Variables")]
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private float wallJumpXForce;
    [SerializeField] private float wallJumpMult;
    [SerializeField] private bool detectWalls;
    private float wallTimer;

    [Header("Player Input")]
    [SerializeField] private state playerState;
    [SerializeField] private bool jumpRec;
    [SerializeField] private bool dashActRec;
    private bool facingLeft;
    private Vector2 playerDirections;
    private Vector2 rawPlayerDirections;
    private bool jumpCutRec;

    [Header("Collision")]
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private Vector2 wallBoxSize;
    [SerializeField] private Vector3 wallBoxOrigin;
    [SerializeField] private float downCastDistance;
    [SerializeField] private float rightCastDistance;
    [SerializeField] private float leftCastDistance;

    // References
    private Rigidbody2D _rb;
    private Collider2D _collider;

    //misc shit
    private float jumpTimer;
    private float dashTimer;
    private state prevState;
    [SerializeField] private float inputBuffer;
    [SerializeField] private float wallBuffer;
    private float startingGravity;
    private float dashDurationTimer;
    [SerializeField] private float dashDuration;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        hasDashed = false;
        jumpTimer = inputBuffer;
        dashTimer = inputBuffer;
        dashDurationTimer = dashDuration;
        startingGravity = _rb.gravityScale;
    }

    void Update()
    {
        FloorDetect();
        InputGather();
        DirectionFacing(false);
    }

    private void FixedUpdate()
    {
        Action();

        if (jumpRec)
        {
            jumpTimer -= Time.fixedDeltaTime;

            if (jumpTimer <= 0)
            {
                jumpRec = false;
                jumpTimer = inputBuffer;
            }
        }
        if (dashActRec)
        {
            dashTimer -= Time.fixedDeltaTime;

            if (dashTimer <= 0)
            {
                dashActRec = false;
                dashTimer = inputBuffer;
            }
        }
        if (!detectWalls)
        {
            wallTimer -= Time.fixedDeltaTime;

            if (wallTimer <= 0)
            {
                detectWalls = true;
                wallTimer = wallBuffer;
            }
        }
        if (playerState == state.dashing)
        {
            dashDurationTimer -= Time.fixedDeltaTime;

            if (dashDurationTimer <= 0)
            {
                UpdateState(state.midair, true);
                dashDurationTimer = dashDuration;
            }
        }
    }

    private void InputGather()
    {
        playerDirections = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rawPlayerDirections = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRec = true;
        }
        jumpCutRec = Input.GetKey(KeyCode.Space) == false && playerState == state.jumping;
        if (Input.GetKeyDown(KeyCode.LeftShift) && !hasDashed)
        {
            dashActRec = true;
        }
    }

    private void Action()
    {
        switch (playerState)
        {
            case state.grounded:
                MovementCalc();

                if(rawPlayerDirections == Vector2.zero)
                {
                    _rb.velocity = Vector2.zero;
                }
                if (jumpRec)
                {
                    UpdateState(state.jumping);
                }
                if (FloorDetect() == false) //Covers if going straight from ground to airborne
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
                if (dashActRec)
                {
                    UpdateState(state.dashing);
                    dashActRec = false;
                }
                break;
            case state.jumping:
                MovementCalc();

                if (jumpCutRec)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, jumpcut * _rb.velocity.y);
                }
                if (dashActRec)
                {
                    UpdateState(state.dashing);
                }
                if (_rb.velocity.y <= 0f)
                {
                    UpdateState(state.midair);
                }
                if (FloorDetect())
                {
                    UpdateState(state.grounded);
                }
                if (WallDirectionDetect() != 0 && WallDirectionDetect() != 3)
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
                if (FloorDetect())
                {
                    UpdateState(state.grounded);
                }
                if (dashActRec)
                {
                    UpdateState(state.dashing);
                }
                if (WallDirectionDetect() != 0 && WallDirectionDetect() != 3)
                {
                    UpdateState(state.walled);
                }
                break;
            case state.dashing:

                dashDurationTimer = dashDuration;
                if (FloorDetect() && _rb.velocity.y <= 0)
                {
                    UpdateState(state.grounded);
                }
                if (WallDirectionDetect() != 0 && WallDirectionDetect() != 3)
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
                if (WallDirectionDetect() == 0)
                {
                    UpdateState(state.midair);
                }
                if (FloorDetect())
                {
                    UpdateState(state.grounded);
                }
                if (jumpRec)
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
                    _rb.velocity = new Vector2(-WallDirectionDetect() * wallJumpXForce * Time.fixedDeltaTime, wallJumpMult * jumpForce * Time.fixedDeltaTime); //wall jump
                }
                else if (prevState == state.midair)
                {
                    detectWalls = true;
                }
                else
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, jumpForce * Time.fixedDeltaTime); //jump
                }

                jumpRec = false;
                break;
            case state.grounded:
                hasDashed = false;
                break;
            case state.dashing:
                _rb.gravityScale = 0;
                Dash();
                break;
            case state.walled:
                hasDashed = false;
                if (prevState != state.walled)
                    _rb.velocity = new Vector2(0, _rb.velocity.y);
                break;
            case state.midair:
                detectWalls = true;
                break;
        }
        if (prevState == state.walled)
        {
            detectWalls = false;
        }
        if (prevState == state.dashing)
        {
            _rb.gravityScale = startingGravity;
        }
        
    }
    private void DirectionFacing(bool flipped)
    {
        if (flipped == false && !hasDashed)
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
        else if (flipped && hasDashed)
        {
            if (rawPlayerDirections.x == -1)
            {
                facingLeft = true;
            }
            else if (rawPlayerDirections.x == 1)
            {
                facingLeft = false;
            }
        }
    }
    private void MovementCalc()
    {
        if (rawPlayerDirections != Vector2.zero)
        {
            float targetSpeed = rawPlayerDirections.x * maxSpeed; //reflects left/right input
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

        if (WallDirectionDetect() == rawPlayerDirections.x && _rb.velocity.y < 0) //wall slide
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
        _rb.velocity = dashPower * rawPlayerDirections;
    }

    private bool FloorDetect()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, downCastDistance, floorLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private int WallDirectionDetect()
    {
        RaycastHit2D leftwallcast = Physics2D.BoxCast(transform.position + wallBoxOrigin, wallBoxSize, 0, -transform.right, leftCastDistance, wallLayer);
        RaycastHit2D rightwallcast = Physics2D.BoxCast(transform.position + wallBoxOrigin, wallBoxSize, 0, transform.right, rightCastDistance, wallLayer);
        if (detectWalls)
        {
            if (leftwallcast && rightwallcast)
            {
                return 2;
            }
            else if (leftwallcast)
            {
                return -1;
            }
            else if (rightwallcast)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 3;
        }
    }

    //public state GetPlayerState()
    //{
    //    return playerState;
    //}


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * downCastDistance, boxSize);
        Gizmos.DrawWireCube(transform.position + wallBoxOrigin - transform.right * leftCastDistance, wallBoxSize);
        Gizmos.DrawWireCube(transform.position + wallBoxOrigin + transform.right * rightCastDistance, wallBoxSize);
    }
}
