using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    public UnityEngine.UI.Image barraDeVida;
    public TextMeshProUGUI scoreText;
    public GameObject restartButton;
    private float vidaMaxima;
    private playerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<playerController>();
        vidaMaxima = playerController.vidas;
    }

    // Update is called once per frame
    void Update()
    {
        barraDeVida.fillAmount = playerController.vidas / vidaMaxima;
        scoreText.text = "Puntaje: " + GameManager.instance.score;
        if (Input.GetKey(KeyCode.Escape))
        {
            restartScene();
        }
    }

    public void showRestartButton()
    {
        restartButton.SetActive(true);
    }
    public void restartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
