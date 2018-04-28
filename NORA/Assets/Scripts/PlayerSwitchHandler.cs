using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerSwitchHandler : MonoBehaviour
{
    public GameObject playerOne;
    public GameObject playerTwo;
    public CinemachineVirtualCamera virtualCam;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (playerOne.GetComponent<PlayerController>().enabled)
            {
                playerOne.GetComponent<PlayerController>().enabled = false;
                virtualCam.Follow = playerTwo.transform;
                playerTwo.GetComponent<PlayerController>().enabled = true;
            }
            else if (playerTwo.GetComponent<PlayerController>().enabled)
            {
                playerTwo.GetComponent<PlayerController>().enabled = false;
                virtualCam.Follow = playerOne.transform;
                playerOne.GetComponent<PlayerController>().enabled = true;
            }
        }
    }   
}
