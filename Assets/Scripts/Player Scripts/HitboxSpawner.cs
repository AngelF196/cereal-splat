using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxSpawner : MonoBehaviour
{
    public GameObject uhitbox;
    public GameObject dhitbox;
    public GameObject lhitbox;
    public GameObject rhitbox;
    public GameObject Downtilt;
    public GameObject DowntiltL;
    public GameObject SideTitleL;
    public GameObject SideTitleR;
    public GameObject Jab;
    public GameObject JabL;
    public GameObject Uptilt;
    public GameObject UptiltL;

    public GameObject hitcircle;
    public Transform hitboxSpawnPoint;
    public PlayerMove movement;

    public void EnableDowntilt()
    {
        if (movement.isFacingLeft)
        {
            DowntiltL.SetActive(true);
        }
        else
        {
            Downtilt.SetActive(true);
        }
    }
    public void EnableJab()
    {
        if (movement.isFacingLeft)
        {
            JabL.SetActive(true);
        }
        else
        {
            Jab.SetActive(true);
        }
    }
    public void EnableUptilt()
    {
        if (movement.isFacingLeft)
        {
            UptiltL.SetActive(true);
        }
        else
        {
            Uptilt.SetActive(true);
        }
    }
    public void EnableSideTilt()
    {
        if (movement.isFacingLeft)
        {
            SideTitleR.SetActive(true);
        }
        else
        {
            SideTitleL.SetActive(true);
        }
    }
    public void EnableUHitbox()
    {
        uhitbox.SetActive(true);
    }
    public void EnableDHitbox()
    {
        dhitbox.SetActive(true);
    }
    public void EnableLHitbox()
    {
        lhitbox.SetActive(true);
    }
    public void EnableRHitbox()
    {
        if (movement.isFacingLeft)
        {
            lhitbox.SetActive(true);
        }
        else
        {
            rhitbox.SetActive(true);
        }
    }
    public void DisableHitboxes()
    {
        uhitbox.SetActive(false);
        dhitbox.SetActive(false);
        lhitbox.SetActive(false);
        rhitbox.SetActive(false);
        Downtilt.SetActive(false);
        DowntiltL.SetActive(false);
        SideTitleL.SetActive(false);
        SideTitleR.SetActive(false);
        Jab.SetActive(false);
        JabL.SetActive(false);
        Uptilt.SetActive(false);
        UptiltL.SetActive(false);
    }
    public void EnableHitcircle()
    {
        hitcircle.SetActive(true);
    }
    public void DisableHitcircle()
    {
        hitcircle.SetActive(false);
    }
}
