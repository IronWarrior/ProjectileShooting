using UnityEngine;

public class CannonBall : MonoBehaviour 
{
    [SerializeField]
    GameObject deathEffect;

    void OnCollisionEnter(Collision collision)
    {
        Instantiate(deathEffect, transform.position, Quaternion.LookRotation(collision.contacts[0].normal));

        Destroy(gameObject);
    }
}
