using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VisIntHolder : MonoBehaviour
{
    public IntHolder intHolder;
    public string prefix, suffix;
    public TMP_Text text;

    public void Start()
    {
        if (intHolder)
        {
            intHolder.OnValueChanged.AddListener(OnValueChanged);
            OnValueChanged(intHolder.value);
        }
    }

    public void OnValueChanged(int value)
    {
        text.text = $"{suffix}{value}{prefix}";
    }
}
