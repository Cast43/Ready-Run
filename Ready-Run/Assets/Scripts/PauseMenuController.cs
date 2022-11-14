using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenuController : MonoBehaviour
{
    public TextMeshProUGUI[] texts;
    [SerializeField] private int pos = 1;
    private Color cor_text = new Color(1f, 0.9215686f, 0.3411765f);
    private Color cor_text_normal = new Color(1f, 1f, 1f);
    private LoaderScript loader;
    [SerializeField] private bool just_up;
    [SerializeField] private bool just_down;
    // Start is called before the first frame update
    void Start()
    {
        texts = GetComponentsInChildren<TextMeshProUGUI>();
        loader = GameObject.Find("Loader").GetComponent<LoaderScript>();

        texts[pos].color = cor_text;
    }

    void Update()
    {
        pos = Mathf.Clamp(pos, 1, 3);

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
        else if (Input.GetAxis("VerticalMenu") == 0f)
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
            case 1:
                //cont
                break;
            case 2:
                //restart
                break;
            case 3:
                loader.LoadScene("Menu");
                break;
        }
    }
    private void SelectSwitch()
    {
        switch (pos)
        {
            case 1:
                TurnAllNormal();
                texts[pos].color = cor_text;
                break;
            case 2:
                TurnAllNormal();
                texts[pos].color = cor_text;
                break;
            case 3:
                TurnAllNormal();
                texts[pos].color = cor_text;
                break;
        }
    }

    private void TurnAllNormal()
    {
        for (int i = 1; i < texts.Length; i++)
        {
            texts[i].color = cor_text_normal;
        }
    }
}
