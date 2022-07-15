using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CategoryScreenHandler : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject MainMenuScreen;
    public Category[] categories;
    public GameObject[] categoryButtons;

    void Awake()
    {
        SetCategoryButtons();
    }

    public void SetCategoryButtons()
    {
        categories = APIHelper.ApiFetchCategories().trivia_categories;
        categoryButtons = new GameObject[categories.Length];

        for (int i = 0; i < categories.Length; i++)
        {
            categoryButtons[i] = Instantiate(buttonPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0), gameObject.transform);
            
            categoryButtons[i].AddComponent<LayoutElement>();
            categoryButtons[i].GetComponentInChildren<Text>().text = categories[i].name;

            Category category = categories[i];  // We need it to be independent from index to use it as a parameter

            Button button = categoryButtons[i].GetComponent<Button>();
            button.onClick.AddListener(delegate { SelectCategory(category); });

        }
    }

    public void SelectCategory(Category category)
    {
        StaticGameInfo.selectedCategory = category;
        Debug.Log("Selected Category is: " + StaticGameInfo.selectedCategory.name);

        StaticGameInfo.questionRequest = APIHelper.ApiFetchQuestionsByCategory(category);
        if (StaticGameInfo.questionRequest.response_code == 0)
        {
            Debug.Log(StaticGameInfo.questionRequest.results[0].question);
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.Log("response_code = " + StaticGameInfo.questionRequest.response_code);
            //Give warning or smthng
        }
    }

    public void BackToMainMenu()
    {
        gameObject.SetActive(false);
        MainMenuScreen.gameObject.SetActive(true);
    }

}
