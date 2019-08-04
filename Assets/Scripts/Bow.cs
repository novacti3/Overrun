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
        originalTime = Time.fixedDeltaTime;
    }

    void Update()
    {
        if(power >= 85 && !player.isRolling && GameMaster.Instance.kills >= 5) {
            GameMaster.Instance.CamShake(0.1f, Mathf.Clamp(power / 1000, 0, 0.4f));
        }
        //Looks at mouse
        LookAtMouse();

        if(master.player != null)
        {
            if (player == null)
                player = master.player.GetComponent<Player>();

            //Shoot instantly if player is on ground : due to change
            if (player.IsGrounded())
            {
                if (Input.GetMouseButton(0))
                {
                    power += 0.5f;

                }
                if (Input.GetMouseButtonUp(0))
                {
                    if (canShoot)


                        Time.timeScale = 1;
                    Time.fixedDeltaTime = originalTime;
                    Shoot();
                    power = 25f;
                }
            }
            else
            {
                //If in the air slow down the time and wait till the button is released
                if (Input.GetMouseButton(0) && canShoot)
                {
                    power += 0.5f;
                    Time.timeScale = 0.05f;
                    Time.fixedDeltaTime = Time.timeScale * 0.02f;

                }
                if (Input.GetMouseButtonUp(0) || player.IsGrounded())
                {
                    Time.timeScale = 1;
                    Time.fixedDeltaTime = originalTime;
                    if (canShoot)
                        Shoot();

                    power = 25f;
                }
            }
        }
    }

    //TODO: Add more velocity when arrow is going left or right
    //Add velocity to the arrow when shot
    void Shoot()
    {
        if(canShoot) {
            canShoot = false;
            GameObject arrowObject = Instantiate(arrow, transform.position, Quaternion.Euler(0,0,180 + transform.eulerAngles.z));
            arrowObject.GetComponent<Arrow>().bow = this;
            
            ///CHANGE TO 5
            if(power >= 100 && master.kills >=5  && !player.isRolling) {
                Debug.Log(master.kills);
                arrowObject.GetComponent<Arrow>().boomArrow = true;
            }
            
            if(player.isRolling && !player.IsGrounded()) {
                power = 50;
                arrowObject.GetComponent<Arrow>().rollArrow = true;
            }
            power = Mathf.Clamp(power, 30, 50);
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
