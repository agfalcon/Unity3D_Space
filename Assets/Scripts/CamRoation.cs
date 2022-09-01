using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRoation : MonoBehaviour
{

    public float rotSpeed = 200.0f;

    float mx = 0;
    float my = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ���콺 �Է� ���� �޾ƿ�
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        //ȸ�� �� ������ ���콺 �Է� ���� ��ô
        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;

        my = Mathf.Clamp(my, -90f, 90f);

        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
