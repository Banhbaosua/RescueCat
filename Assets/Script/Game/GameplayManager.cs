using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GridData gridData;
    [SerializeField] PropsSpawner propsSpawner;
    [SerializeField] NavMeshSurface navMeshSurface;
    [SerializeField] MapCurrentData currentData;


    [SerializeField] StateMachine stateMachine;
    private InputManager inputManager;

    private void Awake()
    {
        propsSpawner.SetGenerateData( currentData.Data);
        propsSpawner.Spawn(currentData.Data.Cars);
        navMeshSurface.BuildNavMesh();

        inputManager = new InputManager();
        inputManager.Initialized();
        playerController.Initialize(inputManager);
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        stateMachine.Initalize(playerController);

    }
}
