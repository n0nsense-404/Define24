using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;
using System;

[System.Serializable]
public class TriviaQuestion
{
    public string category;
    public string type;
    public string difficulty;
    public string question;
    public string correct_answer;
    public List<string> incorrect_answers;
}

[System.Serializable]
public class TriviaData
{
    public int response_code;
    public List<TriviaQuestion> results;
}

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private string[] dialogueLines;
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private GameObject Enemy; // Enemy GameObject
    [SerializeField] private GameObject textField;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private GameObject Con;
    [SerializeField] private TextMeshProUGUI[] answerButtons;

    private int currentLineIndex = 0;
    private bool isDialogueActive = true;
    private string correctAnswer;

    // Player and enemy health variables
    private int playerHealth = 3;
    private int enemyHealth = 3;

    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private TextMeshProUGUI enemyHealthText;

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

        questionPanel.SetActive(false);
        Con.SetActive(false);

        playerHealthText.text = $"{playerHealth}";
        enemyHealthText.text = $"{enemyHealth}";

        DisplayNextLine();
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space) && currentLineIndex <= dialogueLines.Length)
        {
            DisplayNextLine();
        }
    }

    public void DisplayNextLine()
    {
        if (currentLineIndex == dialogueLines.Length)
        {
            questionPanel.SetActive(true);
            textField.SetActive(false);
            isDialogueActive = false;
            SetupQuestionAndAnswers();
            return;
        }

        dialogueText.text = dialogueLines[currentLineIndex];
        currentLineIndex++;
    }

    public async void SetupQuestionAndAnswers()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://opentdb.com/api.php?amount=1"))
        {
            // Send the request and wait for the results
            var operation = www.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Delay(100);
            }

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Parse the response
                var data = JsonUtility.FromJson<TriviaData>(www.downloadHandler.text);
                var results = data.results[0];

                questionText.text = results.question;
                correctAnswer = results.correct_answer;

                // Combine correct and incorrect answers
                var incorrectAnswers = results.incorrect_answers;
                incorrectAnswers.Add(correctAnswer);
                incorrectAnswers = incorrectAnswers.OrderBy(a => Guid.NewGuid()).ToList(); // Randomize order

                // Assign answers to buttons
                for (int i = 0; i < answerButtons.Length; i++)
                {
                    answerButtons[i].text = incorrectAnswers[i];
                }
            }
        }
    }

    public void OnAnswerSelected(int answerIndex)
    {
        string selectedAnswer = answerButtons[answerIndex].text;

        if (selectedAnswer == correctAnswer)
        {
            enemyHealth -= 1; // Enemy loses 3 health on correct answer
            enemyHealthText.text = $"{enemyHealth}";

            if (enemyHealth <= 0)
            {
                // Enemy defeated
                questionPanel.SetActive(false);
                Destroy(Enemy);
                Con.SetActive(true);
                LoadNextSceneWithDelay();
                return;
            }

            // Enemy still alive, display next question
            SetupQuestionAndAnswers();
        }
        else
        {
            playerHealth--; // Player loses 1 health on wrong answer
            playerHealthText.text = $"{playerHealth}";

            if (playerHealth <= 0)
            {
                // Player defeated
                SceneManager.UnloadSceneAsync("Fight");
                Time.timeScale = 1f;
                SceneManager.LoadScene("Main"); // Load main scene from the start
                return;
            }

            // Player still alive, display next question
            SetupQuestionAndAnswers();
        }
    }

    async void LoadNextSceneWithDelay()
    {
        await Task.Delay(3000);
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("Fight");
    }
}
