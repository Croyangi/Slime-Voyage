using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class BootLoader_ResultsScreen : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    [SerializeField] private string _areaId;

    [SerializeField] private TextMeshProUGUI tmp_rank;
    [SerializeField] private float currentScore;
    [SerializeField] private float totalScore;

    [Serializable]
    public class RankScore
    {
        public float minScore;
        public string rankText;
        public Color color;
    }

    [SerializeField] private List<RankScore> rankScores;

    [SerializeField] private TextMeshProUGUI tmp_areaId;
    [SerializeField] private TextMeshProUGUI tmp_version;

    [SerializeField] private TextMeshProUGUI tmp_checkpointsReached;
    [SerializeField] private TextMeshProUGUI tmp_newLocationsDiscovered;

    [SerializeField] private GameObject speedrun;
    [SerializeField] private TextMeshProUGUI tmp_speedrunTime;
    [SerializeField] private TextMeshProUGUI tmp_speedrunRecord;

    [SerializeField] private AudioClip sfx_resultsScreenDrumroll;
    [SerializeField] private AudioClip music_boombox;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_loadingScreen;
    [SerializeField] private string scene_activeScene;

    public TimeSpan timeElapsed { get; private set; }

    private void Awake()
    {
        StartCoroutine(LoadLoadingScreen());
        StartCoroutine(RankReveal());

        SetVersion();
    }

    private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene_activeScene));
    }

    // VFX for dramatic rank reveal
    private IEnumerator RankReveal()
    {
        yield return new WaitForSeconds(1.5f);
        Manager_SFXPlayer.instance.PlaySFXClip(sfx_resultsScreenDrumroll, transform, 1f, mixerGroup: Manager_AudioMixer.instance.mixer_sfx);

        StartCoroutine(GenerateRandomRankVFX());

        yield return new WaitForSeconds(2.20f);

        Manager_SFXPlayer.instance.PlaySFXClip(music_boombox, transform, 0.7f, isLooping: true, mixerGroup: Manager_AudioMixer.instance.mixer_music, isUnaffectedByTime: true);

        Vector3 scale = tmp_rank.gameObject.transform.localScale;
        LeanTween.scale(tmp_rank.gameObject, scale + (Vector3.one * 0.8f), 0.2f);
        LeanTween.scale(tmp_rank.gameObject, scale, 0.5f).setEaseOutBack().setDelay(0.2f);

        // Set rank for last
        StopAllCoroutines();
        SetRank(currentScore, totalScore);
    }


    // Randomly generates letters in an alphabet
    private IEnumerator GenerateRandomRankVFX()
    {
        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        int index = Random.Range(0, alphabet.Length - 1);

        string randomLetter = alphabet.Substring(index, 1);
        tmp_rank.text = randomLetter;

        yield return new WaitForFixedUpdate();
        StartCoroutine(GenerateRandomRankVFX());
    }

    private IEnumerator LoadLoadingScreen()
    {
        // Transition screen
        if (Manager_LoadingScreen.instance != null)
        {
            Manager_LoadingScreen.instance.OpenLoadingScreen();
        }
        else
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene_loadingScreen, LoadSceneMode.Additive);

            // Wait until the asynchronous scene loading is complete
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            Debug.Log("Finished");
            Manager_LoadingScreen.instance.OpenLoadingScreen();
        }
    }

    // Set project version
    private void SetVersion()
    {
        tmp_version.text = "v" + Application.version;
    }

    // Sets text
    private void SetSpeedrunTimes(float current = 0f, float record = 0f)
    {
        //speedrun.SetActive(true);

        if (current > 0f)
        {
            TimeSpan speedrunTime = TimeSpan.FromSeconds(current);
            string time = speedrunTime.Minutes.ToString("00") + ":" +
                                  speedrunTime.Seconds.ToString("00") + "." +
                                  speedrunTime.Milliseconds.ToString("000");
            tmp_speedrunTime.text = "Current Time: " + time;
        }

        if (record > 0f)
        {
            TimeSpan recordTime = TimeSpan.FromSeconds(record);
            string time = recordTime.Minutes.ToString("00") + ":" +
                                  recordTime.Seconds.ToString("00") + "." +
                                  recordTime.Milliseconds.ToString("000");
            tmp_speedrunRecord.text = "Record Time: " + time;
        }
    }

    // Sets text
    private void SetCheckpointsReached(int amount, int totalAmount)
    {
        if (totalAmount > 0) 
        {
            tmp_checkpointsReached.text = "Checkpoints Reached: " + amount + "/" + totalAmount;

            currentScore += amount;
            totalScore += totalAmount;
        }
    }

    // Sets text
    private void SetLocationsDiscovered(int amount, int totalAmount)
    {
        if (totalAmount > 0)
        {
            tmp_newLocationsDiscovered.text = "Locations Discovered: " + amount + "/" + totalAmount;

            currentScore += amount;
            totalScore += totalAmount;
        }
    }

    // Sets rank after tallying scores
    private void SetRank(float currentScore, float totalScore)
    {
        string rank;
        Color color;

        float score = (currentScore / totalScore) * 100f;
        Debug.Log(score);

        // Score VFX
        if (score > 0f)
        {
            RankScore rankScore = CalculateRank(score);
            rank = rankScore.rankText;
            color = rankScore.color;
        } else
        {
            RankScore rankScore = CalculateRank(-1);
            rank = rankScore.rankText;
            color = rankScore.color;
        }

        tmp_rank.text = rank;
        tmp_rank.color = color;
    }

    private RankScore CalculateRank(float score)
    {
        foreach (RankScore rankScore in rankScores)
        {
            if (score >= rankScore.minScore)
            {
                return rankScore;
            }
        }
        return null;
    }

    private void SetAreaId(string id)
    {
        tmp_areaId.text = "ID: " + id;
    }

    public void LoadData(GameData data)
    {
        // Get area id
        _areaId = data.resultsScreenId;
        SetAreaId(_areaId);

        // Speedrun Times
        float recordTime;
        data.recordSpeedrunTimes.TryGetValue(_areaId, out recordTime);
        SetSpeedrunTimes(data.currentSpeedrunTime, recordTime);
        //

        // Checkpoints
        // Search up area data based on id
        AreaSet areaSetCheckpoints = data.SearchAreaWithId(_areaId);
        SerializableDictionary<string, bool> checkpointsReached = areaSetCheckpoints.checkpointsReached;

        int checkpointCount = 0;
        foreach (bool value in checkpointsReached.Values) 
        { 
            if (value)
            {
                checkpointCount++;
            }
        }
        SetCheckpointsReached(checkpointCount, checkpointsReached.Count);
        //

        // Areas Discovered
        // Search up area data based on id
        AreaSet areaSetLocations = data.SearchAreaWithId(_areaId);
        SerializableDictionary<string, bool> areasDiscovered = areaSetLocations.locationsDiscovered;

        int locationCount = 0;
        foreach (bool value in areasDiscovered.Values)
        {
            if (value)
            {
                locationCount++;
            }
        }
        SetLocationsDiscovered(locationCount, areasDiscovered.Count);
        //

    }

    public void SaveData(ref GameData data)
    {
        
    }
}
