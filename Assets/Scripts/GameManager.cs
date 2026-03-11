using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// GameManager - Gère le timer, la liste d'objets à collecter et la validation finale.
/// Placer ce script sur un GameObject vide "GameManager" dans la scène.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Paramètres du jeu")]
    [Tooltip("Durée du jeu en secondes (60 = 1 minute)")]
    public float gameDuration = 60f;

    [Header("UI Références")]
    public TextMeshProUGUI timerText;           // Texte du compteur
    public TextMeshProUGUI objectiveListText;   // Liste des objets à trouver
    public GameObject resultPanel;             // Panel résultat (fin de jeu)
    public TextMeshProUGUI resultText;          // Texte du résultat

    [Header("Objets à collecter")]
    [Tooltip("Remplir avec les IDs des objets que le joueur doit trouver")]
    public List<string> requiredObjectIDs = new List<string>();

    // --- État interne ---
    private float timeRemaining;
    private bool gameActive = false;
    private List<string> collectedIDs = new List<string>();

    // ---------------------------------------------------------------
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        resultPanel.SetActive(false);
        StartGame();
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
    }

    // ---------------------------------------------------------------
    /// <summary>Démarre ou redémarre une partie.</summary>
    public void StartGame()
    {
        timeRemaining = gameDuration;
        collectedIDs.Clear();
        gameActive = true;
        resultPanel.SetActive(false);
        UpdateObjectiveUI();
    }

    /// <summary>Appelé par SuitcaseZone quand un objet est déposé.</summary>
    public void RegisterCollectedObject(string id)
    {
        if (!gameActive) return;
        if (!collectedIDs.Contains(id))
            collectedIDs.Add(id);

        UpdateObjectiveUI();
        Debug.Log($"[GameManager] Objet collecté : {id}");
    }

    /// <summary>Appelé par SuitcaseZone si un objet est retiré de la valise.</summary>
    public void UnregisterCollectedObject(string id)
    {
        collectedIDs.Remove(id);
        UpdateObjectiveUI();
    }

    // ---------------------------------------------------------------
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

    // ---------------------------------------------------------------
    private void UpdateTimerUI()
    {
        int seconds = Mathf.CeilToInt(timeRemaining);
        timerText.text = $"⏱ {seconds}s";
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

    // ---------------------------------------------------------------
    /// <summary>Bouton "Rejouer" dans le panel résultat.</summary>
    public void OnRestartButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}