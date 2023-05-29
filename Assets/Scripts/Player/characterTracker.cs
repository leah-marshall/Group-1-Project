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
    private int CAT = 0;
    private int PUFF = 1;
    private int OCTO = 2;
    private int BIRD = 3;
    private int BEAR = 4;
    private int WHALE = 5;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject animalModel;
            if (SceneManager.GetActiveScene().buildIndex == 2){ // tutorial
                    Time.timeScale = 0f;
                    player = GameObject.Find("Player").GetComponent<Transform>();
                    playercontroller = GameObject.Find("Player").GetComponent<ballcontroller>();
                    Vector3 animalPos = new Vector3(8, 4, -11.25f);
                    Quaternion animalRotation = Quaternion.identity;
                    if (animalToInstance == CAT){
                        animalPos = new Vector3(8, 5.3f, -11.25f);
                        animalRotation.eulerAngles = new Vector3(animalRotation.eulerAngles.x, 90, animalRotation.eulerAngles.z);
                    } else if (animalToInstance == PUFF){
                        animalPos = new Vector3(8, 5.3f, -11.25f);
                        animalRotation.eulerAngles = new Vector3(animalRotation.eulerAngles.x, 0, animalRotation.eulerAngles.z);
                    } else if (animalToInstance == OCTO){
                        animalPos = new Vector3(8, 5.3f, -11.25f);
                        animalRotation.eulerAngles = new Vector3(animalRotation.eulerAngles.x, 90, animalRotation.eulerAngles.z);
                    } else if (animalToInstance == BIRD){
                        animalPos = new Vector3(8, 5.3f, -11.25f);
                        animalRotation.eulerAngles = new Vector3(animalRotation.eulerAngles.x, 0, animalRotation.eulerAngles.z);
                    } else if (animalToInstance == BEAR){
                        animalPos = new Vector3(8, 5.3f, -11.3f);
                        animalRotation.eulerAngles = new Vector3(-90, 0, animalRotation.eulerAngles.z);
                    } else if (animalToInstance == WHALE){
                        animalPos = new Vector3(8, 5.3f, -11.25f);
                        animalRotation.eulerAngles = new Vector3(animalRotation.eulerAngles.x, 90, animalRotation.eulerAngles.z);
                    }
                    animalModel = Instantiate(animalPrefabs[animalToInstance], animalPos, animalRotation); // 8, 4, -11.25
                    animalModel.transform.parent = player;
                    playercontroller.animator = GameObject.FindGameObjectsWithTag("Animations")[0].GetComponent<Animator>();
                    Time.timeScale = 1f;
            } else if (SceneManager.GetActiveScene().buildIndex == 4){ // city
                    Time.timeScale = 0f;
                    player = GameObject.Find("Player").GetComponent<Transform>();
                    playercontroller = GameObject.Find("Player").GetComponent<ballcontroller>();
                    Vector3 animalPos = new Vector3(8, 4, -11.25f);
                    Quaternion animalRotation = Quaternion.identity;
                    if (animalToInstance == CAT){
                        animalPos = new Vector3(0, 69.3f, -90f);
                        animalRotation.eulerAngles = new Vector3(animalRotation.eulerAngles.x, 90, animalRotation.eulerAngles.z);
                    } else if (animalToInstance == PUFF){
                        animalPos = new Vector3(0, 69.3f, -90f);
                        animalRotation.eulerAngles = new Vector3(animalRotation.eulerAngles.x, 0, animalRotation.eulerAngles.z);
                    } else if (animalToInstance == OCTO){
                        animalPos = new Vector3(0, 69.3f, -90f);
                        animalRotation.eulerAngles = new Vector3(animalRotation.eulerAngles.x, 90, animalRotation.eulerAngles.z);
                    } else if (animalToInstance == BIRD){
                        animalPos = new Vector3(0, 69.3f, -90f);
                        animalRotation.eulerAngles = new Vector3(animalRotation.eulerAngles.x, 0, animalRotation.eulerAngles.z);
                    } else if (animalToInstance == BEAR){
                        animalPos = new Vector3(0, 69.3f, -90.05f);
                        animalRotation.eulerAngles = new Vector3(-90, 0, animalRotation.eulerAngles.z);
                    } else if (animalToInstance == WHALE){
                        animalPos = new Vector3(0, 69.3f, -90f);
                        animalRotation.eulerAngles = new Vector3(animalRotation.eulerAngles.x, 90, animalRotation.eulerAngles.z);
                    }
                    animalModel = Instantiate(animalPrefabs[animalToInstance], animalPos, animalRotation); 
                    playercontroller.animator = GameObject.FindGameObjectsWithTag("Animations")[0].GetComponent<Animator>();
                    animalModel.transform.parent = player;
                    Time.timeScale = 1f;
            } else if (SceneManager.GetActiveScene().buildIndex == 6){ // final
                    Time.timeScale = 0f;
                    player = GameObject.Find("Player").GetComponent<Transform>();
                    playercontroller = GameObject.Find("Player").GetComponent<ballcontroller>();
                    Vector3 animalPos = new Vector3(8, 4, -11.25f);
                    Quaternion animalRotation = Quaternion.identity;
                    if (animalToInstance == CAT){
                        animalPos = new Vector3(0, 499.3f, -45f);
                        animalRotation.eulerAngles = new Vector3(animalRotation.eulerAngles.x, 90, animalRotation.eulerAngles.z);
                    } else if (animalToInstance == PUFF){
                        animalPos = new Vector3(0, 499.3f, -45f);
                        animalRotation.eulerAngles = new Vector3(animalRotation.eulerAngles.x, 0, animalRotation.eulerAngles.z);
                    } else if (animalToInstance == OCTO){
                        animalPos = new Vector3(0, 499.3f, -45f);
                        animalRotation.eulerAngles = new Vector3(animalRotation.eulerAngles.x, 90, animalRotation.eulerAngles.z);
                    } else if (animalToInstance == BIRD){
                        animalPos = new Vector3(0, 499.3f, -45f);
                        animalRotation.eulerAngles = new Vector3(animalRotation.eulerAngles.x, 0, animalRotation.eulerAngles.z);
                    } else if (animalToInstance == BEAR){
                        animalPos = new Vector3(0, 499.3f, -45.05f);
                        animalRotation.eulerAngles = new Vector3(-90, 0, animalRotation.eulerAngles.z);
                    } else if (animalToInstance == WHALE){
                        animalPos = new Vector3(0, 499.3f, -45f);
                        animalRotation.eulerAngles = new Vector3(animalRotation.eulerAngles.x, 90, animalRotation.eulerAngles.z);
                    }
                    animalModel = Instantiate(animalPrefabs[animalToInstance], animalPos, animalRotation); 
                    playercontroller.animator = GameObject.FindGameObjectsWithTag("Animations")[0].GetComponent<Animator>();
                    animalModel.transform.parent = player;
                    Time.timeScale = 1f;
            }
    }
}
