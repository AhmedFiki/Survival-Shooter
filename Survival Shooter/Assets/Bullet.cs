using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Vector3 moveDirection;

    public float bulletSpeed = 10f;
    public float maxDistance = 100;

    private void Awake()
    {
        SetDirection(transform.right);
    }
    private void Start()
    {
        
    }


    void Update()
    {
        transform.position += moveDirection * bulletSpeed * Time.deltaTime;
        Destroy(gameObject,3f);  
    }


    public void SetDirection(Vector3 direction)
    {
        //Debug.Log(direction+"    "+transform.right + gameObject.name);
        moveDirection = direction.normalized;
    }
}
