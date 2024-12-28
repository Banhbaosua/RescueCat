using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public enum GameState
{
    KickStart,
    Main,
    Marathon,
    Win,
    Lose,
}
public class StateMachine : MonoBehaviour
{
    [SerializeField] private float kickStartTime;
    [SerializeField] SpeedoMeter speedoMeter;
    [SerializeField] StaminaBar staminaBar;
    [SerializeField] Collider marathonPhaseTrigger;
    [SerializeField] Collider marathonDestination;
    [SerializeField] static GameObject loadingScreenPrefab;
    [SerializeField] WinBoard victoryBoard;
    [SerializeField] CatCatcher catCatcher;
    [SerializeField] WaveBehaviour waveBehaviour;
    [SerializeField] MapCurrentData mapCurrentData;

    private BaseState currentState;
    private KickStartPhase kickStartState;
    private MainPhase mainState;
    private MarathonPhase marathonState;
    private Win winState;
    private Lose loseState;
    private PlayerController playerController;
    private LoadingScreen loadingScreen;
    [Header("Camera")]
    public CinemachineVirtualCamera startCamera;
    public CinemachineVirtualCamera mainCamera;
    public CinemachineVirtualCamera marathonCamera;
    public CinemachineBrain cameraBrain;
    public CharacterData characterData;
    public PlayerController PlayerController => playerController;
    public float KickStartTime => kickStartTime;
    public SpeedoMeter SpeedoMeter => speedoMeter;
    public StaminaBar StaminaBar => staminaBar;
    public Collider MarathonPhaseTrigger => marathonPhaseTrigger;
    public Collider MarathonDestination => marathonDestination;
    public LoadingScreen LoadingScreen => loadingScreen;
    public WinBoard VictoryBoard => victoryBoard;
    public CatCatcher CatCatcher => catCatcher;
    public WaveBehaviour WaveBehaviour => waveBehaviour;
    private Dictionary<GameState,BaseState> states;
    public CompositeDisposable disposables;
    public void Initalize(PlayerController playerController)
    {

        disposables = new CompositeDisposable();
        states = new Dictionary<GameState,BaseState>();
        kickStartState = new KickStartPhase(this);
        mainState = new MainPhase(this);
        winState = new Win(this);
        loseState = new Lose(this);
        marathonState = new MarathonPhase(this);
        this.playerController = playerController;

        var stamina = characterData.GetStamina();
        staminaBar.SetText(stamina, stamina);

        states.Add(GameState.KickStart, kickStartState);
        states.Add(GameState.Main, mainState);
        states.Add(GameState.Marathon, marathonState);
        states.Add(GameState.Win, winState);
        states.Add(GameState.Lose, loseState);

        marathonPhaseTrigger.OnTriggerEnterAsObservable()
            .Where(x => x.CompareTag("Player"))
            .Subscribe(_ => ChangeState(GameState.Marathon)).AddTo(disposables);

        marathonDestination.OnTriggerEnterAsObservable()
            .Where(x => x.CompareTag("Player"))
            .Subscribe(_ => ChangeState(GameState.Win)).AddTo(disposables);

        victoryBoard.Button.onClick.AddListener(() =>
        {
            characterData.IncreaseDif();
            mapCurrentData.Data.Used();
            LoadSceneAsyncUtil.Instance.LoadAsync("UpgradeSpeed").Forget(); 
        });
        waveBehaviour.OnPlayerReach += () => ChangeState(GameState.Lose);

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

    public float GetReward()
    {
        return CatCatcher.Cats * 6 * characterData.GetIncom();
    }

    public async UniTask CameraTransition(CinemachineVirtualCamera camera)
    {
        Debug.Log("start");
        camera.enabled = true;
        await UniTask.WaitUntil(() => cameraBrain.IsBlending);
        await UniTask.WaitUntil(() => !cameraBrain.IsBlending);
    }

    private void OnDisable()
    {
        currentState?.OnStateExit();
    }
}
