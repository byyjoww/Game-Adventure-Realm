using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter
{
    private Text textComponent;
    private TMP_Text tmproComponent;
    private string textToWrite;
    private bool simulateFullText;
    private Action onComplete;
    private Tuple<TextWriter, MaskableGraphic> listInstance;

    private int characterIndex;

    private static List<Tuple<TextWriter, MaskableGraphic>> activeWriters = new List<Tuple<TextWriter, MaskableGraphic>>();

    #region STATIC_FUNCTIONS
    public static bool WriteToTMPro(MonoBehaviour worker, TMP_Text textComponent, string textToWrite, float timePerCharacter = 0.1f, 
        bool simulateFullText = true, bool clearText = true, bool canInterrupt = true, Action onComplete = null)
    {
        if(!TryCreateWriter(textComponent, canInterrupt)) { return false; }

        new TextWriter(worker, textComponent, textToWrite, timePerCharacter, simulateFullText, clearText, onComplete);

        return true;
    }

    public static bool WriteToText(MonoBehaviour worker, Text textComponent, string textToWrite, float timePerCharacter = 0.1f, 
        bool simulateFullText = true, bool clearText = true, bool canInterrupt = true, Action onComplete = null)
    {
        if (!TryCreateWriter(textComponent, canInterrupt)) { return false; }

        new TextWriter(worker, textComponent, textToWrite, timePerCharacter, simulateFullText, clearText, onComplete);

        return true;
    }

    public static bool TryCreateWriter(MaskableGraphic textComponent, bool canInterrupt)
    {
        var tuple = GetTuple(textComponent);

        if (tuple == null) { return true; }        

        if (canInterrupt) { InterruptWriter(textComponent); }

        //Debug.Log("Attempting to create a text writer while another is active.");

        return false;
    }

    private static Tuple<TextWriter, MaskableGraphic> GetTuple(MaskableGraphic textComponent)
    {
        return activeWriters.SingleOrDefault(x => x.Item2 == textComponent);
    }

    public static void InterruptWriter(MaskableGraphic textComponent)
    {
        Tuple<TextWriter, MaskableGraphic> tuple = GetTuple(textComponent);

        if (tuple == null) { return; }

        tuple.Item1.Interrupt();
    }
    #endregion

    #region CONSTRUCTORS
    private TextWriter(MonoBehaviour worker, TMP_Text textComponent, string textToWrite, float timePerCharacter, 
        bool simulateFullText, bool clearText, Action onComplete = null)
    {
        this.tmproComponent = textComponent;
        this.textToWrite = textToWrite;
        this.simulateFullText = simulateFullText;
        this.onComplete = onComplete;

        listInstance = new Tuple<TextWriter, MaskableGraphic>(this, textComponent);
        activeWriters.Add(listInstance);

        if (clearText) { textComponent.text = ""; }
        Tools.Update(worker, WriteText, timePerCharacter, WriteComplete);
    }

    private TextWriter(MonoBehaviour worker, Text textComponent, string textToWrite, float timePerCharacter, 
        bool simulateFullText, bool clearText, Action onComplete = null)
    {
        this.textComponent = textComponent;
        this.textToWrite = textToWrite;
        this.simulateFullText = simulateFullText;
        this.onComplete = onComplete;

        listInstance = new Tuple<TextWriter, MaskableGraphic>(this, textComponent);
        activeWriters.Add(listInstance);

        if (clearText) { textComponent.text = ""; }
        Tools.Update(worker, WriteText, timePerCharacter, WriteComplete);
    }
    #endregion

    #region WRITE_FUNCTIONS
    private void WriteText()
    {
        if(textComponent == null && tmproComponent == null) { return; }
        if (characterIndex >= textToWrite.Length) { return; }

        characterIndex++;
        string txt = textToWrite.Substring(0, characterIndex);

        if (simulateFullText)
        {
            var builder = new StringBuilder();
            builder.Append("<color=#00000000>");
            builder.Append(textToWrite.Substring(characterIndex));
            builder.Append("</color>");
            txt += builder.ToString();
        }

        if (textComponent != null) { textComponent.text = txt; }
        if (tmproComponent != null) { tmproComponent.text = txt; }
    }

    private bool WriteComplete()
    {
        if (characterIndex >= textToWrite.Length) 
        {
            RemoveFromActive();

            return true; 
        }

        return false;
    }

    private void Interrupt()
    {
        characterIndex = textToWrite.Length;

        if (textComponent != null) { textComponent.text = textToWrite; }
        if (tmproComponent != null) { tmproComponent.text = textToWrite; }

        RemoveFromActive();
    }

    private void RemoveFromActive()
    {
        if(activeWriters.Remove(listInstance)) { onComplete?.Invoke(); }        
    }
    #endregion
}
