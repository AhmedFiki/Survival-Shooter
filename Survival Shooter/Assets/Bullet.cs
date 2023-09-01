using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float maxDistance = 100;
    void Update()
    {
        transform.position += transform.right * bulletSpeed * Time.deltaTime;

       
            Destroy(gameObject,3f);
        
    }
}
