using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InputController : MonoBehaviour
{
    ItemControlScript draggingObject; // ��������������� ������
    Vector3 scrollingLastPos; // ��� ���������� ������ �������� �������
    bool scrolling = false; // ����� ������ �� ������ ����� - ���������� ���������
    float cameraXConstraint; // ������������ ��������� ������
    const float backgroundHalfWidth = 6.4f; // ������ ����, �������, ���� ��������� ���������, �� ��� ��������� ������� ��� ��������, ���� ������� ������������ � ���������
    const int scrollMargin = 100; // ���������� �� ���� ������, �� ������� ���� ��������� �������, ����� ������� ��������� ���������
    public const float BottomLimit = -1f; // ��� ����, ����� ���������� ��������� ���� ������ ������

    // �������� ����������, ��� ��������, � ��� �����
    #if UNITY_ANDROID
        const float EdgeScrollingSpeed = 7f;
        const float DragScrollingSpeed = 0.01f;
    #endif
    #if UNITY_STANDALONE
        const float EdgeScrollingSpeed = 0.5f;
        const float DragScrollingSpeed = 0.0005f;
    #endif

    void Start()
    {
        cameraXConstraint = backgroundHalfWidth - Camera.main.aspect * Camera.main.orthographicSize;
    }
    void Update()
    {
        var cursorPos = Input.mousePosition;
        var ray = Camera.main.ScreenPointToRay(new Vector3(cursorPos.x, cursorPos.y, 0));
        var cursorPosInGame = ray.GetPoint(0);

        //�������� �������, ���� ��� �������� ���� ������� - ���������� ��������������, ���� ��� - ���������� ���������
        if (Input.GetMouseButtonDown(0))
        {
            
            var item = Physics2D.OverlapPoint(cursorPosInGame, LayerMask.GetMask("Item"));
            if (item != null)
            {
                draggingObject = item.GetComponent<ItemControlScript>();
                draggingObject.DragStart();
            } else
            {
                scrolling = true;
                scrollingLastPos = cursorPos;
            }
        }

        //����������, ���������� �������������� � ����������
        if (Input.GetMouseButtonUp(0))
        {
            if (draggingObject != null)
            {
                draggingObject.DragEnd();
                draggingObject = null;
            }
            scrolling = false;
        }

        //�������������� ��������
        if (draggingObject != null)
        {
            draggingObject.transform.position = cursorPosInGame;
            if (cursorPos.x < scrollMargin)
                SetCameraXApplyConstraints(transform.position.x - Time.deltaTime * EdgeScrollingSpeed);
            if (cursorPos.x > Camera.main.pixelWidth - scrollMargin)
                SetCameraXApplyConstraints(transform.position.x + Time.deltaTime * EdgeScrollingSpeed);
        }

        //���������
        if (scrolling)
        {
            var delta = cursorPos - scrollingLastPos;
            scrollingLastPos = cursorPos;
            SetCameraXApplyConstraints(transform.position.x - delta.x * DragScrollingSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) 
            Application.Quit();
    }

    
    //��� ������� ������������� � ������ �������� �����������
    void SetCameraXApplyConstraints(float x)
    {
        if (x < -cameraXConstraint) x = -cameraXConstraint;
        if (x > cameraXConstraint) x = cameraXConstraint;
        transform.position = new Vector3(x, transform.position.y, -10);
    }

    static public void ButtonClosePress()
    {
        Application.Quit();
    }
}
