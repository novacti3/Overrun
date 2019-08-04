using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fill : MonoBehaviour
{
    void Update()
    {
        float kills = GameMaster.Instance.kills;
        GetComponent<RectTransform>().localPosition  = new Vector3(0, Mathf.Clamp(-40 + (kills*(100/5)),-40, 60));
        Debug.Log(-100 + (kills*(100/5)));
    }
}
