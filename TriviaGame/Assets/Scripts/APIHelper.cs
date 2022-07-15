using UnityEngine;
using System.Net;
using System.IO;


public static class APIHelper
{
    public static FetchedCategories ApiFetchCategories()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://opentdb.com/api_category.php");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        return JsonUtility.FromJson<FetchedCategories>(json);
    }


    public static FetchedQuestions ApiFetchRandomQuestions()
    {
        string url = "https://opentdb.com/api.php?amount=10&type=multiple";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        return JsonUtility.FromJson<FetchedQuestions>(json);
    }

    public static FetchedQuestions ApiFetchQuestionsByCategory(Category category)
    {
        string url = "https://opentdb.com/api.php?amount=10&category=" + category.id + "&type=multiple";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        return JsonUtility.FromJson<FetchedQuestions>(json);
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



