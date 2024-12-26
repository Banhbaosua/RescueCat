using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsLoop : MonoBehaviour
{
    [SerializeField] Transform[] props;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform despawnPoint;
    private float speed = 0f;
    void Update()
    {
        if(speed > 0f)
            Move();
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    void Move()
    {
        foreach (var prop in props) 
        {
            prop.transform.position = new Vector3(prop.position.x,prop.position.y,prop.position.z - speed*Time.deltaTime );
            if(prop.transform.position.z < despawnPoint.position.z)
            {
                prop.transform.position = new Vector3(prop.position.x, prop.position.y, spawnPoint.position.z);
            }
        }
    }
}
