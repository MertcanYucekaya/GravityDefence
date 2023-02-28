using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    [System.Serializable]
    public class Level
    {
        public Transform nextLevelTarget;
        public Transform door;
        public string LevelCount;
        public int character;
        public List<GameObject> LevelElementsParent = new();

    }
    [SerializeField] public List<Level> levels = new();
}
