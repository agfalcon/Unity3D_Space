using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OxygenUI : MonoBehaviour
{
    public TextMeshProUGUI ScriptTxt;
    // Start is called before the first frame update
    void Start()
    {
        ScriptTxt.text = "Oxygen\n100%";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOxygen(float oxygen)
    {
        ScriptTxt.text = "Oxygen\n" + oxygen;
    }
}
