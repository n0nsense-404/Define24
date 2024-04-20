using UnityEngine;
using TMPro;

using System.Threading.Tasks;

using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText; 
    [SerializeField] private string[] dialogueLines; 
    [SerializeField] private GameObject questionPanel; 

    [SerializeField] private GameObject Enemy; 
    
    [SerializeField] private GameObject textField; 
       [SerializeField] private TextMeshProUGUI questionText;
    
    [SerializeField] private GameObject Con; 
    [SerializeField] private TextMeshProUGUI[] answerButtons; 

    private int currentLineIndex = 0; 
    private bool isDialogueActive = true;
    private string correctAnswer; 
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

    public void SetupQuestionAndAnswers()
    {
 
        questionText.text = "Question: Whats 1 + 1 ? "; 


        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].text = $"{i}"; 
        }


        correctAnswer = "2"; 
    }
    [SerializeField] private string NextScene;
    public void OnAnswerSelected(int answerIndex) 
    {
        string selectedAnswer = answerButtons[answerIndex].text;

    if (selectedAnswer == correctAnswer){
 
    questionPanel.SetActive(false);
    Destroy(Enemy);
    Con.SetActive(true);
    LoadNextSceneWithDelay();
    }

    async void LoadNextSceneWithDelay(){
    await Task.Delay(3000); 

    SceneManager.LoadScene(NextScene);
    }
}
}
