using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText; // Reference to TextMeshProUGUI element
    [SerializeField] private string[] dialogueLines; // Array to store dialogue strings
    [SerializeField] private GameObject questionPanel; // Reference to the question panel GameObject
    [SerializeField] private TextMeshProUGUI questionText; // Reference to TextMeshProUGUI for question
    [SerializeField] private TextMeshProUGUI[] answerButtons; // Array of TextMeshProUGUIs for answer buttons

    private int currentLineIndex = 0; // Keeps track of the current dialogue line
    private bool isDialogueActive = true; // Flag to track if dialogue is ongoing
    private string correctAnswer; // Stores the correct answer for the question

    void Start()
    {
        if (dialogueText == null)
        {
            Debug.LogError("DialogueManager: TextMeshProUGUI element not assigned!");
            return;
        }

        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.LogError("DialogueManager: No dialogue lines provided!");
            return;
        }

        questionPanel.SetActive(true); // Initially hide the question panel
        DisplayNextLine(); // Show the first line of dialogue on Start
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space) && currentLineIndex < dialogueLines.Length) // Check for space key press during dialogue
        {
            DisplayNextLine();
        }
    }

    public void DisplayNextLine()
    {
        if (currentLineIndex == dialogueLines.Length)
        {
            // Dialogue finished, show question panel
            questionPanel.SetActive(true);
            isDialogueActive = false;
            SetupQuestionAndAnswers(); // Set up question and answer options
            return;
        }

        dialogueText.text = dialogueLines[currentLineIndex];
        currentLineIndex++;
    }

    public void SetupQuestionAndAnswers()
    {
        // Assuming you have a separate array or data structure to store questions and answers
        questionText.text = "Question: Whats 1 + 1 ? "; // Replace with your question retrieval logic

        // Set answer options on buttons (assuming 4 answer buttons)
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].text = "2"; // Replace with your answer retrieval logic
        }

        // Set the correct answer (assuming it's stored in a separate variable in your data structure)
        correctAnswer = "2"; // Replace with your logic
    }

    public void OnAnswerSelected(int answerIndex) // This function will be called by the button's onClick event
    {
        string selectedAnswer = answerButtons[answerIndex].text;

        if (selectedAnswer == correctAnswer)
        {
            // Destroy enemy (assuming the enemy script is attached to this game object)
            Destroy(gameObject);
        }
        else
        {
            // Handle incorrect answer (e.g., display message, retry question, load scene)
            // Optionally, reload the scene (assuming you have a scene name stored)
            // SceneManager.LoadScene(/* Your scene name */);
        }
    }
}
