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
        ScriptTxt.text = "����� ���ֺ�����Դϴ�.\n�־��� ��Ұ� �� �������� ���� ������ Ž���Ͽ� ������ �����ϼ���!!\n���콺 ���� Ű : �ѹ߻�\n���콺 ������ Ű : �����ν�Ʈ\n��ư c : ����";
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
