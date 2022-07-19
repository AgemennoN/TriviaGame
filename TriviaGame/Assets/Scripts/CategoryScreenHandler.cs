using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CategoryScreenHandler : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject MainMenuScreen;
    [SerializeField] private GameObject ScrollViewContent;
    [SerializeField] private TMP_InputField InputCategory;
    private Category[] categories;
    private GameObject[] categoryButtons;   // Button game objects disabled and enabled according to the searched word
    private List<string> categoryNames;     // List that only holds categorie names as string to use in searchbar

    void Awake()
    {
        SetCategoryButtons();
        EnableSearchedCategories();
    }

    private void SetCategoryButtons()
    {
        categories = APIHelper.ApiFetchCategories().trivia_categories;
        categoryButtons = new GameObject[categories.Length];
        categoryNames = new List<string>();

        for (int i = 0; i < categories.Length; i++)
        {
            // creates new buttons, name them, add listener to them.
            categoryButtons[i] = Instantiate(buttonPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0), ScrollViewContent.transform);
            categoryButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = categories[i].name;
            categoryNames.Add(categories[i].name.ToLower());

            Category category = categories[i];  // To use the index with delegate
            Button button = categoryButtons[i].GetComponent<Button>();
            button.onClick.AddListener(delegate { SelectCategory(category); });

        }
    }

    public void EnableSearchedCategories()  // This function is called when the input changes
    {
        if (InputCategory.text.Trim() == "" || InputCategory.text == null)
        // If searchbar is Empty, enable all the buttons
        {
            for (int i = 0; i < categoryNames.Count; i++)
            {
                categoryButtons[i].SetActive(true);
            }
        }
        else // Only enable the buttons that contains the Input, disable others.
        {
            for (int i = 0; i < categoryNames.Count; i++)
            {
                if (categoryNames[i].Contains(InputCategory.text.ToLower().Trim()))
                    categoryButtons[i].SetActive(true);
                else
                    categoryButtons[i].SetActive(false);
            }
        }
    }

    public void SelectCategory(Category category)   
    {
        // Stores the selected category in a static class, so that it will persist along the scenes
        StaticGameInfo.selectedCategory = category;

        // Fetches the questions with the selected category from API 
        StaticGameInfo.questionRequest = APIHelper.ApiFetchQuestionsByCategory(category, StaticGameInfo.totalQuestionNumber);
        if (StaticGameInfo.questionRequest.response_code == 0) // response_code == 0 means fetch is successful
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.Log("response_code = " + StaticGameInfo.questionRequest.response_code);
        }
    }

    public void BackToMainMenu()
    {
        ClearSearchbar();
        gameObject.SetActive(false);
        MainMenuScreen.gameObject.SetActive(true);
    }

    public void ClearSearchbar() //Cleans the Searchbar Input
    {
        InputCategory.text = "";
    }

}
