using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Instance;

    public CanvasGroup smeltingTutorial;
    public CanvasGroup forgingTutorial;
    public CanvasGroup quenchingTutorial;

    public bool isTutorialShow;

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowTutorial(CanvasGroup tutorial, bool isFirstDay)
    {
        if (isFirstDay)
        {
            tutorial.alpha = 1f;
            tutorial.interactable = true;
            tutorial.blocksRaycasts = true;
        }
        else
        {
            tutorial.alpha = 0f;
            tutorial.interactable = false;
            tutorial.blocksRaycasts = false;
        }

        isTutorialShow = true;


    }

    public void CloseTutorial(CanvasGroup tutorial)
    {
        tutorial.alpha = 0f;
        tutorial.interactable = false;
        tutorial.blocksRaycasts = false;

        isTutorialShow = false;
    }
}
