using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections; // Import the DoTween namespace

public class TenQuestionsGame : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text resultText;
    public GameObject elementButtonPrefab;
    public Transform buttonContainer;

    private int currentLevel = 1;
    private int maxLevel = 9;
    private int elementsCount;
    private List<string> elements = new List<string>();
    private string aiChoice;

    public Image displayScreen;
    public Sprite looseImg, winImg;

    // List of elements for each level
    private Dictionary<int, List<string>> levelElements = new Dictionary<int, List<string>>()
    {
        {1, new List<string>{"Red", "Blue", "Green", "Yellow", "Purple", "Orange", "Black", "White", "Pink", "Brown"}},
        {2, new List<string>{"Cat", "Dog", "Elephant", "Tiger", "Lion", "Giraffe", "Monkey", "Horse", "Deer"}},
        {3, new List<string>{"Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune"}},
        {4, new List<string>{"Happy", "Grumpy", "Sleepy", "Bashful", "Sneezy", "Dopey", "Doc"}},
        {5, new List<string>{"Sight", "Hearing", "Smell", "Taste", "Touch", "Intuition"}},
        {6, new List<string>{"Earth", "Water", "Fire", "Air", "Spirit"}},
        {7, new List<string>{"Winter", "Spring", "Summer", "Fall"}},
        {8, new List<string>{"Red", "Blue", "Yellow"}},
        {9, new List<string>{"Night", "Day"}}
    };

    void Start()
    {
        StartLevel();
    }
    public void Home()
    {
        SoundManager.Instance.PlayClickSound();
        SceneManager.LoadScene(1);
    }
    void StartLevel()
    {
        elements.Clear();
        resultText.text = "";
        displayScreen.gameObject.transform.DOScale(0, .5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            displayScreen.gameObject.SetActive(false);
            if (currentLevel > maxLevel)
            {
                EndGame();
                return;
            }

            // Get the elements for the current level
            elements = new List<string>(levelElements[currentLevel]);
            elementsCount = elements.Count;

            // AI chooses a random element
            aiChoice = elements[Random.Range(0, elementsCount)];

            // Display level and create element buttons
            levelText.text = "Level " + currentLevel;
            CreateElementButtons();
        }
        


        

        );
       
    }

    void CreateElementButtons()
    {
        // Clear previous buttons
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        // Create new buttons for each element
        foreach (string element in elements)
        {
            GameObject button = Instantiate(elementButtonPrefab, buttonContainer);
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            buttonText.text = element;
            button.transform.localScale = Vector3.zero; // Set scale to 0 to prepare for animation
            button.GetComponent<Button>().onClick.AddListener(() => OnElementChosen(element));

            // Animate the button using DoTween
            button.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack); // Smooth scaling animation
        }
    }

    void OnElementChosen(string chosenElement)
    {
        SoundManager.Instance.PlayClickSound();
        if (chosenElement == aiChoice)
        {
            // Player loses the game
            displayScreen.sprite = looseImg;
            displayScreen.gameObject.SetActive(true);
            displayScreen.gameObject.transform.DOScale(0.46083f, .5f).SetEase(Ease.Flash);
            resultText.text = "You Lose!"+ "\n\n\n" + "AI chose:" + aiChoice;
            StartCoroutine(DisableScreen());
        }
        else
        {
            displayScreen.sprite = winImg;
            displayScreen.gameObject.SetActive(true);
            displayScreen.gameObject.transform.DOScale(0.46083f, .5f).SetEase(Ease.Flash);

            // Player wins the level
            resultText.text = "You Win!"+ "\n\n\n" + " AI chose: " + aiChoice;
            currentLevel++;
            Invoke("StartLevel", 2f); // Wait for 2 seconds before starting the next level
        }
    }

    public IEnumerator DisableScreen()
    {
        yield return new WaitForSeconds(2f);
        displayScreen.gameObject.transform.DOScale(0, .5f).SetEase(Ease.Linear).OnComplete(()=>
        displayScreen.gameObject.SetActive(false)
        
        );

        aiChoice = elements[Random.Range(0, elementsCount)];
    }
    void EndGame()
    {
        displayScreen.gameObject.SetActive(true);
        displayScreen.gameObject.transform.DOScale(0.46083f, .5f).SetEase(Ease.Flash);
        resultText.text = "Congratulations! You've completed all levels!";
        Invoke("MainMenu",1f);
    }


    public void MainMenu()
    {
        SceneManager.LoadScene(1);
    }
}
