using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private int damage = 8;
    [SerializeField] private float baseKnockback = 6f;
    [SerializeField] private float knockbackScaling = 0.4f;
    [SerializeField] private Vector2 launchDirection = new Vector2(1, 1); // normalized later
    [SerializeField] private float hitstun = 0.3f;

    public GameObject owner;

    void Start()
    {
        Debug.Log("this ran");
        owner = transform.root.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Hit: " + col.name + " root: " + col.transform.root.name);


        if (col.transform.root.gameObject == owner)
        {
            return;
        }

        if (col.TryGetComponent(out Knockback target))
        {
            target.TakeHit(damage, baseKnockback, knockbackScaling, launchDirection, hitstun);
        }
    }
}
