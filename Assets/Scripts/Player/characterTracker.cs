using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class characterTracker : MonoBehaviour
{

    public static int animalToInstance;
    [SerializeField] private GameObject[] animalPrefabs;
    private Transform player;
    private ballcontroller playercontroller;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject animalModel;
            if (SceneManager.GetActiveScene().buildIndex == 2){ // tutorial
                    Time.timeScale = 0f;
                    player = GameObject.Find("Player").GetComponent<Transform>();
                    playercontroller = GameObject.Find("Player").GetComponent<ballcontroller>();
                    animalModel = Instantiate(animalPrefabs[animalToInstance], new Vector3(8, 4, -11.25f), Quaternion.identity);
                    animalModel.transform.parent = player;
                    playercontroller.animator = GameObject.FindGameObjectsWithTag("Animations")[0].GetComponent<Animator>();
                    Time.timeScale = 1f;
            } else if (SceneManager.GetActiveScene().buildIndex == 4){ // city
                    Time.timeScale = 0f;
                    player = GameObject.Find("Player").GetComponent<Transform>();
                    playercontroller = GameObject.Find("Player").GetComponent<ballcontroller>();
                    animalModel = Instantiate(animalPrefabs[animalToInstance], new Vector3(0, 70, -90f), Quaternion.identity);
                    playercontroller.animator = GameObject.FindGameObjectsWithTag("Animations")[0].GetComponent<Animator>();
                    animalModel.transform.parent = player;
                    Time.timeScale = 1f;
            } else if (SceneManager.GetActiveScene().buildIndex == 6){ // final
                    Time.timeScale = 0f;
                    player = GameObject.Find("Player").GetComponent<Transform>();
                    playercontroller = GameObject.Find("Player").GetComponent<ballcontroller>();
                    animalModel = Instantiate(animalPrefabs[animalToInstance], new Vector3(0, 500, -45f), Quaternion.identity);
                    playercontroller.animator = GameObject.FindGameObjectsWithTag("Animations")[0].GetComponent<Animator>();
                    animalModel.transform.parent = player;
                    Time.timeScale = 1f;
            }
    }
}
