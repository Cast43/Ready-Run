using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public TextMeshProUGUI[] texts;
    [SerializeField] private int pos = 1;
    private Color cor_text = new Color(1f, 0.9215686f, 0.3411765f);
    private Color cor_text_normal = new Color(1f, 1f, 1f);
    private LoaderScript loader;
    [SerializeField] private bool just_up;
    [SerializeField] private bool just_down;
    [SerializeField] private PlayerMovement pc;
    // Start is called before the first frame update
    void Start()
    {
        texts = GetComponentsInChildren<TextMeshProUGUI>();
        loader = GameObject.Find("Loader").GetComponent<LoaderScript>();
        pc = GameObject.Find("Player").GetComponent<PlayerMovement>();

        texts[pos].color = cor_text;
    }

    void Update()
    {
        if (pc.is_paused)
        {
            SelectControl();
        }
        pos = Mathf.Clamp(pos, 1, 3);

    }

    private void SelectControl()
    {
        if (Input.GetAxisRaw("VerticalMenu") == -1f && just_up == false)
        {
            pos++;
            just_down = false;
            just_up = true;
            SelectSwitch();
        }
        else if (Input.GetAxisRaw("VerticalMenu") == 1f && just_down == false)
        {
            pos--;
            just_up = false;
            just_down = true;
            SelectSwitch();
        }
        else if (Input.GetAxisRaw("VerticalMenu") == 0f)
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
                pc.is_paused = !pc.is_paused;
                break;
            case 2:
                loader.LoadScene("Main");
                pc.is_paused = false;
                break;
            case 3:
                loader.LoadScene("Menu");
                pc.is_paused = false;
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
