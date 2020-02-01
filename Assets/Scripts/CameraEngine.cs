using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEngine : MonoBehaviour
{
    public GameObject MainCam;
    public GameObject[] RoomCam;

    public int RoomNumber { get; private set; }

    public void Awake()
    {
        if ( RoomCam.Length > 3 )
        {
            SelectRoom(3);
        }
    }

    public void SelectRoom(int roomNumber)
    {
        RoomNumber = roomNumber;
        MainCam.transform.position = RoomCam[roomNumber].transform.position;
        MainCam.transform.rotation = RoomCam[roomNumber].transform.rotation;
    }
}
