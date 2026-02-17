using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float weight = 1f;
    private float percent = 0f;
    private PlayerMove _movement;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _movement = GetComponent<PlayerMove>();
    }

    public void TakeHit(int dmg, float baseKB, float scaling, Vector2 dir, float stunTime)
    {
        percent += dmg;

        float force = baseKB + (percent * scaling);
        Vector2 knockback = dir.normalized * force / weight;
        StartCoroutine(HitStun(stunTime));
        _rb.velocity = Vector2.zero;
        _rb.AddForce(knockback, ForceMode2D.Impulse);
    }

    IEnumerator HitStun(float stunTime)
    {
        _movement.HitStunToggle();
        yield return new WaitForSeconds(stunTime);
        _movement.HitStunToggle();
    }
}
