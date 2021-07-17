using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject GameWindow = null;
    [SerializeField] private GameObject FinishWindow = null;
    [SerializeField] private GameObject StartWindow = null;
    [SerializeField] private Text Distance = null;
    [SerializeField] private GameObject Hero = null;
    [SerializeField] private GameObject Camera = null;
    [SerializeField] private GameObject StartPlatform = null;
    [SerializeField] private RoadGenerator roadGenerator = null;

    private int passedDistance;

    private HeroCharacterController heroCharacterController;
    private Vector3 cameraStartPosition;
    void Start()
    {
        heroCharacterController = Hero.GetComponent<HeroCharacterController>();
        cameraStartPosition = Camera.transform.position;
    }

    void Update()
    {
        passedDistance = (int)Hero.transform.position.x;
        Distance.text = passedDistance.ToString();
    }

    public void startGame()
    {
        StartWindow.SetActive(false);
        GameWindow.SetActive(true);
        RefreshHero();
        heroCharacterController.horizontalInput = 1f;
        heroCharacterController.actualSpeed = heroCharacterController.speed;
        roadGenerator.StartSpawn();
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

    public void toMainMenu()
    {
        FinishWindow.SetActive(false);
        StartWindow.SetActive(true);
        RefreshHero();
        heroCharacterController.actualSpeed = 0;
    }

    private void RefreshHero()
    {
        Hero.transform.position = new Vector3(0f, 0f, 0f);
        Camera.transform.position = cameraStartPosition;
        heroCharacterController.heroHealth = 100f;
        Hero.SetActive(true);
    }
}
