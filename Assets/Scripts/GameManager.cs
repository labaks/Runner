using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject GameWindow = null;
    [SerializeField] private GameObject FinishWindow = null;
    [SerializeField] private GameObject Hero = null;

    private HeroCharacterController heroCharacterController;
    void Start()
    {
        heroCharacterController = Hero.GetComponent<HeroCharacterController>();
        startGame();
    }

    void Update()
    {

    }

    public void startGame() {
        Hero.transform.position = new Vector3(0f, 0f, 0f);
        heroCharacterController.heroHealth = 100f;
        heroCharacterController.runSpeed = 2f;
        Hero.SetActive(true);
    }

    public void restartGame()
    {
        Hero.SetActive(false);
        FinishWindow.SetActive(false);
        GameWindow.SetActive(true);
        startGame();
    }

    public void finishGame()
    {
        FinishWindow.SetActive(true);
        GameWindow.SetActive(false);
    }
}
