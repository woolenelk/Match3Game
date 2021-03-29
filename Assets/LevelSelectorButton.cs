using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelectorButton : MonoBehaviour
{
    [SerializeField]
    string level;
    public void OnLevelLoadButtonPressed()
    {
        SceneManager.LoadScene(level);
    }

}
