using UnityEngine;
using System.Net;
using System.IO;
using System.Web;

public static class APIHelper
{
    public static FetchedCategories ApiFetchCategories()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://opentdb.com/api_category.php");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();

        FetchedCategories fetchedCategories = JsonUtility.FromJson<FetchedCategories>(json);
        foreach (Category category in fetchedCategories.trivia_categories)
        {
            category.name = CategoryNameFixer(category.name);
        }
        return fetchedCategories;
    }

    public static FetchedQuestions ApiFetchRandomQuestions()
    {
        string url = "https://opentdb.com/api.php?amount=10&type=multiple";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();

        FetchedQuestions fetchedQuestions = JsonUtility.FromJson<FetchedQuestions>(json);
        foreach (Question question in fetchedQuestions.results)
        {
            question.category = CategoryNameFixer(question.category);
            question.question = HttpUtility.HtmlDecode(question.question);
            question.correct_answer = HttpUtility.HtmlDecode(question.correct_answer);
            question.incorrect_answers[0] = HttpUtility.HtmlDecode(question.incorrect_answers[0]);
            question.incorrect_answers[1] = HttpUtility.HtmlDecode(question.incorrect_answers[1]);
            question.incorrect_answers[2] = HttpUtility.HtmlDecode(question.incorrect_answers[2]);
        }
        return fetchedQuestions;
    }

    public static FetchedQuestions ApiFetchQuestionsByCategory(Category category)
    {
        string url = "https://opentdb.com/api.php?amount=10&category=" + category.id + "&type=multiple";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();

        FetchedQuestions fetchedQuestions = JsonUtility.FromJson<FetchedQuestions>(json);
        foreach (Question question in fetchedQuestions.results)
        {
            question.category = CategoryNameFixer(question.category);
            question.question = HttpUtility.HtmlDecode(question.question);
            question.correct_answer = HttpUtility.HtmlDecode(question.correct_answer);
            question.incorrect_answers[0] = HttpUtility.HtmlDecode(question.incorrect_answers[0]);
            question.incorrect_answers[1] = HttpUtility.HtmlDecode(question.incorrect_answers[1]);
            question.incorrect_answers[2] = HttpUtility.HtmlDecode(question.incorrect_answers[2]);
        }
        return fetchedQuestions;
    }

    public static string CategoryNameFixer(string StringToBeFixed)
    {
        string[] DeleteUntil = new string[] {": " };
        string[] FixedString = StringToBeFixed.Split(DeleteUntil, System.StringSplitOptions.None );

        if (FixedString.Length == 1)    // Input String doesn't needed to be fixed
            return StringToBeFixed;
        else                            // Return the string after ':'
            return FixedString[1];
    }

}


[System.Serializable]
public class FetchedCategories
{
    public Category[] trivia_categories;
}

[System.Serializable]
public class Category
{
    public int id;
    public string name;
}




[System.Serializable]
public class FetchedQuestions
{
    public int response_code;
    public Question[] results;
}

[System.Serializable]
public class Question
{
    public string category;
    public string type;
    public string difficulty;
    public string question;
    public string correct_answer;
    public string[] incorrect_answers;


}



