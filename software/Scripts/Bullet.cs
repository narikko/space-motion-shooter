using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Rigidbody rb;
    public float timer = 0.5f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }
    public void shoot(Vector3 direction, float force)
    {
        rb.AddForce(direction*force);
    }

    private float timePassed = 0;

    private void Update()
    {
        timePassed += Time.deltaTime;
        if(timePassed > timer)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
