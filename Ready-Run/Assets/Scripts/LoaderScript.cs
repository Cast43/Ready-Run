using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderScript : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private float transition_duration;

    public void LoadScene(string name)
    {
        StartCoroutine(LoadDelay(name));
    }

    IEnumerator LoadDelay(string name)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transition_duration);
        SceneManager.LoadScene(name);
    }
}
