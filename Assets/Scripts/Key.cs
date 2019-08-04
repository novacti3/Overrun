using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Key : MonoBehaviour
{
    public delegate void KeyPickedUp();
    public static event KeyPickedUp OnKeyPickedUp;

    [SerializeField]
    AudioSource Keysound;

    public void Pickup()
    {
        Keysound.Play();
        OnKeyPickedUp?.Invoke();
        Debug.Log("Key picked up");
        Destroy(gameObject);
    }

    private void OnCOllisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
        }
    }
}
