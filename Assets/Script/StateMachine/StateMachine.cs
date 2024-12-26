using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    KickStart,
    Main,
    Marathon,
}
public class StateMachine : MonoBehaviour
{
    private BaseState currentState;
    private KickStartPhase kickStartState;
    private MainPhase mainState;
    private MarathonPhase marathonState;

    public CinemachineVirtualCamera startCamera;
    public CinemachineVirtualCamera mainCamera;
    public CinemachineBrain cameraBrain;
    public float kickStartStateTime;



    private Dictionary<GameState,BaseState> states;
    public void Initalize()
    {
        states = new Dictionary<GameState,BaseState>();
        kickStartState = new KickStartPhase(this);
        mainState = new MainPhase(this);
        marathonState = new MarathonPhase(this);

        states.Add(GameState.KickStart, kickStartState);
        states.Add(GameState.Main, mainState);
        states.Add(GameState.Marathon, marathonState);
        currentState = kickStartState;
        currentState.OnStateEnter();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.StateUpdate();
    }

    public void ChangeState(GameState state)
    {

        currentState.OnStateExit();
        currentState = states[state];
        currentState.OnStateEnter();
    }
}
