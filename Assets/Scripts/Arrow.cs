using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    LayerMask player;

    [HideInInspector]
    public Bow bow;

    bool canDoDamage = true;
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Just standing up for myself here for some unearthly reason layer mask variable wasnt working here
        //I tried for ages

        //If the arrow is touching the player, pick it up
        if(other.gameObject.layer == 10) {
            
            bow.PickUpArrow();
            Destroy(gameObject);

        }

        //If it hits an enemy kill it
        if(other.gameObject.GetComponent<Enemy>() && canDoDamage) {
            other.gameObject.GetComponent<Enemy>().Die();
            GetComponent<Rigidbody2D>().AddForce(transform.right * 3);
        } 

        //If its a wall stick in it and dont do damage anymore
        if(other.gameObject.layer == 12){
            Destroy(GetComponent<Rigidbody2D>());
            canDoDamage = false;
        } 
        //If in wall dont do damage
        if(other.gameObject.layer == 8) {
            canDoDamage = false;
        } 
    }
}
