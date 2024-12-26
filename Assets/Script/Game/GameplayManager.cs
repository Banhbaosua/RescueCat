using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GridData gridData;

    private StateMachine stateMachine;
    private InputManager inputManager;
    private GridManager gridManager;

    private void Awake()
    {
        gridManager = new GridManager(gridData);
        gridManager.Generate();

        inputManager = new InputManager();
        inputManager.Initialized();
        playerController.Initialize(inputManager);
        
        stateMachine = new StateMachine();
    }

    private void Start()
    {
        stateMachine.Initalize();
    }
}
