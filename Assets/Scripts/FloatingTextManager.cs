using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;
    private List<FloatingText> floatingTexts = new List<FloatingText>();

    private void Update() 
    {
        foreach(FloatingText floatingText in floatingTexts)
        {
            floatingText.UpdateFloatingText();
        }
    }

    public void Show(string message, int fontSize, Color color, Vector3 position, Vector3 motion, float showDuration)
    {
        //get an inactive floating text object
        FloatingText floatingText = GetFloatingText();
        floatingText.Text.text = message;
        floatingText.Text.fontSize = fontSize;
        floatingText.Text.color = color;
        floatingText.Text.transform.position = Camera.main.WorldToScreenPoint(position);
        floatingText.Motion = motion;
        floatingText.ShowDuration = showDuration;
        floatingText.Text.text = message;

        floatingText.Show();
    }

    private FloatingText GetFloatingText()
    {
        //try to retrieve first found inactive floating text
        FloatingText floatingText = floatingTexts.Find(x => !x.IsActive);

        //if cannot find any inactive floating text
        if(floatingText == null)
        {
            //create new floating text and assign it into the array
            floatingText = new FloatingText();
            floatingText.Target = Instantiate(textPrefab);
            floatingText.Target.transform.SetParent(textContainer.transform);
            floatingText.Text = floatingText.Target.GetComponent<Text>();

            floatingTexts.Add(floatingText);
        }

        return floatingText;
    }


}
