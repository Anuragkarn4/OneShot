using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    
    private Rigidbody rb;
    private BowController owner;
    private bool hasHit = false;
    public float lifeTime = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    public void SetOwner(BowController bow)
    {
        owner = bow;
    }


    void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (other.CompareTag("Fruit"))
        {
            hasHit = true;
            Destroy(other.gameObject);
            GameManager.Instance.AddScore(10);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Bomb"))
        {
            hasHit = true;
            Destroy(other.gameObject);
            GameManager.Instance.OnBombHit();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            hasHit = true;
            Destroy(gameObject, 0.2f);  // brief delay so you see it stick
        }
    }
}