using UnityEngine;
using System.Collections;

public class Scorekeeper : MonoBehaviour {

    public GameObject goBeerText;
    public GameObject goSodaText;
    public GameObject goPopmeterText;
    public GameObject goTimer;
    public GameObject goStreak;
    public static int popularityMeter = 50;
    public static int currentStreak = 0;
    public static int comboMultiplier = 1;
    public static int penaltyDidntGetRefreshment = 2;
    public static int penaltyAdultGetsSoda = 1;
    public static int penaltyChildGetsBeer = 6;
    public static int penaltyHitPartyer = 1;
    public static int rewardAdultGetsBeer = 2;
    public static int rewardChildGetsSoda = 2;
    public static Color addColor = new Color(0.992f, 0.992f, 0.286f, 1.0f);
    public static Color minusColor = new Color();

    private float seconds;
    private int highestPop;
    private int longestStreak;
    private int lastStreak;
    private int[] streakToComboGates = new int[6];

    // GUI Stuff
    private PlayerInput scriptInput;
    private TextMesh textBeer;
    private TextMesh textSoda;
    private TextMesh textPopMeter;
    private TextMesh textTimer;
    private TextMesh textStreak;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Application.loadedLevelName == "Start")
            GameObject.Destroy(gameObject);
        if (Application.loadedLevelName == "Level1")
        {
            popularityMeter = 50;
            currentStreak = 0;
            comboMultiplier = 1;
        }
        StartCoroutine(DisplayEndingScores());
    }

    // Use this for initialization
    void Start()
    {
        minusColor = Color.red;

        textBeer = goBeerText.GetComponent<TextMesh>();
        textSoda = goSodaText.GetComponent<TextMesh>();
        textPopMeter = goPopmeterText.GetComponent<TextMesh>();
        textTimer = goTimer.GetComponent<TextMesh>();
        textStreak = goStreak.GetComponent<TextMesh>();


        scriptInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        highestPop = popularityMeter;
        lastStreak = currentStreak;

        for (int i = 0; i < streakToComboGates.Length; i++)
        {
            if (i == 0)
                streakToComboGates[i] = 5;
            else
                streakToComboGates[i] = i * 10;
        }
    }

    void Update()
    {
        if (Application.loadedLevelName == "Start")
        {
            GameObject.Destroy(this.gameObject);
        }
        if (Application.loadedLevelName == "Level1")
        {
            RefreshScoresAndMultiplier();
            RefreshGUI();
            EndGameNoQuench();
        }

    }

    void EndGameNoQuench()
    {
        if (popularityMeter <= 0)
        {
            Application.LoadLevel("End");
        }
    }

    void RefreshScoresAndMultiplier()
    {
        if (popularityMeter > highestPop)
        {
            highestPop = popularityMeter;
        }
        if (currentStreak > longestStreak)
        {
            longestStreak = currentStreak;
        }
        if (currentStreak != lastStreak)
        {
            // If we've passed the threshold for a combo, find our combo level/multiplier and assign it
            if (currentStreak >= streakToComboGates[0])
            {
                int comboLevel = StreakToMultiplier();
                print("Combo Level = " + comboLevel.ToString() + "!!!");
                comboMultiplier = Mathf.FloorToInt(comboLevel / 2) + 1;  // Level 1 will be 1, 2 and 3 will be 2, 4 and 5 will be 4, etc...
            }

            // Else if the last streak we had was a combo streak, but the current streak is not, we need to reassign combo multiplier to 1
            else if (lastStreak >= streakToComboGates[0])
            {
                comboMultiplier = 1;
            }
        }
        lastStreak = currentStreak;
    }

    int StreakToMultiplier()
    {
        for (int i = 0; i < streakToComboGates.Length; i++)
        {
            if (i < streakToComboGates.Length - 1)
            {
                if (currentStreak >= streakToComboGates[i] && currentStreak < streakToComboGates[i + 1])
                {
                    return i + 1;  // Return the array number plus one since combos won't start at level 0
                }
            }
            else
                return i;
        }

        return 0;

    }

    void RefreshGUI()
    {
        textPopMeter.text = "Popularity: " + popularityMeter.ToString();
        textStreak.text = "Streak: " + currentStreak.ToString();
        textBeer.text = scriptInput.ammoBeer.ToString();
        textSoda.text = scriptInput.ammoSoda.ToString();

        //int minutes = Mathf.FloorToInt(Time.timeSinceLevelLoad/60.0f);
        //float seconds = Time.timeSinceLevelLoad % 60.0f;
        seconds = Time.timeSinceLevelLoad;

        //string clockTime = string.Format("{0:0}:{1:0#.0}", minutes, seconds);

        textTimer.text = "Time " + seconds.ToString("F2");
    }

    IEnumerator DisplayEndingScores()
    {
        while (Application.loadedLevelName != "End")
        {
            yield return null;
        }

        TextMesh finalSeconds = GameObject.FindWithTag("Seconds").GetComponent<TextMesh>();
        TextMesh popularity = GameObject.FindWithTag("Popularity").GetComponent<TextMesh>();
        TextMesh streak = GameObject.FindWithTag("Streak").GetComponent<TextMesh>();

        finalSeconds.text = seconds.ToString("F2") + " seconds!";
        popularity.text = highestPop.ToString();
        streak.text = longestStreak.ToString();
    }
}
