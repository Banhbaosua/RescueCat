using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using System.Threading;

public class CatController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] SphereCollider moveArea;
    [SerializeField] Transform finalDestination;
    private bool playerDetected;
    private bool isCatched;

    public bool IsCatched => isCatched;
    CancellationTokenSource cancellationTokenSource;
    private void Start()
    {
        RandomMoveTask().Forget();
    }
    async UniTask MoveToDestinationTask()
    {
        cancellationTokenSource = new CancellationTokenSource();
        await UniTask.WaitUntil((() => agent.remainingDistance <= agent.stoppingDistance), cancellationToken: cancellationTokenSource.Token);
    }

    async UniTaskVoid RandomMoveTask()
    {
        while (!playerDetected)
        {
            var newDes = RandomPos();
            
            agent.SetDestination(newDes);
            await MoveToDestinationTask();
        }
    }

    Vector3 RandomPos()
    {
        var minBound2D =  new Vector2(moveArea.bounds.min.x, moveArea.bounds.min.z);
        var maxBound2D = new Vector2(moveArea.bounds.max.x, moveArea.bounds.max.z);
        var randomX = Random.Range(minBound2D.x,maxBound2D.x);
        var randomZ = Random.Range(minBound2D.y, maxBound2D.y);
        return new Vector3(randomX,0,randomZ);
    }

    public void DetectPlayer()
    {
        playerDetected = true;
        cancellationTokenSource?.Cancel();
        agent.SetDestination(finalDestination.position);
    }

    public void DisableCatAgent()
    {
        isCatched = true;
        cancellationTokenSource?.Cancel();
        agent.enabled = false;
        transform.GetComponent<Collider>().enabled = false;
    }

    public async UniTaskVoid FollowPlayer(Transform player,PlayerController playerCtl)
    {
        isCatched = true;
        float speed = 0f;
        while(true)
        {
            agent.SetDestination(player.transform.position);
            if(playerCtl.CurrentSpeed > speed)
                speed = playerCtl.CurrentSpeed;
            agent.speed = speed;
            Debug.Log(agent.speed);
            await UniTask.Yield();
        }
    }

    public void Destination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    private void OnDisable()
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
    }
}
