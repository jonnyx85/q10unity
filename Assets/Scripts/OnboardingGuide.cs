using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement; // Required for scene management

public class OnboardingGuide : MonoBehaviour
{
    public TMP_Text guideText; // Text object to display the guide
    private string[] guideMessages;
    private int currentMessageIndex = 0;
    public float typingSpeed = 0.05f; // Speed of the typing effect
    public float waitTimeBeforeNextScene = 3f; // Time to wait before switching scenes after onboarding
  

    public AudioSource typingAudioSource; // Reference to the AudioSource that will play the typing sound
    public AudioClip typingSoundClip; // The sound to play when typing each letter


    public Slider progressBar; // Reference to the UI Slider for progress
    public TMP_Text progressText; // Reference to the text that shows the progress percentage
   
    public float fakeLoadingTime = 5f; // Time to simulate the loading process
    private float loadingProgress = 0f; // Internal progress tracker
    public GameObject splashScreen,OwlGuide;
    void Start()
    {
        // Start the fake loading process
        StartCoroutine(FakeLoading());
    }

    IEnumerator FakeLoading()
    {
        while (loadingProgress < 1f)
        {
            // Increment progress over time
            loadingProgress += Time.deltaTime / fakeLoadingTime;

            // Update the slider value
            progressBar.value = loadingProgress;

            // Update the text to show the progress in percentage
            progressText.text = Mathf.FloorToInt(loadingProgress * 100) + "%";

            // Wait for the next frame
            yield return null;
        }
        Startgame();
       
    }


    void Startgame()
    {
        // Check if onboarding should be shown
        if (PlayerPrefs.HasKey("OnboardingShown"))
        {
            // If onboarding was shown before, load the next scene directly
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
        else
        {
            splashScreen.SetActive(false);

            OwlGuide.gameObject.transform.DOScale(1f, .5f).SetEase(Ease.Linear).OnComplete(() =>
            {

                // Otherwise, show the onboarding and mark it as shown
                PlayerPrefs.SetInt("OnboardingShown", 1);
                PlayerPrefs.Save();

                // Initialize guide messages
                guideMessages = new string[]
                {
                "Welcome to the 10 Questions Game! I'm Ollie the Owl, and I'll be your guide.",
                "The goal is simple: guess an element that I didn’t pick!",
                "Each level has fewer elements. Can you outsmart me and reach the final level?",
                "At the start of each level, you’ll see a list of elements on the screen.",
                "I'll secretly choose one element. Your job is to pick a different one.",
                "If you choose the same element I picked, you lose that round.",
                "But if you pick a different element, you move to the next level!",
                "Each level gets harder, with fewer elements to choose from.",
                "Reach the final level with only 2 elements and make your last choice to win the game!",
                "Good luck, and remember: Think wisely, choose carefully!"
                };

                // Start the onboarding guide
                DisplayNextMessage();
            });
            
        }
    }

    void DisplayNextMessage()
    {
        if (currentMessageIndex < guideMessages.Length)
        {
            guideText.text = ""; // Clear the text
            StartCoroutine(TypeText(guideMessages[currentMessageIndex])); // Type out the current message
        }
        else
        {
            // Onboarding complete, move to the next scene after a delay
            guideText.text = "Let's start the game!";
            StartCoroutine(MoveToNextScene());
        }
    }

    IEnumerator TypeText(string message)
    {
        guideText.text = ""; // Clear the text to start typing

        foreach (char letter in message.ToCharArray())
        {
            guideText.text += letter; // Add one letter at a time

            // Play typing sound
            PlayTypingSound();

            yield return new WaitForSeconds(typingSpeed); // Wait for typingSpeed before typing the next letter
        }

        // Wait for a few seconds before moving to the next message
        yield return new WaitForSeconds(2f);
        currentMessageIndex++;
        DisplayNextMessage(); // Display the next message
    }

    void PlayTypingSound()
    {
        // Check if the audio source and clip are assigned
        if (typingAudioSource != null && typingSoundClip != null)
        {
            typingAudioSource.PlayOneShot(typingSoundClip); // Play the typing sound effect
        }
    }

    IEnumerator MoveToNextScene()
    {
        // Wait for the designated time before switching scenes
        yield return new WaitForSeconds(waitTimeBeforeNextScene);

        // Load the next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
