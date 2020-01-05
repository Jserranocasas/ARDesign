using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ModeAction : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    private static ModeAction s_Instance = null;

    /// <summary>
    /// 
    /// </summary>
    private ModeStatus status;

    /// <summary>
    /// 
    /// </summary>
    public GameObject editionPanel;

    /// <summary>
    /// 
    /// </summary>
    public GameObject selectButton;

    /// <summary>
    /// 
    /// </summary>
    public GameObject optionButton;


    /// <summary>
    /// 
    /// </summary>
    public static ModeAction Instance
    {
        get
            { 
                return s_Instance;
            }
    }

    private void Awake()
    {
        if (s_Instance != null && s_Instance != this)
        {
            Destroy(this.gameObject);
        }

        s_Instance = this;
        
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() 
    {
        status = ModeStatus.Start;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="st"></param>
    public void setMode(ModeStatus st)
    {
        if(st == ModeStatus.Insertion){
            StartCoroutine(WaitAndInsert(0.05f));      
        } 
        else  if (st == ModeStatus.Edition)
        {
            changeModeEdition();
        }
        else if (st == ModeStatus.Selection)
        {
            changeModeSelection();
        }
        else if (st == ModeStatus.Creation)
        {
            changeModeCreation();
        }
        else if (st == ModeStatus.Option)
        {
            changeModeOptions();
        }
        else
        {
            changeModeRuler();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="st"></param>
    public ModeStatus getMode()
    {
        return status;
    }

    /// <summary>
    /// Firstly wait a few seconds and then change mode to insertion
    /// </summary>
    /// <param name="seconds">Seconds to wait</param>
    IEnumerator WaitAndInsert(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        //Change to insertion mode
        changeModeInsertion();
    }

    /// <summary>
    /// 
    /// </summary>
    private void changeModeEdition() 
    {
        status = ModeStatus.Edition;
        editionPanel.SetActive(true);
        selectButton.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    private void changeModeInsertion()
    {
        status = ModeStatus.Insertion;
        editionPanel.SetActive(false);
        selectButton.SetActive(true);
        optionButton.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    private void changeModeSelection()
    {
        status = ModeStatus.Selection;
        selectButton.SetActive(false);
        editionPanel.SetActive(false);
        optionButton.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    private void changeModeCreation()
    {
        status = ModeStatus.Creation;
    }

    /// <summary>
    /// 
    /// </summary>
    private void changeModeOptions()
    {
        status = ModeStatus.Option;
        selectButton.SetActive(false);
        optionButton.SetActive(false);
        editionPanel.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    private void changeModeRuler()
    {
        status = ModeStatus.Ruler;
        selectButton.SetActive(false);
        optionButton.SetActive(false);
        editionPanel.SetActive(false);
    }
}
