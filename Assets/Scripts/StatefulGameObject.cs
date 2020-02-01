using System.Collections.Generic;
using UnityEngine;

public class StatefulFlower : StatefulGameObject<FlowerState> { }
public class StatefulElectricityBill : StatefulGameObject<ElectricityBillState> { }

public abstract class StatefulGameObject<T> : MonoBehaviour
    where T : System.Enum
{
    [System.Serializable]
    public class StateObjectPair
    {
        public T State;
        public GameObject Object; // TODO: do we actually want this to be a list?
    }

    // TODO: Add NaughtyAttributes library
    //[ReorderableList]
    [SerializeField] private List<StateObjectPair> States;

    public void SetState(T activeState)
    {
        foreach(StateObjectPair pair in States)
        {
            pair.Object.SetActive(pair.State.Equals(activeState));
        }
    }
}
