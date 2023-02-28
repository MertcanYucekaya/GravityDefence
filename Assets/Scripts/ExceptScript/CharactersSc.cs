using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CharactersSc : MonoBehaviour
{
    private bool scriptEnabled = true;
    private RagdollControlSc ragdollControlSc;
    [SerializeField] Rigidbody[] rigis;
    private Player player;
    private Transform target;
    private Coroutine threadCheck;
    [Header("StandUp")]
    [SerializeField] private Transform hips;
    [SerializeField] private Rigidbody hipsRigi;
    [SerializeField] private List<Transform> hipsElements;
    [HideInInspector] public Sequence seq;
    [HideInInspector] public Sequence runSeq;
    [Header("Kill")]
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private Material killMaterial;
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private float runTimeMiltiplier;
    [Header("Astonishment")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string writing;
    [SerializeField] private float astoTime;

    void Start()
    {
        StartCoroutine(referenceSet());
        DOTween.Init();
    }

    Coroutine co = null;
    private void OnDestroy()
    {
        StopCoroutine(characterRagdollEnable());
    }
    public void ragdollThreadControl()
    {
        if (co != null && scriptEnabled)
        {
            StopCoroutine(co);
        }
        StopCoroutine(characterRagdollEnable());
        co = StartCoroutine(characterRagdollEnable());
    }
    IEnumerator characterRagdollEnable()
    {
        yield return new WaitForSeconds(ragdollControlSc.enableRagdollCount);
        if (scriptEnabled)
        {
            setRicisKinematic(true);
            hipsRigi.isKinematic = true;

            seq = DOTween.Sequence();

            seq.Join(hips.transform.DOMoveY(ragdollControlSc.hips.position.y, ragdollControlSc.standUpTime));
            seq.Join(hips.transform.DORotateQuaternion(ragdollControlSc.hips.rotation, ragdollControlSc.standUpTime));
            seq.AppendInterval(ragdollControlSc.standUpTime);
            for (int i = 0; i < hipsElements.Count; i++)
            {
                seq.Join(hipsElements[i].transform.DORotateQuaternion(ragdollControlSc.limbRot[i], ragdollControlSc.standUpTime));
            }
            seq.OnComplete(() =>
            {
                transform.position = new Vector3(hips.position.x, ragdollControlSc.hipsDataObj.position.y, hips.position.z);
                runMethod();
            });
        }

    }
    void runMethod()
    {
        runSeq = DOTween.Sequence();
        animator.SetBool("Wakeup", true);
        animator.enabled = true;
        transform.LookAt(target);
        float animTime = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z)
            , new Vector3(target.transform.position.x, 0, target.transform.position.z)) * runTimeMiltiplier;

        runSeq.Join(transform.DOMove(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), animTime));
    }
    public void setRicisKinematic(bool disable)
    {
        if (disable)
        {
            foreach (Rigidbody r in rigis)
            {
                r.isKinematic = true;
            }
        }
        else
        {
            if (cor == false)
            {
                StopCoroutine(threadCheck);
                text.enabled = false;
            }
            animator.enabled = false;
            seq.Kill();
            runSeq.Kill();
            foreach (Rigidbody r in rigis)
            {
                r.isKinematic = false;
            }
        }
    }


    public void onDetection()
    {
        skinnedMeshRenderer.material = killMaterial;
        seq.Kill();
        scriptEnabled = false;
        setRicisKinematic(false);
        player.nextLevelCheck();
    }
    IEnumerator referenceSet()
    {
        yield return new WaitForSeconds(.2f);
        ragdollControlSc = GameObject.Find("RagdollsControl").GetComponent<RagdollControlSc>();
        player = GameObject.Find("Player").GetComponent<Player>();
        foreach (Transform t in player.GetComponentsInChildren<Transform>())
        {
            if (t.CompareTag("Target"))
            {
                target = t;
            }
        }
    }
    public void gameEnd()
    {
        animator.SetBool("Wakeup", false);
        seq.Kill();
        runSeq.Kill();
    }
    bool cor = false;
    public void coroutineReference()
    {
        threadCheck = StartCoroutine(astoMethod());
    }
    public IEnumerator astoMethod()
    {
        yield return new WaitForSeconds(astoTime/2);
        text.text = writing;
        yield return new WaitForSeconds(astoTime);
        text.enabled = false;
        runMethod();
        cor = true;

    }
}
