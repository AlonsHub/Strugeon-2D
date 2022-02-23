using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//used to be "BasicMessage" - as it was the basic IdleLog message, but it can be any displayer it set's its mind to!
public class BasicDisplayer : MonoBehaviour
{
    public List<TMP_Text> textBoxes;
    public List<Image> images;

    //public IdleLogOrder myOrder; //not always used - set remotely when relevant


    public virtual bool SetMe(List<string> textsPerTextBox, List<Sprite> spritesPerImage)
    {
        if(textBoxes.Count != textsPerTextBox.Count || images.Count != spritesPerImage.Count)
        {
            Debug.LogError("incorrect amount of text or sprites");
            return false;
        }

        for (int i = 0; i < textsPerTextBox.Count; i++)
        {
            textBoxes[i].text = textsPerTextBox[i];
        }

        for (int i = 0; i < spritesPerImage.Count; i++)
        {
            images[i].sprite = spritesPerImage[i];
        }

        return true;
    }
    //public virtual bool SetMe(string singleText, Sprite singleSprite) //single-each version //TBD Make sure other single-uses dont actually use the list version!!!
    //{
    //    if (textBoxes.Count == 0 || images.Count == 0)
    //    {
    //        return false;
    //    }

    //    textBoxes[0].text = singleText;
    //    images[0].sprite = singleSprite;

    //    return true;
    //}
    //public virtual bool SetMe(List<string> textsPerTextBox)
    //{
    //    if (textBoxes.Count != textsPerTextBox.Count)
    //    {
    //        Debug.LogError("incorrect amount of text/boxes");
    //        return false;
    //    }


    //    for (int i = 0; i < textsPerTextBox.Count; i++)
    //    {
    //        textBoxes[i].text = textsPerTextBox[i];
    //    }


    //    return true;
    //}
    //public virtual bool SetMe(List<Sprite> spritesPerImage)
    //{
    //    if (images.Count != spritesPerImage.Count)
    //    {
    //        Debug.LogError("incorrect amount of sprites/images");
    //        return false;
    //    }

    //    for (int i = 0; i < spritesPerImage.Count; i++)
    //    {
    //        images[i].sprite = spritesPerImage[i];
    //    }

    //    return true;
    //}

    //RemoveOrderFromBackLogAndDoDestroy() can just be "my button" and inhereting children will override and call base if they want logic and also to remove the message
    public virtual void BasicDisplayersButton(bool doDestroy) //BasicDisplayers with buttons can use this to remove themselves (if they came by order)
    {
        //IdleLog.backLog.Remove(myOrder);

        //out animation

        if(doDestroy)
        Destroy(gameObject);
    }

    //public void UnSetMe(bool doDestroy)
    //{

    //}
}
