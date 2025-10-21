using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States
{
    Idle,
    Walking,
    Airborne
}

public class StateMachine : MonoBehaviour
{
    [SerializeField] private GameObject statesGO;
    private Character_Controller character;
    
    private Dictionary<States, IState> statesDict;
    private IState currentState;

    public Character_Controller Character => character;

    private void Start()
    {
        character = GetComponent<Character_Controller>();
        statesDict = new Dictionary<States, IState>();

        IState[] statesList = statesGO.GetComponentsInChildren<IState>();

        foreach (IState state in statesList)
        {
            state.Machine = this;            
            statesDict.Add(state.Type, state);
        }

        currentState = statesDict[States.Idle];
    }

    public void UpdateStateMachine()
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
