using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private void Start()
    {
        UIManager.StartLevel();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pause");
            if (Time.timeScale == 0)
                UIManager.Resume();
            else UIManager.ShowMenu();
        }
    }
}