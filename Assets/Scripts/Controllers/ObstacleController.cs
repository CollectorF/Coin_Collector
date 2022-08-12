using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public delegate void CollidedEvent();

    public event CollidedEvent OnCollided;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnCollided?.Invoke();
            Destroy(collision.gameObject);
        }
    }
}
