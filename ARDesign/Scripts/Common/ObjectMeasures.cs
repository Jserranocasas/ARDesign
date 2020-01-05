using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using GoogleARCore;
using UnityEngine;

public class ObjectMeasures : MonoBehaviour
{
    private bool hasChangeSprite;

    public Image ObjectImage;

    public InputField WidthField;
    public InputField HightField;
    public InputField DepthField;

    public List<Sprite> Images = new List<Sprite>();
    public List<string> NameImages = new List<string>();

    /// <summary>
    /// Dictionary to manager furniture images.
    /// </summary>
    private Dictionary<string, Sprite> FurnitureImages = new Dictionary<string, Sprite>();

    void Start() 
    {
        for(int i=0; i<Images.Count; i++)
        {
            FurnitureImages.Add(NameImages[i], Images[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(hasChangeSprite){
            hasChangeSprite = false;

            GameObject ObjectToScale = Session.GetSelectedObject();
            ObjectImage.sprite = FurnitureImages[ObjectToScale.name];
        }
    }

    /// <summary>
    /// Scale selected object with measures of InputField.
    /// </summary>
    public void ScaleObject(){
        if(WidthField.text != "" && HightField.text != "" && DepthField.text != "")
        {
            GameObject ObjectToScale = Session.GetSelectedObject();

            if(ObjectToScale != null)
            {
                Vector3 sizeVec = ObjectToScale.GetComponent<Collider>().bounds.size;
                Vector3 scaleVector = new Vector3(  
                    float.Parse(WidthField.text) / (sizeVec.x * 100f),
                    float.Parse(HightField.text) / (sizeVec.y * 100f),
                    float.Parse(DepthField.text) / (sizeVec.z * 100f));

                ObjectToScale.transform.localScale = scaleVector;
            }
        }

        WidthField.text = ""; HightField.text = ""; DepthField.text = "";
    }

    public void IntoRuler()
    {
        hasChangeSprite = true;
        ModeAction.Instance.setMode(ModeStatus.Ruler);
    }

    public void IntoEdition()
    {
        ModeAction.Instance.setMode(ModeStatus.Edition);
    }
}
