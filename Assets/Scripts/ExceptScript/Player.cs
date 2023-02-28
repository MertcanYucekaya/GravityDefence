using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] private RagdollControlSc ragdollControlSc;
    private bool isMove;
    [SerializeField] private float moveCoolDown;
    private float moveCoolDownC;
    
    [Header("Force")]
    [SerializeField] private float rigidForce;
    [SerializeField] private float rigidForceY;
    [SerializeField] private GameObject charactersParent;
    private Rigidbody[] addForceRigis;
    [SerializeField] private GameObject moveableObjectParent;
    private Rigidbody[] moveableObjects;
    [SerializeField] private float moveAbleObjectForce;

    [Header("Input")]
    [SerializeField] private float moveForDistance;
    private Touch touch;
    private Vector2 currentTouchPos = Vector2.zero;
    private bool currentPosReset = true;
    [Header("LevelCheck")]
    [SerializeField] private LevelControl levelControl;
    private int nextLevelForDead;
    [HideInInspector] public int currentLevel;
    [Header("NextLevel")]
    Sequence seq;
    [SerializeField] private float targetAnimTime;
    [SerializeField] private float doorOpenAnimTime;
    [SerializeField] private float doorOpenAngle;
    [Header("StickAnim")]
    [SerializeField] private Animator stickAnimator;
    [SerializeField] private float sitckAnimEndTime;
    void Start()
    {
        isMove = true;
        charactersRigidReference();
        moveAbleObjectReference();
        characterReference();
        DOTween.Init();
        nextLevelForDead = 0;
        currentLevel = 0;
        moveCoolDownC = 0;

    }
    void Update()
    {
        if(Input.touchCount == 1 && isMove)
        {
            touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Moved)
            {
                if (currentPosReset)
                {
                    currentTouchPos = touch.position;
                    currentPosReset = false;
                }
                // X Move
                if (Vector2.Distance(currentTouchPos,new Vector2(touch.position.x, currentTouchPos.y)) >= moveForDistance)
                {
                    currentPosReset = true;
                    if (touch.position.x - currentTouchPos.x >= 0)
                    // SAG
                    {
                        move(1);
                    }
                    else
                    // SOL
                    {
                        move(2);
                    }
                }
                // Y Move
                else if (Vector2.Distance(currentTouchPos, new Vector2(currentTouchPos.x, touch.position.y)) >= moveForDistance)
                {
                    currentPosReset = true;
                    if (touch.position.y - currentTouchPos.y >= 0)
                    // YUKARI
                    {
                        move(3);
                    }
                    else
                    // ASSAGI
                    {
                        move(4);
                    }
                }
            }
        }
        else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            currentPosReset = true;
        }
    }
    void move(int vector)
    {
        ragdollControlSc.startCharacterThread();
        ragdollControlSc.setRagdolls(false);
        foreach (Rigidbody r in moveableObjects)
        {
            if (r.CompareTag("Disco"))
            {
                Debug.Log("asdas");
                r.GetComponent<DiscoBall>().discoMethod();
            }
        }
        if (Time.time >= (moveCoolDownC+ moveCoolDown))
        {
            moveCoolDownC = Time.time;
            if (vector == 1)
            {
                stickAnimator.SetInteger("Move", 1);
                Invoke("moveSetMoveAnim", sitckAnimEndTime);
                foreach (Rigidbody r in addForceRigis)
                {
                    r.AddForce(r.transform.position + Vector3.right * rigidForce);
                }
                foreach (Rigidbody r in moveableObjects)
                {
                    r.AddForce(r.transform.position + Vector3.right * moveAbleObjectForce);
                }
            }
            else if (vector == 2)
            {
                stickAnimator.SetInteger("Move", 2);
                Invoke("moveSetMoveAnim", sitckAnimEndTime);
                foreach (Rigidbody r in addForceRigis)
                {
                    r.AddForce(r.transform.position + Vector3.left * rigidForce);
                }
                foreach (Rigidbody r in moveableObjects)
                {
                    r.AddForce(r.transform.position + Vector3.left * moveAbleObjectForce);
                }
            }
            else if (vector == 3)
            {
                stickAnimator.SetInteger("Move", 3);
                Invoke("moveSetMoveAnim", sitckAnimEndTime);
                foreach (Rigidbody r in addForceRigis)
                {
                    r.AddForce(r.transform.position + Vector3.forward * rigidForce);
                }
                foreach (Rigidbody r in moveableObjects)
                {
                    r.AddForce(r.transform.position + Vector3.forward * moveAbleObjectForce);
                }
            }
            else if (vector == 4)
            {
                stickAnimator.SetInteger("Move", 4);
                Invoke("moveSetMoveAnim", sitckAnimEndTime);
                foreach (Rigidbody r in addForceRigis)
                {
                    r.AddForce(r.transform.position + Vector3.up * rigidForceY);
                }
                foreach (Rigidbody r in moveableObjects)
                {
                    r.AddForce(r.transform.position + Vector3.up * moveAbleObjectForce);
                }
            }
        }
    }
    void characterReference()
    {
        foreach(CharactersSc c in levelControl.levels[currentLevel].LevelElementsParent[0].GetComponentsInChildren<CharactersSc>())
        {
            c.coroutineReference();
        }
    }
    void charactersRigidReference()
    {
        addForceRigis = levelControl.levels[currentLevel].LevelElementsParent[0].GetComponentsInChildren<Rigidbody>();

    }
    void moveAbleObjectReference()
    {
        moveableObjects = levelControl.levels[currentLevel].LevelElementsParent[1].GetComponentsInChildren<Rigidbody>();
    }
    private object _lock = new object();
    public void nextLevelCheck()
    {
        lock (_lock)
        {
            nextLevelForDead++;
            Debug.Log(currentLevel);
            if(nextLevelForDead == levelControl.levels[currentLevel].character)
            {
                currentLevel++;
                nextLevelForDead = 0;
                if (currentLevel >= levelControl.levels.Count)
                {
                    isMove = false;
                    GameManager.Instance.LevelState(true);
                }
                else
                {
                    isMove = false;
                    seq = DOTween.Sequence();
                    foreach (GameObject g in levelControl.levels[currentLevel].LevelElementsParent)
                    {
                        g.SetActive(true);
                    }
                    seq.Join(transform.DOMoveZ(levelControl.levels[currentLevel].nextLevelTarget.position.z, targetAnimTime)).SetEase(Ease.InCubic).OnComplete(() =>
                    {
                        foreach (GameObject g in levelControl.levels[currentLevel - 1].LevelElementsParent)
                        {
                            Destroy(g);
                        }
                        charactersRigidReference();
                        moveAbleObjectReference();
                        ragdollControlSc.charactersScReference();
                        characterReference();
                        isMove = true;
                    });
                    
                    seq.Join(levelControl.levels[currentLevel - 1].door.DORotate(new Vector3(0, doorOpenAngle, 0), doorOpenAnimTime));
                }
                
            }
        }
    }

    void moveSetMoveAnim()
    {
        stickAnimator.SetInteger("Move", 0);
    }
}
