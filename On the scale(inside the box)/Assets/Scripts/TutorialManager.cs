using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static bool TutorialActive = false;
    public static bool SkipOnReload = false;
    private static System.Collections.Generic.HashSet<string> _completedScenes = new();

    [System.Serializable]
    public struct DialogueLine
    {
        public bool isMercury;
        public string text;
    }

    [Header("Mercury")]
    [SerializeField] private GameObject panelMercury;
    [Header("Angelus")]
    [SerializeField] private GameObject panelAngelus;
    [Header("Shared")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float delayBeforeShow = 1f;
    [SerializeField] private DialogueLine[] lines;

    private int _currentLine = 0;
    private bool _started = false;

    private void Start()
    {
        panelMercury.SetActive(false);
        panelAngelus.SetActive(false);
        dialogueText.gameObject.SetActive(false);

        string currentScene = SceneManager.GetActiveScene().name;
        bool skip = _completedScenes.Contains(currentScene) || SkipOnReload;
        SkipOnReload = false; // always reset it here so next scene is never affected

        if (skip) return;

        Invoke(nameof(ShowTutorial), delayBeforeShow);
    }

    private void ShowTutorial()
    {
        TutorialActive = true;
        _started = true;
        dialogueText.gameObject.SetActive(true);
        ShowLine(_currentLine);
    }

    private void Update()
    {
        if (!_started) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            _currentLine++;
            if (_currentLine >= lines.Length)
                EndTutorial();
            else
                ShowLine(_currentLine);
        }
    }

    private void ShowLine(int index)
    {
        DialogueLine line = lines[index];
        dialogueText.text = line.text;

        if (line.isMercury)
        {
            panelMercury.SetActive(true);
            panelAngelus.SetActive(false);
        }
        else
        {
            panelMercury.SetActive(false);
            panelAngelus.SetActive(true);
        }
    }

    private void EndTutorial()
    {
        TutorialActive = false;
        SkipOnReload = false;
        string currentScene = SceneManager.GetActiveScene().name;
        _completedScenes.Add(currentScene);
        panelMercury.SetActive(false);
        panelAngelus.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        _started = false;
    }
}