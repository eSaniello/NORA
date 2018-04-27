using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask groundLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8) //8 = ground layer
            Destroy(gameObject);
    }
}
