using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBearer : Enemy
{
    GameObject shield;

    float shieldHealth;

    [SerializeField]
    AudioSource shieldBreak;

    protected override void Start()
    {
        base.Start();
        shield = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (gm.player != null && shield != null)
            shield.transform.right = Vector2.Lerp(shield.transform.right, gm.player.position - transform.position, 0.5f * Time.deltaTime);
    }

    public void DamageShield()
    {
        shieldHealth--;

        if (shieldHealth <= 0)
        {
            Destroy(transform.GetChild(0).gameObject);
            shieldBreak.Play();
        }
    }

}
