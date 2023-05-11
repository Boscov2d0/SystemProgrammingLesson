using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyTextField : MonoBehaviour
{
    [SerializeField] private Text _textObject;
    [SerializeField] private Scrollbar _scrollbar;

    private List<string> messages = new List<string>();

    private void Start()
    {
        _scrollbar.onValueChanged.AddListener((float value) => UpdateText());
    }
    public void ReceiveMessage(object message)
    {
        messages.Add(message.ToString());
        float value = (messages.Count - 1) * _scrollbar.value;
        _scrollbar.value = Mathf.Clamp(value, 0, 1);
        UpdateText();
    }
    private void UpdateText()
    {
        string text = "";
        int index = (int)(messages.Count * _scrollbar.value);

        for (int i = index; i < messages.Count; i++)
        {
            text += messages[i] + "\n";
        }

        _textObject.text = text;
    }
}