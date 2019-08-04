using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Player player;

    bool canShoot = true;

    [SerializeField]
    GameObject arrow;

    [SerializeField]
    LayerMask arrowLayer;

    float originalTime;

    LineRenderer trajectory;

    GameMaster master;

    float power = 5f;

    void Start()
    {
        master = GameMaster.Instance;
        //Initialization
        cam = Camera.main.GetComponent<Camera>();
        player = GameMaster.Instance.player.GetComponent<Player>();
        originalTime = Time.fixedDeltaTime;
    }

    void Update()
    {
        //Looks at mouse
        LookAtMouse();

        //Shoot instantly if player is on ground : due to change
        if(player.IsGrounded())
        {
            if(Input.GetMouseButton(0)) {
                power += 0.5f;

            }
            if(Input.GetMouseButtonUp(0))
            {
                if(canShoot) 
                

                    Time.timeScale = 1;
                    Time.fixedDeltaTime = originalTime;
                    Shoot();
                    power = 25f;
            }
        }
        else
        {
            //If in the air slow down the time and wait till the button is released
            if(Input.GetMouseButton(0) && canShoot)
            {
                power += 0.5f;
                Time.timeScale = 0.05f;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                
            }
            if(Input.GetMouseButtonUp(0) || player.IsGrounded()) {
                Time.timeScale = 1;
                Time.fixedDeltaTime = originalTime;
                if(canShoot)
                    Shoot();
                
                power = 25f;
            }
        }

        Debug.Log(power);
    }
    
    //TODO: Add more velocity when arrow is going left or right
    //Add velocity to the arrow when shot
    void Shoot()
    {
        if(canShoot) {
            canShoot = false;
            GameObject arrowObject = Instantiate(arrow, transform.position, transform.rotation);
            arrowObject.GetComponent<Arrow>().bow = this;
            
            if(power >= 100 && master.kills >= 5 && !player.isRolling) {
                Debug.Log(master.kills);
                arrowObject.GetComponent<Arrow>().boomArrow = true;
            }
            
            if(player.isRolling && !player.IsGrounded()) {
                power = 50;
                arrowObject.GetComponent<Arrow>().rollArrow = true;
            }
            power = Mathf.Clamp(power, 5, 50);
            arrowObject.GetComponent<Rigidbody2D>().velocity = (cam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * power;
        }
        
        
    }

    public void PickUpArrow()
    {
        canShoot = true;
    }

    //Makes bow point to mouse
    void LookAtMouse()
    {
        Vector3 target = cam.ScreenToWorldPoint(Input.mousePosition);
        target.z = 0;

        transform.parent.right = target - transform.parent.position;
    }
}
