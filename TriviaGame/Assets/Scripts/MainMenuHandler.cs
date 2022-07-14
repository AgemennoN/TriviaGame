using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}
