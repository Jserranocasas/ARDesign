using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class ChangeFieldsTutorial : MonoBehaviour
{
    public static int currentPagee = 0;

    public GameObject backButton;

    public Sprite[] checkedSprite;

    public Sprite[] backgrounds;

    public string[] texts;

    public Text textTutorial;

    void Start()
    {
        StartTutorial();
    }


    public void OnClickNextTutorial()
    {

        if (currentPagee == 0)
        {
            GameObject.Find("Modifiable Panel").GetComponent<Image>().sprite = backgrounds[++currentPagee];
            GameObject.Find("State Image").GetComponent<Image>().sprite = checkedSprite[0];
            GameObject.Find("State Image2").GetComponent<Image>().sprite = checkedSprite[1];
            UpdateText();
            backButton.SetActive(true);
        }
        else if (currentPagee == 1)
        {
            GameObject.Find("Modifiable Panel").GetComponent<Image>().sprite = backgrounds[++currentPagee];
            GameObject.Find("State Image2").GetComponent<Image>().sprite = checkedSprite[0];
            GameObject.Find("State Image3").GetComponent<Image>().sprite = checkedSprite[1];
            UpdateText();
        }
        else if (currentPagee == 2)
        {
            GameObject.Find("Modifiable Panel").GetComponent<Image>().sprite = backgrounds[++currentPagee];
            GameObject.Find("State Image3").GetComponent<Image>().sprite = checkedSprite[0];
            GameObject.Find("State Image4").GetComponent<Image>().sprite = checkedSprite[1];
            UpdateText();
        }
        else if (currentPagee == 3)
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void OnClickBackTutorial()
    {

        if (currentPagee == 1)
        {
            GameObject.Find("Modifiable Panel").GetComponent<Image>().sprite = backgrounds[--currentPagee];
            GameObject.Find("State Image").GetComponent<Image>().sprite = checkedSprite[1];
            GameObject.Find("State Image2").GetComponent<Image>().sprite = checkedSprite[0];
            UpdateText();
            backButton.SetActive(false);
        }
        else if (currentPagee == 2)
        {
            GameObject.Find("Modifiable Panel").GetComponent<Image>().sprite = backgrounds[--currentPagee];
            GameObject.Find("State Image2").GetComponent<Image>().sprite = checkedSprite[1];
            GameObject.Find("State Image3").GetComponent<Image>().sprite = checkedSprite[0];
            UpdateText();
        }
        else if (currentPagee == 3)
        {
            GameObject.Find("Modifiable Panel").GetComponent<Image>().sprite = backgrounds[--currentPagee];
            GameObject.Find("State Image3").GetComponent<Image>().sprite = checkedSprite[1];
            GameObject.Find("State Image4").GetComponent<Image>().sprite = checkedSprite[0];
            UpdateText();
        }
    }

    private void UpdateText(){
        if (GlobalLanguage.CurrentLanguage == "Spanish")
        {
            textTutorial.text = texts[currentPagee];
        }
        if (GlobalLanguage.CurrentLanguage == "English")
        {
            textTutorial.text = texts[currentPagee+4];
        }
        if (GlobalLanguage.CurrentLanguage == "French")
        {
            textTutorial.text = texts[currentPagee+8];
        }
        if(GlobalLanguage.CurrentLanguage == null)
        {
            textTutorial.text = texts[currentPagee];
        }
    }

    public void OnClickSkip() {
        SceneManager.LoadScene("Menu");
    }

    public void StartTutorial(){
        currentPagee = 0;
        GameObject.Find("Modifiable Panel").GetComponent<Image>().sprite = backgrounds[currentPagee];
        GameObject.Find("State Image").GetComponent<Image>().sprite = checkedSprite[1];
        GameObject.Find("State Image2").GetComponent<Image>().sprite = checkedSprite[0];
        GameObject.Find("State Image3").GetComponent<Image>().sprite = checkedSprite[0];
        GameObject.Find("State Image4").GetComponent<Image>().sprite = checkedSprite[0];

        backButton.SetActive(false);
        UpdateText();
    }
}
