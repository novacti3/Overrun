using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBearer : Enemy
{
    GameObject shield;
    
    float shieldHealth;

    protected override void Start() {
        base.Start();
        shield = transform.GetChild(0).gameObject;    
    }
    protected override void FixedUpdate() {
        base.FixedUpdate();
    }

    private void Update() {
    
        if(shield != null)
            shield.transform.right = Vector2.Lerp(shield.transform.right, player.transform.position - transform.position, 0.5f * Time.deltaTime);    
    }

    public void DamageShield() {
        shieldHealth --;
        
        if(shieldHealth <= 0) {
            Destroy(transform.GetChild(0).gameObject);
        }
    }

}
