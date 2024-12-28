using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAsyncUtil : MonoBehaviour
{
    [SerializeField] GameObject loadingScreenPrefab;
    private GameObject loadingScreenGO;

    public static LoadSceneAsyncUtil Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

    }
    public async UniTaskVoid LoadAsync(string scene)
    {
        Time.timeScale = 1;
        var loadSceneTask = SceneManager.LoadSceneAsync(scene);
        loadSceneTask.allowSceneActivation = false;
        if(loadingScreenGO == null)
            loadingScreenGO = Instantiate(loadingScreenPrefab,this.transform);

        loadingScreenGO.SetActive(true);
        var loadingScreen = loadingScreenGO.GetComponent<LoadingScreen>();
        while (loadSceneTask.progress < 0.9f)
        {
            await UniTask.Yield();
            loadingScreen.UpdateFill(loadSceneTask.progress);
        }

        await UniTask.Delay(1000);
        loadSceneTask.allowSceneActivation = true;
        loadingScreen.gameObject.SetActive(false);
    }
}
