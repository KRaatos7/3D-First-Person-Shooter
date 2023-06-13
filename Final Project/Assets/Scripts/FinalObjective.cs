using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalObjective : MonoBehaviour
{

    private float radius = 3f;
    public MovementScript player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Menu");
        }
    }
}
