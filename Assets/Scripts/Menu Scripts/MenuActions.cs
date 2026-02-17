using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{

    [SerializeField] GameObject controlsScreen;
    [SerializeField] TextMeshProUGUI playerCount; 
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
        SceneManager.LoadScene(1);
    }
}
