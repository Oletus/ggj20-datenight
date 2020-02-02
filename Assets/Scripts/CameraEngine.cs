using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraEngine : MonoBehaviour
{
    public GameObject MainCam;
    public GameObject[] RoomCam;

    [SerializeField] private Graphic WhiteFlash;

    private bool Initialized = false;

    public int RoomNumber { get; private set; }

    [SerializeField] private AudioSource TimeMachineSound;

    public void Awake()
    {
        if ( RoomCam.Length > 3 )
        {
            SelectRoom(3);
        }
    }

    public void SelectRoom(int roomNumber)
    {
        if ( RoomNumber == roomNumber )
        {
            return;
        }
        if ( !Initialized )
        {
            if ( WhiteFlash != null )
            {
                WhiteFlash.gameObject.SetActive(true);
                WhiteFlash.canvasRenderer.SetAlpha(0.0f);
            }
        }
        else
        {
            if ( WhiteFlash != null )
            {
                WhiteFlash.canvasRenderer.SetAlpha(1.0f);
                WhiteFlash.CrossFadeAlpha(0.0f, 0.2f, false);
                TimeMachineSound.Play();
            }
        }
        Initialized = true;
        RoomNumber = roomNumber;
        MainCam.transform.position = RoomCam[roomNumber].transform.position;
        MainCam.transform.rotation = RoomCam[roomNumber].transform.rotation;
    }
}
