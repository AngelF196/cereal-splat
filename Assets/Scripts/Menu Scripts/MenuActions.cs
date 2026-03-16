using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    [SerializeField] GameObject controlsScreen;
    GameManager manager = GameManager.Instance;
    [SerializeField] TextMeshProUGUI playerCount;
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource ready;
    [SerializeField] private Results results;
    public void IncPlayers() 
    {
        if (GameManager.Instance.numPlayers < 4)
        {
            GameManager.Instance.numPlayers++;
            playerCount.text = "Players: " + GameManager.Instance.numPlayers.ToString();
        }
    }

    public void DecPlayers()
    {
        if (GameManager.Instance.numPlayers > 1)
        {
            GameManager.Instance.numPlayers--;
            playerCount.text = "Players: " + GameManager.Instance.numPlayers.ToString();
        }
    }

    public void ToggleControls()
    {
        if (!controlsScreen.activeInHierarchy) 
        {
            controlsScreen.SetActive(true);
        }
        else
        {
            controlsScreen.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Debug.Log("lol");
    }

    public void LoadGame()
    {
        music.Stop();
        ready.Play();
        StartCoroutine(PlayAndWait());
    }
    IEnumerator PlayAndWait()
    {
        ready.Play();

        yield return new WaitWhile(() => ready.isPlaying);

        // Code here runs after the sound finishes
        SceneManager.LoadScene(1);
    }
    public void LoadResults()
    {
        int winner = results.GetWinner();
        if (manager != null)
        {
            manager.winningIndex = winner;
        }
        SceneManager.LoadScene(2);
    }

    public void LoadTitle() 
    {
        SceneManager.LoadScene(0);
    }
}
