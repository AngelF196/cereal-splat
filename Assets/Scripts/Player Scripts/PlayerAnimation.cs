using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerMove _movement;
    private Animator _animator;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private bool grounded;
    private bool walled;


    void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _movement = GetComponentInParent<PlayerMove>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_movement.isFacingLeft)
        {
            _sr.flipX = true;
        }
        else
        {
            _sr.flipX = false;
        }



    }
        public void FlipAnimation()
        {
            _animator.SetTrigger("Flip");
        }

        public void UpdateAnimationState(PlayerMove.state state, PlayerMove.state prevState)
        {
            if (state != PlayerMove.state.grounded)
            {
                grounded = false;
            }

            switch (state)
            {
                case (PlayerMove.state.grounded):
                    walled = false;
                    grounded = true;
                    _animator.SetBool("Grounded", false);
                    break;
                case (PlayerMove.state.jumping):
                    walled = false;
                    grounded = false;
                    _animator.SetBool("Grounded", false);
                    break;
                case (PlayerMove.state.midair):
                    walled = false;
                    grounded = false;
                    _animator.SetBool("Grounded", false);

                break;
                case (PlayerMove.state.walled):
                    grounded = false;
                    _animator.SetBool("Grounded", false);
                    break;
            }
        }

    }