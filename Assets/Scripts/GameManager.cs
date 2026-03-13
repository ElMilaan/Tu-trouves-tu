using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Create a unique instance of the GameManager to access it wherever we are in the project calling GameManager.Instance
    // This is useful because we don't have to create references everywhere we have to call the manager
    public static GameManager Instance { get; private set; }
    public AudioSource firstCall;
    
    public float gameDuration = 60f;
    
    public TextMeshProUGUI timerText;           
    public TextMeshProUGUI objectiveListText;   
    public GameObject resultPanel;             
    public TextMeshProUGUI resultText;          
    
    public List<string> requiredObjectIDs = new List<string>();
    
    private float timeRemaining;
    private bool gameActive = false;
    private List<string> collectedIDs = new List<string>();
    private bool happeningActive = false;
    
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }
    
    void Start()
    {
        resultPanel.SetActive(false);
        timeRemaining = gameDuration;
        collectedIDs.Clear();
        gameActive = true;
        resultPanel.SetActive(false);
        UpdateObjectiveUI();
        
        PlayIntroWithDelay(2.0f);
    }

    void Update()
    {
        if (!gameActive) return;

        timeRemaining -= Time.deltaTime;
        UpdateTimerUI();

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            EndGame();
        }

        /*if (timeRemaining <= 55.0f && !happeningActive)
        {
            GetComponent<FadeImage>().StartEffect();
            happeningActive = true;
        }*/
    }
    
    public void PlayIntroWithDelay(float delay)
    {
        StartCoroutine(PlayAfterDelay(delay));
    }
    
    IEnumerator PlayAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        firstCall.Play();
    }

    // Triggered by DepositZone when an object enter in it
    public void RegisterCollectedObject(string id)
    {
        if (!gameActive) return;
        if (!collectedIDs.Contains(id))
            collectedIDs.Add(id);
        UpdateObjectiveUI();
        Debug.Log($"Objet collecté : {id}");
    }

    // Triggered by DepositZone when an object is removed from it
    public void UnregisterCollectedObject(string id)
    {
        collectedIDs.Remove(id);
        UpdateObjectiveUI();
    }
    
    private void EndGame()
    {
        gameActive = false;
        ShowResult();
    }

    private void ShowResult()
    {
        resultPanel.SetActive(true);

        List<string> correctItems   = new List<string>();
        List<string> missingItems   = new List<string>();
        List<string> wrongItems     = new List<string>();

        foreach (string id in requiredObjectIDs)
        {
            if (collectedIDs.Contains(id)) correctItems.Add(id);
            else                            missingItems.Add(id);
        }
        foreach (string id in collectedIDs)
        {
            if (!requiredObjectIDs.Contains(id)) wrongItems.Add(id);
        }

        bool success = missingItems.Count == 0 && wrongItems.Count == 0;

        string msg = success ? "<color=green>✅ PARFAIT ! Tous les bons objets !</color>\n"
                             : "<color=red>❌ Raté !</color>\n";

        if (correctItems.Count > 0)
            msg += $"\n<color=green>Bons objets ({correctItems.Count}) :</color> {string.Join(", ", correctItems)}";
        if (missingItems.Count > 0)
            msg += $"\n<color=yellow>Manquants ({missingItems.Count}) :</color> {string.Join(", ", missingItems)}";
        if (wrongItems.Count > 0)
            msg += $"\n<color=red>Mauvais objets ({wrongItems.Count}) :</color> {string.Join(", ", wrongItems)}";

        resultText.text = msg;
    }
    
    private void UpdateTimerUI()
    {
        int seconds = Mathf.CeilToInt(timeRemaining);
        timerText.text = $"{seconds}s";
        timerText.color = seconds <= 10 ? Color.red : Color.white;
    }

    private void UpdateObjectiveUI()
    {
        string list = "<b>Objets à trouver :</b>\n";
        foreach (string id in requiredObjectIDs)
        {
            bool found = collectedIDs.Contains(id);
            list += found ? $"<color=green>✅ {id}</color>\n"
                          : $"<color=white>◻ {id}</color>\n";
        }
        objectiveListText.text = list;
    }

    public void launchHappening()
    {
        
    }
    
    public void OnRestartButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}