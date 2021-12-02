using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Objectives : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;

    public float textSpeed;

    private int index;

    private bool isTyping = false;

    public void Start()
    {
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine(lines[0]));
        index = 0;
        //StartCoroutine(MarkCompleted());
    }

    private bool CompletedLevel()
    {
        return index >= lines.Length;
    }

    public void CompleteObjective()
    {
        string setCompletedText = "";

        index++;

        Debug.Log(index);

        for (int i = 0; i < index; i++)
        {
            setCompletedText += "- " + lines[i] + " [Completed]\n";
        }

        textComponent.text = setCompletedText;  

        if (!CompletedLevel())
        {
            StartCoroutine(TypeLine(lines[index]));
        }
         
    }

    IEnumerator TypeLine(string line)
    {
        yield return new WaitUntil(() => !isTyping);
        isTyping = true;

        string objective = line;
        objective = "- " + objective + " []\n";
        foreach (char c in objective.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }

    //private IEnumerator MarkCompleted()
    //{
    //    for (int i = 0; i < lines.Length; i++)
    //    {
    //        yield return new WaitForSeconds(2f);
    //        CompleteObjective();
    //    }
    //}
}
