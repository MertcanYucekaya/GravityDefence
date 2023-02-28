using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSc : MonoBehaviour
{
    [SerializeField] LevelControl level;
    [SerializeField] Player player;
    bool end = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Contains("Detection"))
        {
            if (end == false)
            {
                GameManager.Instance.LevelState(false);
                end = true;


                foreach (CharactersSc s in level.levels[player.currentLevel].LevelElementsParent[0].GetComponentsInChildren<CharactersSc>())
                {
                    s.gameEnd();
                }
            }
        }
    }
}
