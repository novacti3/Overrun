using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Util class, attach to gameobject to not destroy them 
public class DDOL : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
