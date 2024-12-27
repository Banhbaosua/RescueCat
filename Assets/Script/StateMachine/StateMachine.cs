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
    [SerializeField] private float kickStartTime;
    [SerializeField] SpeedoMeter speedoMeter;
    [SerializeField] StaminaBar staminaBar;

    private BaseState currentState;
    private KickStartPhase kickStartState;
    private MainPhase mainState;
    private MarathonPhase marathonState;
    private PlayerController playerController;

    public CinemachineVirtualCamera startCamera;
    public CinemachineVirtualCamera mainCamera;
    public CinemachineBrain cameraBrain;
    public CharacterData characterData;
    public PlayerController PlayerController => playerController;
    public float KickStartTime => kickStartTime;
    public SpeedoMeter SpeedoMeter => speedoMeter;
    public StaminaBar StaminaBar => staminaBar;

    private Dictionary<GameState,BaseState> states;
    public void Initalize(PlayerController playerController)
    {
        states = new Dictionary<GameState,BaseState>();
        kickStartState = new KickStartPhase(this);
        mainState = new MainPhase(this);
        marathonState = new MarathonPhase(this);
        this.playerController = playerController;

        var stamina = characterData.GetStamina();
        staminaBar.SetText(stamina, stamina);

        states.Add(GameState.KickStart, kickStartState);
        states.Add(GameState.Main, mainState);
        states.Add(GameState.Marathon, marathonState);
        currentState = kickStartState;
        currentState.OnStateEnter();
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.StateUpdate();
    }

    public void ChangeState(GameState state)
    {
        currentState.OnStateExit();
        currentState = states[state];
        currentState.OnStateEnter();
    }
}
