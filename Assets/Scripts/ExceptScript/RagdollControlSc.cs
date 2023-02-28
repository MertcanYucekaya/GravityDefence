using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollControlSc : MonoBehaviour
{
    [SerializeField] private LevelControl level;
    [SerializeField] private Player player;
    [SerializeField] private GameObject charactersParent;
    [SerializeField] public float enableRagdollCount;
    CharactersSc[] characters;
    

    [Header("HipsData")]
    [SerializeField] public Transform hipsDataObj;
    [SerializeField] public Transform hips;
    [SerializeField] public Transform[] hipsElements;
    [HideInInspector] public List<Quaternion> limbRot = new();
    [SerializeField] public float standUpTime;
    void Start()
    {
        foreach(Transform t in hipsElements)
        {
            limbRot.Add(t.rotation);
        }

        charactersScReference();
    }

    void Update()
    {
        
    }
    public void startCharacterThread()
    {
        foreach (CharactersSc sc in characters)
        {
            sc.ragdollThreadControl();
        }
    }

    public void setRagdolls(bool disable)
    {
        foreach (CharactersSc g in characters)
        {
            if (disable)
            {
                g.setRicisKinematic(true);
            }
            else
            {
                g.setRicisKinematic(false);
            }
        }
    }
    public void charactersScReference()
    {
        characters = level.levels[player.currentLevel].LevelElementsParent[0].GetComponentsInChildren<CharactersSc>();
    }
}
