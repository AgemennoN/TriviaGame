using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject categoryScreen;
    [SerializeField] private Button selectedCategoryBtn;
    public int totalQuestionNumber;
    private void Start() 
    {
        //StaticGameInfo.totalQuestionNumber could be adjusted in option menu in later versions of the game easly
        totalQuestionNumber = 10;
        StaticGameInfo.totalQuestionNumber = totalQuestionNumber;

        // If there is a selected category, enable its button on main menu
        if (StaticGameInfo.selectedCategory != null)
        {
            selectedCategoryBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Play with Category: " + StaticGameInfo.selectedCategory.name;
            selectedCategoryBtn.onClick.RemoveAllListeners();
            CategoryScreenHandler CSH = categoryScreen.GetComponent<CategoryScreenHandler>();
            selectedCategoryBtn.onClick.AddListener(delegate { CSH.SelectCategory(StaticGameInfo.selectedCategory); });
            selectedCategoryBtn.gameObject.SetActive(true);
        }
    }

    public void EnableCategoryScreen()
    {
        categoryScreen.SetActive(true);
        gameObject.SetActive(false);
    }

    public void RandomQuestions()   // Starts the game with questions from random categories
    {
        StaticGameInfo.questionRequest = APIHelper.ApiFetchRandomQuestions(StaticGameInfo.totalQuestionNumber);
        if (StaticGameInfo.questionRequest.response_code == 0)
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.Log("response_code = " + StaticGameInfo.questionRequest.response_code);
        }
    }

}
