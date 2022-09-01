using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionUI : MonoBehaviour
{
    public TextMeshProUGUI ScriptTxt;
    // Start is called before the first frame update
    void Start()
    {
        ScriptTxt.text = "당신은 우주비행사입니다.\n주어진 산소가 다 떨어지기 전에 동굴을 탐험하여 끝으로 도달하세요!!\n마우스 왼쪽 키 : 총발사\n마우스 오른쪽 키 : 점프부스트\n버튼 c : 조준";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            Destroy(ScriptTxt);
        }
    }
}
