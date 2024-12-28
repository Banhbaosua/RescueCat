using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBehaviour : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] Rigidbody rb;
    private bool movable;
    public event Action OnPlayerReach = delegate { };
    private void FixedUpdate()
    {
        if(movable)
            rb.MovePosition(transform.position+speed * Time.fixedDeltaTime * Vector3.forward);
    }
    public void SetSpeed(float value)
    {
        this.speed = value;
    }

    public void Move()
    {
        movable = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnPlayerReach?.Invoke();
        }
    }
}
