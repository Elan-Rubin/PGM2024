using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject levelsParent; 
    [SerializeField] private List<Level> levels = new();
    private int levelIndex = 0;
    public Level GetCurrentLevel()
    {
        return levels[levelIndex];
    }
    public Golfball GetGolfBall() => GetCurrentLevel().Golfball;
    public Golfhole GetGolfHole() => GetCurrentLevel().Golfhole;

    [HideInInspector] public UnityEvent LevelCompletedEvent { get; private set; }

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;

    }

    void Start()
    {
        InitializeLevels();
    }

    void Update()
    {

    }

    private void InitializeLevels()
    {
        foreach(Transform child in levelsParent.transform)
        {
            var n = child.name;
            var g = child.gameObject;
            var h = HelperClass.FindChildWithTag(child.gameObject, "Golfhole");
            var b = HelperClass.FindChildWithTag(child.gameObject, "Golfball");

            var l = new Level(n, g, h.GetComponent<Golfhole>(), b.GetComponent<Golfball>());
            levels.Add(l);
        }

        foreach(var l in levels)
        {
            Debug.Log(l.ToString());
        }
        LevelStart();
    }

    private void LevelStart()
    {
        var l = levels[levelIndex];
        l.Golfhole.InitializeGolfhole(l.Golfball);
        l.Golfball.InitializeGolfball(l.Golfhole);
        Golfer.Instance.InitializeGolfer(l.Golfhole, l.Golfball); 
    }

    public void LevelComplete()
    {
        LevelCompletedEvent.Invoke();

        levelIndex++;
    }

    [System.Serializable]
    public class Level
    {
        private string name;
        [SerializeField] private string levelName;
        [SerializeField] private GameObject levelGO;
        public GameObject LevelGO { get { return levelGO; } }
        public Golfball Golfball;
        public Golfhole Golfhole;

        public Level(string name, GameObject GO, Golfhole hole, Golfball ball)
        {
            levelName = name;
            levelGO = GO;
            Golfhole = hole;
            Golfball = ball;
        }

        public override string ToString()
        {
            return $"Level: {levelName}, golfhole: {Golfhole.transform.position}, golfball: {Golfball.transform.position}.";
        }
    }
}
