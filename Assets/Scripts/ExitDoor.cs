using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ExitDoor : MonoBehaviour
{
    public delegate void ExitDoorUsed();
    public static event ExitDoorUsed OnExitDoorUsed;

    public bool isUnlocked = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Player in exit door range");
        if(isUnlocked && collision.gameObject.GetComponent<Player>() && Input.GetKeyDown(KeyCode.E))
        {
            OnExitDoorUsed?.Invoke();
            Debug.Log("Exit door used");
        }
    }
}
