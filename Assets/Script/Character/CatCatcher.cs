using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Linq;
using UnityEngine.UI;

public class CatCatcher : MonoBehaviour
{
    [SerializeField] SphereCollider catchArea;
    [SerializeField] Transform[] catPosHead;
    [SerializeField] Transform[] catPosTail;
    [SerializeField] SpriteRenderer coneSprite;
    [SerializeField] RectTransform catchBarPrefab;
    [SerializeField] PlayerController playerController;

    private HashSet<Transform> cats = new();
    private int posHeadIndex = 0;
    private int posTailIndex = 0;
    public int Cats => posHeadIndex + posTailIndex;
    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<CatController>() != null)
        {
            coneSprite.enabled = true;
            if (cats.Contains(other.transform)) return;
            cats.Add(other.transform);
            var cat = other.transform.GetComponent<CatController>();
            cat.DetectPlayer();
            var catchable = CheckCatInsideCatchCone(other.transform);
            if (catchable)
            {
                CatchCatTask(cat,2f).Forget();
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        CheckCatsInProximity();
    }

    void CheckCatsInProximity()
    {
        var cats = Physics.OverlapSphere(this.transform.position, catchArea.radius, LayerMask.NameToLayer("Cat"))
            .Select(x => x.GetComponent<CatController>())
            .Where(x => x != null)
            .Where(x => !x.IsCatched)
            .ToArray();
        if (cats.Length == 0)
            coneSprite.enabled = false;
    }

    async UniTaskVoid CatchCatTask(CatController cat ,float timeToCatch)
    {
        var catchBar = Instantiate(catchBarPrefab, cat.transform);
        catchBar.transform.position = new Vector3(0,0.5f,0);
        catchBar.gameObject.SetActive(true);

        await CatchProgress(cat.transform,catchBar, timeToCatch,cancellationTokenSource.Token);

        Destroy(catchBar.gameObject);

        var random = Random.Range(0, 2);
        if (posHeadIndex == catPosHead.Length)
            random = 1;
        if (posTailIndex == catPosTail.Length)
            random = 0;
        if(random == 0)
        {
            cat.DisableCatAgent();
            cat.transform.position = catPosHead[posHeadIndex].transform.position;
            cat.transform.parent = catPosHead[posHeadIndex];
            cat.transform.rotation = Quaternion.identity;
            posHeadIndex++;
        }
        else
        {
            cat.FollowPlayer(catPosTail[posTailIndex],playerController).Forget();
            posTailIndex++;
        }
    }

    bool CheckCatInsideCatchCone(Transform catTransform)
    {
        var direction = catTransform.position - this.transform.position;
        var dotPro = Vector3.Dot(transform.forward, direction.normalized);
        if (dotPro > Mathf.Cos(30))
            return true;
        else
            return false;
    }
    async UniTask CatchProgress(Transform cat,Transform catchBar, float time,CancellationToken token)
    {
        float timer = 0f;
        var slider = catchBar.GetComponentInChildren<Slider>();
        while(timer<time)
        {
            await UniTask.Yield(cancellationToken:token);

            bool inside = CheckCatInsideCatchCone(cat);
            catchBar.gameObject.SetActive(inside);
            if (!inside) continue;

            catchBar.transform.position = new Vector3(cat.transform.position.x, cat.transform.position.y + 10f, cat.transform.position.z);
            catchBar.transform.forward = Camera.main.transform.forward;

            timer += Time.deltaTime;
            slider.value = timer/time;

            if (timer > time)
            {
                catchBar.gameObject.SetActive(false);
                cats.Add(cat);
                CheckCatsInProximity();
                return;
            }
        }
    }

    private void OnDisable()
    {
        cancellationTokenSource?.Cancel();
    }
}
