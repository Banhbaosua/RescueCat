using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchBar : MonoBehaviour
{
    void Update()
    {
        this.transform.forward = Camera.main.transform.forward;
    }
}
