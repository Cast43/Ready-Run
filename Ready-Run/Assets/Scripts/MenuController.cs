using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private Color cor_text = new Color(0.2431373f, 0.4117647f, 0.345098f);
    private Color cor_text_normal = new Color(0.8901961f, 0.8117647f, 0.7058824f);  
    private LoaderScript loader;
    void Start()
    {
        selector = GameObject.Find("SelectorParent");
        text_select = GameObject.Find("Select").GetComponentsInChildren<TextMeshPro>();
        loader = GameObject.Find("Loader").GetComponent<LoaderScript>();

        selector.transform.position = select_pos[pos];
        text_select[pos].color = cor_text;
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
                loader.LoadScene("Main");
                break;
            case 1:
                loader.LoadScene("HTP");
                break;
            case 2:
                loader.LoadScene("Options");
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
                TurnAllNormal();
                text_select[pos].color = cor_text;
                break;
            case 1:
                selector.transform.position = select_pos[pos];
                TurnAllNormal();
                text_select[pos].color = cor_text;
                break;
            case 2:
                selector.transform.position = select_pos[pos];
                TurnAllNormal();
                text_select[pos].color = cor_text;
                break;
            case 3:
                selector.transform.position = select_pos[pos];
                TurnAllNormal();
                text_select[pos].color = cor_text;
                break;
        }
    }

    private void TurnAllNormal()
    {
        for (int i = 0; i < text_select.Length; i++)
        {
            text_select[i].color = cor_text_normal;
        }
    }
}
