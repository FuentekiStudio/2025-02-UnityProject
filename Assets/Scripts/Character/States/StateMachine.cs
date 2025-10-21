using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States
{
    Idle,
    Walking,
    Jumping
}

public class StateMachine : MonoBehaviour
{
    //List<IState> states = new List<IState>();
    [SerializeField] private GameObject statesGO;
    private Character_Controller character;
    
    private Dictionary<States, IState> statesDict;
    private IState currentState;

    public Character_Controller Character => character;

    private void Start()
    {
        character = GetComponent<Character_Controller>();

        IState[] statesList = statesGO.GetComponentsInChildren<IState>();

        foreach (IState state in statesList)
        {
            statesDict.Add(state.Type, state);
        }

        if (!statesDict.TryGetValue(States.Idle, out currentState))
        {
            Debug.Log("Couldn't find Idle State");
        }
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    public void ChangeState(States newState)
    {
        currentState.Exit();
        currentState = statesDict[newState];
        currentState.Enter();
    }
}
