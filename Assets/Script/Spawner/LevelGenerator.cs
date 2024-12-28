using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] MapGenerateData[] mapDatas;
    [SerializeField] GridData gridData;
    [SerializeField] PropsSpawner propsSpawner;
    [SerializeField] int numberOfCars;
    [SerializeField] CharacterData characterData;
    GridManager gridManager;
    int difficultLevel;
    int mapIndex;
    public MapGenerateData[] MapDatas => mapDatas;

    public void Generate()
    {
        difficultLevel = characterData.GetDifficultLevel();
        mapIndex = 0;
        gridManager = new GridManager(gridData);
        Debug.Log(mapDatas.Length);
        for (int i = 0; i < mapDatas.Length; i++)
        {
            GenerateMapDataTask().Forget();
        }
    }

    (GameObject,GameObject) SpawnRandomPrefab(bool isCar)
    {
        GameObject prefab;
        if (isCar)
            prefab = propsSpawner.GetRandomCarPrefab();
        else
            prefab = propsSpawner.GetRandomObstaclePrefab();
        var GO = Instantiate(prefab);
        GO.SetActive(false);
        float randomYRot = Random.Range(0, 180f);
        prefab.transform.rotation = Quaternion.Euler(new Vector3(0,randomYRot,0));

        float minX = gridData.minAnchor.x;
        float maxX = gridData.maxAnchor.x;
        float minY = gridData.minAnchor.y;
        float maxY = gridData.maxAnchor.y;

        var randomXPos = Random.Range(minX,maxX);
        var randomYPos = Random.Range(minY,maxY);
        GO.transform.position = new Vector3(randomXPos, 0, randomYPos);
        return (GO,prefab);
    }

    void SaveMapGenerateData(PropData[] data,int cars)
    {
        mapDatas[mapIndex].SetPropsData(data,cars);
        mapDatas[mapIndex].RenewUse();
    }
    async UniTaskVoid GenerateMapDataTask()
    {
        var grid = gridManager.Generate();
        List<UniTask<(GameObject,GameObject)>> tasks = new List<UniTask<(GameObject, GameObject)>>();
        int cars = numberOfCars;
        for(int i =0; i< numberOfCars + difficultLevel; i++) 
        {
            await UniTask.Yield();
            UniTask<(GameObject,GameObject)> task;
            if(cars > 0)
                task = GeneratePropTask(grid,true);
            else
                task = GeneratePropTask(grid,false);
            tasks.Add(task);
            cars--;
        }

        (GameObject, GameObject)[] gameObjects = await UniTask.WhenAll(tasks);
        PropData[] propDatas = new PropData[gameObjects.Length];
        for(int i =0; i < gameObjects.Length; i++)
        {
            var propPref = gameObjects[i];
            var pos = propPref.Item1.transform.position;
            var rot = propPref.Item1.transform.rotation;
            var pref = propPref.Item2;
            PropData data = new PropData(pos,rot,pref);
            propDatas[i] = data;
        }
        SaveMapGenerateData(propDatas,numberOfCars);
        difficultLevel++;
        mapIndex++;
        Debug.Log(mapIndex);
    }

    async UniTask<(GameObject,GameObject)> GeneratePropTask(GridUnit[][] grid,bool isCar)
    {
        while (true)
        {
            (GameObject, GameObject) propAndPref;
            if (isCar)
                propAndPref = SpawnRandomPrefab(true);
            else
                propAndPref = SpawnRandomPrefab(false);
            var prop = propAndPref.Item1;
            prop.SetActive(false);
            var pref = propAndPref.Item2;
            prop.SetActive(true);
            var collider = prop.GetComponent<Collider>();
            var minBound = collider.bounds.min;
            var maxBound = collider.bounds.max;

            var stepsMinBoundX = gridManager.StepsXToReachPos(minBound.x);
            var stepsMinBoundY = gridManager.StepsYToReachPos(minBound.z);

            var stepsMaxBoundX = gridManager.StepsXToReachPos(maxBound.x);
            var stepsMaxBoundY = gridManager.StepsYToReachPos(maxBound.z);
            var stepsToCheck = stepsMaxBoundY - stepsMinBoundY;
            var check = CheckLeftRight(stepsMinBoundX, stepsMaxBoundX, stepsMinBoundY, stepsToCheck, grid);
            if (check)
            {
                var popullatable = gridManager.OccupiedUnitsOfGrid(stepsMinBoundX, stepsMaxBoundX, stepsMinBoundY, stepsMaxBoundY,grid);
                if (!popullatable)
                    continue;
                return (prop,pref);
            }
            await UniTask.Yield();
        }
    }

    bool CheckLeftRight(int stepsMinX, int stepsMaxX,int minY,int stepsToCheck, GridUnit[][] grid)
    {
        bool leftAvailable = false;
        bool rightAvailable  = false;
        if(stepsMinX > 0)
            leftAvailable = gridManager.CheckGridOccupiedLeft(stepsMinX, minY, stepsToCheck, grid);
        if(stepsMaxX < gridManager.Grids.Length-1)
            rightAvailable = gridManager.CheckGridOccupiedRight(stepsMaxX, minY, stepsToCheck, grid);

        if (!leftAvailable && !rightAvailable)
            return false;
        return true;
    }
}
