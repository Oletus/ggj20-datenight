using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

public class Room : MonoBehaviour
{
    [SerializeField] private StatefulGameObject Flower;
    [SerializeField] private StatefulGameObject ElectricityBill;
    [SerializeField] private StatefulGameObject WaterPipe;
    [SerializeField] private StatefulGameObject WaterCan;
    [SerializeField] private StatefulGameObject Couch;
    [SerializeField] private StatefulGameObject Window;
    [SerializeField] private StatefulGameObject Dog;
    [SerializeField] private StatefulGameObject Ball;
    [System.NonSerialized] public int RoomIndex;


    [SerializeField] private AudioClipLibrary AudioClips;
    [SerializeField] private AudioSource AudioSource;

    public void ApplyState(RoomState roomState)
    {
        this.Flower?.SetState(roomState.FlowerState);
        this.WaterPipe.SetState(roomState.WaterPipeState);
        this.WaterCan.SetState(roomState.WaterCanState);
        this.Flower.SetState(roomState.FlowerState);
        this.ElectricityBill?.SetState(roomState.ElectricityBillState);
        this.Couch.SetState(roomState.CouchState);
        this.Window.SetState(roomState.WindowState);
        this.Dog.SetState(roomState.DogState);
        this.Ball.SetState(roomState.BallState);
    }

    public void PlaySound(string soundName)
    {
        if ( AudioClips != null && AudioSource != null )
        {
            AudioClip clip = AudioClips.Get(soundName);
            if ( clip != null )
            {
                AudioSource.PlayOneShot(clip);
            } else
            {
                Debug.LogError("Could not find audio clip " + soundName);
            }
        }
    }
}
