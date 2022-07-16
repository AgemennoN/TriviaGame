using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject categoryScreen;
    [SerializeField] private Button selectedCategoryBtn;

    private void OnEnable()
    {
        CategoryScreenHandler CSH = categoryScreen.GetComponent<CategoryScreenHandler>();
        if (StaticGameInfo.selectedCategory != null)
        {
            selectedCategoryBtn.GetComponentInChildren<Text>().text = StaticGameInfo.selectedCategory.name;
            selectedCategoryBtn.onClick.RemoveAllListeners();
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
            Debug.Log(StaticGameInfo.questionRequest.results[0].question);

            SceneManager.LoadScene("GameScene");

        }
        else
        {
            Debug.Log("response_code = " + StaticGameInfo.questionRequest.response_code);
            //Give warning or smthng
        }
    }

}
