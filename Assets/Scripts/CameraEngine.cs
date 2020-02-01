using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEngine : MonoBehaviour
{
    public GameObject MainCam;
    public GameObject[] RoomCam;

    public void SelectRoom(int roomNumber)
    {
        MainCam.transform.position = RoomCam[roomNumber].transform.position;
        MainCam.transform.rotation = RoomCam[roomNumber].transform.rotation;
    }
}
