using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideText : MonoBehaviour
{
    private float _timeSinceLastSetText = 10000;
    public Text Text { get { return this.GetComponent<Text>(); } }

    private void Awake()
    {
        this.Text.CrossFadeAlpha(0, 0.5f, true);
    }

    public void SetText(string text)
    {
        this.Text.CrossFadeAlpha(1, 0.5f, true);
        _timeSinceLastSetText = 0;
        this.Text.text = text;

    }

    private void Update()
    {
        const float TimeToShowText = 5;
        if(_timeSinceLastSetText < TimeToShowText &&  _timeSinceLastSetText + Time.deltaTime > TimeToShowText)
        {
            this.Text.CrossFadeAlpha(0, 0.5f, true);
        }

        _timeSinceLastSetText += Time.deltaTime;
    }
}
