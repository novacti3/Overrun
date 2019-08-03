using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField]
    private GameObject playerGO;

    [SerializeField]
    private Camera cam;

    private Player player;

    bool canShoot = true;

    [SerializeField]
    GameObject arrow;

    [SerializeField]
    LayerMask arrowLayer;

    float originalTime;

    void Start() {
        //Initialization
        player = playerGO.GetComponent<Player>();
        originalTime = Time.fixedDeltaTime;
        
    }
    void Update()
    {
        //Looks at mouse
        LookAtMouse();

        //Shoot instantly if player is on ground : due to change
        if(player.IsGrounded()) {
            if(Input.GetMouseButtonDown(0)) {
                if(canShoot) 
                    Shoot();
                
            }
        } else {
            
            //If in the air slow down the time and wait till the button is released
            if(Input.GetMouseButton(0) && canShoot) {
                Time.timeScale = 0.05f;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                
            }
            if(Input.GetMouseButtonUp(0) || player.IsGrounded()) {
                Time.timeScale = 1;
                Time.fixedDeltaTime = originalTime;
                if(canShoot)
                    Shoot();
            }
        }
    }
    
    //TODO: Add more velocity when arrow is going left or right
    //Add velocity to the arrow when shot
    void Shoot() {
        GameObject arrowObject = Instantiate(arrow, transform.position, transform.rotation);
        arrowObject.GetComponent<Arrow>().bow = this;
        arrowObject.GetComponent<Rigidbody2D>().velocity = (cam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * 25;
        canShoot = false;

    }

    public void PickUpArrow() {
        canShoot = true;
    }

    //Makes bow point to mouse
    void LookAtMouse() {
        Vector3 target = cam.ScreenToWorldPoint(Input.mousePosition);
        target.z = 0;

        transform.parent.right = target - transform.parent.position;
    }
}
