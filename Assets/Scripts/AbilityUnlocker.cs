using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AbilityUnlocker : MonoBehaviour
{
    public bool unlockDoubleJump = false;
    public bool unlockWallJump = false;
    public bool unlockDash = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("AYO");
            if (unlockDoubleJump)
                collision.GetComponent<PlayerController2D>().canDoubleJump = true;
            else if (unlockWallJump)
                collision.GetComponent<PlayerController2D>().canWallJump = true;
            else if (unlockDash)
                collision.GetComponent<PlayerController2D>().canDash = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            Destroy(gameObject);
    }


    //editor scripting
    private void OnDrawGizmos()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();

        Gizmos.color = new Color(0f, 1f, 0f, 1f);
        Gizmos.DrawWireCube(transform.position + new Vector3(col.offset.x, col.offset.y, 0f), col.size);

        Gizmos.color = new Color(0f, 1f, 0f, .3f);
        Gizmos.DrawCube(transform.position + new Vector3(col.offset.x, col.offset.y, 0f), col.size);
    }

    private void OnDrawGizmosSelected()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();

        Gizmos.color = new Color(1f, 1f, 0f, 1f);
        Gizmos.DrawWireCube(transform.position + new Vector3(col.offset.x, col.offset.y, 0f), col.size);

        Gizmos.color = new Color(1f, 1f, 0f, .3f);
        Gizmos.DrawCube(transform.position + new Vector3(col.offset.x, col.offset.y, 0f), col.size);
    }
}
