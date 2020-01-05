using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StartTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Load text from a JSON file (Assets/Resources/jsonContinueTutorial.json)
        // TextAsset jsonFile = Resources.Load("jsonContinueTutorial") as TextAsset;

        // StartSession loadedStartSession = JsonUtility.FromJson<StartSession>(jsonFile.ToString());

        // if (loadedStartSession.getShowTutorial())
        // {
        //     StartSession startSession = new StartSession();
        //     startSession.setShowTutorial(false);
        //     string json = JsonUtility.ToJson(startSession);

        //     File.WriteAllText(Application.persistentDataPath + "/Resources/jsonContinueTutorial.json", json);
        // }
        // else
        // {
        //     SceneManager.LoadScene("Menu");
        // }
    }

    /// <summary>
    /// Class that represent a json object.
    /// </summary>
    [System.Serializable]
    private class StartSession
    {
        public bool showTutorial;

        public bool getShowTutorial()
        {
            return showTutorial;
        }

        public void setShowTutorial(bool st)
        {
            showTutorial = st;
        }
    }
}
