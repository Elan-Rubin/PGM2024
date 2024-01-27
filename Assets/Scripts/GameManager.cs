using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
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

        InitializeLevels();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void InitializeLevels()
    {
        foreach (var l in levels)
        {
            var h = HelperClass.FindChildWithTag(l.LevelGO, "Golfhole");
            var b = HelperClass.FindChildWithTag(l.LevelGO, "Golfball");
            l.Initializelevel(h.GetComponent<Golfhole>(), b.GetComponent<Golfball>());
        }
    }

    public void LevelCompleted()
    {
        LevelCompletedEvent.Invoke();


    }

    [System.Serializable]
    public struct Level
    {
        private string name;
        [SerializeField] private string levelName;
        [SerializeField] private GameObject levelGO;
        public GameObject LevelGO { get { return levelGO; } }
        private Golfball ball;
        public Golfball Golfball { get { return ball; } set { ball = value; } }
        private Golfhole hole;
        public Golfhole Golfhole { get { return hole; } set { hole = value; } }

        public void Initializelevel(Golfhole hole, Golfball ball)
        {
            Golfhole = hole;
            Golfball = ball;
        }
    }
}
