using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject categoryScreen;
    [SerializeField] private Button selectedCategoryBtn;
    
    private void OnEnable() // If there is a selected category enable its button on main menu
    {
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

    public void RandomQuestions()
    {
        StaticGameInfo.questionRequest = APIHelper.ApiFetchRandomQuestions();

        if (StaticGameInfo.questionRequest.response_code == 0)
        {
            StaticGameInfo.totalQuestionNumber = StaticGameInfo.questionRequest.results.Length;
            SceneManager.LoadScene("GameScene");

        }
        else
        {
            Debug.Log("response_code = " + StaticGameInfo.questionRequest.response_code);
            //Give warning or smthng
        }
    }

}
