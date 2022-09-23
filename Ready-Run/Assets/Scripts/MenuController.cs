using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    private GameObject selector;
    [SerializeField]private int pos = 0;
    private Vector3[] select_pos =
    {
        new Vector3(-1.38f, -0.32f, 0f),
        new Vector3(-2.64f, -1.345f, 0f),
        new Vector3(-1.875f, -2.33f, 0f),
        new Vector3(-1.134f, -3.332f, 0f),
    };
    private TextMeshPro[] text_select;
    private Color marelin_legal = new Color(0.9716981f, 0.7697805f, 0.2154236f);
    void Start()
    {
        selector = GameObject.Find("SelectorParent");
        text_select = GameObject.Find("Select").GetComponentsInChildren<TextMeshPro>();

        selector.transform.position = select_pos[pos];
        text_select[pos].color = marelin_legal;
    }

    // Update is called once per frame
    void Update()
    {
        pos = Mathf.Clamp(pos, 0, 3);

        SelectControl();
    }

    private void SelectControl()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            pos++;
            SelectSwitch();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            pos--;
            SelectSwitch();
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            DoSelect();
        }
    }

    private void DoSelect()
    {
        switch (pos)
        {
            case 0:
                //Level select
                break;
            case 1:
                //How to play screen
                break;
            case 2:
                //Options Menus
                break;
            case 3:
                Application.Quit();
                break;
        }
    }

    private void SelectSwitch()
    {
        switch (pos)
        {
            case 0:
                selector.transform.position = select_pos[pos];
                TurnAllWhite();
                text_select[pos].color = marelin_legal;
                break;
            case 1:
                selector.transform.position = select_pos[pos];
                TurnAllWhite();
                text_select[pos].color = marelin_legal;
                break;
            case 2:
                selector.transform.position = select_pos[pos];
                TurnAllWhite();
                text_select[pos].color = marelin_legal;
                break;
            case 3:
                selector.transform.position = select_pos[pos];
                TurnAllWhite();
                text_select[pos].color = marelin_legal;
                break;
        }
    }

    private void TurnAllWhite()
    {
        for (int i = 0; i < text_select.Length; i++)
        {
            text_select[i].color = Color.white;
        }
    }
}
