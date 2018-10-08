using UnityEngine;
using System.Collections;

public class Bash : MonoBehaviour
{
    private PlayerController2D playerController;
    private float bashReachRadius;
    private float bashSpeed;
    private float maxTimeToWaitForBash;
    private bool canBash;
    private Transform arrow;
    //private GameObject effect;
    private Vector3 direction;
    private RaycastHit2D[] objectsNear;
    private bool _canBash;
    private GameObject bashableObj;
    
    void Start()
    {
        playerController = GetComponent<PlayerController2D>();
        bashReachRadius = playerController.bashReachRadius;
        bashSpeed = playerController.bashSpeed;
        maxTimeToWaitForBash = playerController.maxTimeToWaitForBash;
        arrow = playerController.arrow;
        //effect = playerController.effect;
        arrow.gameObject.SetActive(false);
    }


    IEnumerator BashCounter()
    {
        float pauseTime = Time.realtimeSinceStartup + maxTimeToWaitForBash;

        while (Time.realtimeSinceStartup < pauseTime)
        {
            yield return null;
        }

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1f;
            _canBash = false;
            arrow.gameObject.SetActive(false);
        }
    }

    Vector3 _direction;
    void Update()
    {
        canBash = playerController.canBash;

        if ((Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.C)) && canBash)
        {
            objectsNear = Physics2D.CircleCastAll(transform.position, bashReachRadius, Vector3.forward);
            {
                foreach (RaycastHit2D obj in objectsNear)
                {
                    if (obj.collider.gameObject.CompareTag("Bullet"))
                    {
                        bashableObj = obj.collider.gameObject;
                        StartCoroutine("BashCounter");
                        Time.timeScale = 0;

                        _canBash = true;
                        arrow.position = bashableObj.transform.position;
                        arrow.Translate(0, 0, 10);

                        arrow.gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }
        else if ((Input.GetButtonUp("Fire2") || Input.GetKeyUp(KeyCode.C)) && _canBash)
        {
            Time.timeScale = 1;

            direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - bashableObj.transform.position);
            direction.z = 0;
            direction = direction.normalized;
            Debug.Log("Mouse: " + direction);

            //_direction = playerController._velocity;
            //_direction = _direction.normalized;

            //_direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            //_direction = _direction.normalized;
            //Debug.Log("Keypad: " + _direction);

            playerController._velocity = direction * bashSpeed;

            bashableObj.GetComponent<Rigidbody2D>().velocity = direction * (-1) * bashableObj.GetComponent<Rigidbody2D>().velocity.magnitude;

            float width = bashableObj.GetComponent<SpriteRenderer>().bounds.size.x;
            float height = bashableObj.GetComponent<SpriteRenderer>().bounds.size.y;

            float tan = Mathf.Atan2(direction.y, direction.x);

            int roundCos = Mathf.RoundToInt(Mathf.Cos(tan));
            int roundSin = Mathf.RoundToInt(Mathf.Sin(tan));

            playerController.enabled = false;
            //transform.position = bashableObj.transform.position + direction * 1.2f;
            transform.position = new Vector3(bashableObj.transform.position.x + (width * roundCos), bashableObj.transform.position.y + (height * roundSin), 0);
            playerController.enabled = true;

            _canBash = false;
            arrow.gameObject.SetActive(false);
        }
        else if ((Input.GetButton("Fire2") || Input.GetKey(KeyCode.C)) && _canBash)
        {
            //Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            //diff.Normalize();

            Vector3 diff = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            diff.Normalize();
            Debug.Log("Keypad: " + _direction);

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
            //Instantiate(effect, bashableObj.transform.position, Quaternion.identity);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bashReachRadius);
    }
}