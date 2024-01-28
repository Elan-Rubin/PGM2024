using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject levelsParent; 
    [SerializeField] private List<LevelInfo> levels = new();
    private int levelIndex = 0;

    bool beeping = true;
    public LevelInfo GetCurrentLevel()
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

            var l = new LevelInfo(n, g, h.GetComponent<Golfhole>(), b.GetComponent<Golfball>());
            levels.Add(l);

            child.gameObject.SetActive(false);
        }

        /*foreach(var l in levels)
        {
            Debug.Log(l.ToString());
        }*/
        LevelStart();
    }

    private void LevelStart()
    {
        var l = levels[levelIndex];
        l.LevelGO.SetActive(true);
        
        l.Golfhole.InitializeGolfhole(l.Golfball);
        l.Golfball.InitializeGolfball(l.Golfhole);
        Golfer.Instance.InitializeGolfer(l.Golfhole, l.Golfball);
        CameraManager.Instance.CenterCamera();
        UIManager.Instance.FadeIn();

        var element = l.LevelGO.GetComponent<Level>().Element;
        switch (element)
        {
            case LevelElement.Wind:

                break;
            case LevelElement.Beeping:
                StartCoroutine(nameof(BeepingCoroutine));
                beeping = true;
                break;
            case LevelElement.Exploding:
                break;
            case LevelElement.StrongWind:
                break;
            case LevelElement.Wizard:
                break;
            case LevelElement.Streaker:
                break;
        }
    }

    private IEnumerator BeepingCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlaySoundEffect("beep");
        StartCoroutine(nameof(BeepingCoroutine));
    }

    public void StopBeeping() => StartCoroutine(nameof(StopBeepingCoroutine));
    private IEnumerator StopBeepingCoroutine()
    {
        beeping = false;
        yield return new WaitForSeconds(1f);

    }

    public void LevelComplete() => StartCoroutine(nameof(LevelCompleteCoroutine));
    private IEnumerator LevelCompleteCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        //LevelCompletedEvent.Invoke();
        //disable previous level
        levels[levelIndex].LevelGO.SetActive(false);

        levelIndex++;
        UIManager.Instance.FadeOut();
        yield return new WaitForSeconds(0.75f);

        LevelStart();
    }

    [System.Serializable]
    public class LevelInfo
    {
        private string name;
        [SerializeField] private string levelName;
        [SerializeField] private GameObject levelGO;
        public GameObject LevelGO { get { return levelGO; } }
        public Golfball Golfball;
        public Golfhole Golfhole;

        public LevelInfo(string name, GameObject GO, Golfhole hole, Golfball ball)
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
