using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComicManager : MonoBehaviour
{
    private int current_panel;
    [SerializeField] GameObject[] Panels;
    [SerializeField] GameObject All_Panels;
    private int total_panels = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(animalStore.animal == 0)
        {
            All_Panels = GameObject.Find("cat_images");
        }
        total_panels = All_Panels.transform.childCount;
        Panels = new GameObject[total_panels];
        for(int i = 0; i < total_panels; i++)
        {
            Panels[i] = All_Panels.transform.GetChild(i).gameObject;
        }

        current_panel = 0;
        Panels[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space))|| (Input.GetMouseButtonDown(0)))
        {
            AdvanceComic();
        }
    }

    private void AdvanceComic()
    {
        if(total_panels > current_panel + 1)
        {
            Panels[current_panel].SetActive(false);
            Panels[current_panel + 1].SetActive(true);
            current_panel++;
        }
        else
        {
            Debug.Log(SceneManager.GetActiveScene().buildIndex + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
