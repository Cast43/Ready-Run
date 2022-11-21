using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    private RectTransform selector;
    [SerializeField]private int pos = 0;
    private Vector3[] select_pos =
    {
        new Vector3(-168f, -41f, 0f),
        new Vector3(-297f, -165f, 0f),
        new Vector3(-205f, -289f, 0f),
        new Vector3(-130f, -414f, 0f),
    };
    private TextMeshProUGUI[] text_select;
    private Color cor_text = new Color(1f, 0.9215686f, 0.3411765f);
    private Color cor_text_normal = new Color(1f, 1f, 1f);  
    private LoaderScript loader;
    [SerializeField] private bool just_up;
    [SerializeField] private bool just_down;
    void Start()
    {
        selector = GameObject.Find("Canvas/SelectorP").GetComponent<RectTransform>();
        text_select = GameObject.Find("Canvas/Texts/SelectOpts").GetComponentsInChildren<TextMeshProUGUI>();
        loader = GameObject.Find("Loader").GetComponent<LoaderScript>();

        selector.anchoredPosition = select_pos[pos];
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
        if (Input.GetAxis("VerticalMenu") == -1f && just_up == false)
        {
            pos++;
            just_down = false;
            just_up = true;
            SelectSwitch();
        }
        else if (Input.GetAxis("VerticalMenu") == 1f && just_down == false)
        {
            pos--;
            just_up = false;
            just_down = true;
            SelectSwitch();
        }
        else if(Input.GetAxis("VerticalMenu") == 0f)
        {
            just_up = false;
            just_down = false;
        }

        if (Input.GetButtonDown("Submit"))
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
                selector.anchoredPosition = select_pos[pos];
                TurnAllNormal();
                text_select[pos].color = cor_text;
                break;
            case 1:
                selector.anchoredPosition = select_pos[pos];
                TurnAllNormal();
                text_select[pos].color = cor_text;
                break;
            case 2:
                selector.anchoredPosition = select_pos[pos];
                TurnAllNormal();
                text_select[pos].color = cor_text;
                break;
            case 3:
                selector.anchoredPosition = select_pos[pos];
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
