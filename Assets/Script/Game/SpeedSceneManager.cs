using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSceneManager : MonoBehaviour
{
    [SerializeField] SpeedSceneBehaviour speedSceneBehaviour;
    [SerializeField] UpgradeController upgradeController;
    [SerializeField] MapCurrentData currentData;
    [SerializeField] CharacterData characterData;
    [SerializeField] LevelGenerator levelGenerator;

    private InputManager inputManager;
    private void Awake()
    {
        inputManager = new InputManager();
        inputManager.Initialized();

        upgradeController.Initiallize();

        speedSceneBehaviour.Initialized();
        levelGenerator.Generate();
    }
}
