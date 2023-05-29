using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComicManagerWithCharacterSelection : MonoBehaviour
{ 
    private int current_panel;
    [SerializeField] GameObject[] Panels;
    [SerializeField] GameObject[] PanelsC;
    [SerializeField] GameObject[] PanelsP;
    [SerializeField] GameObject[] PanelsO;
    [SerializeField] GameObject[] PanelsBi;
    [SerializeField] GameObject[] PanelsBe;
    [SerializeField] GameObject[] PanelsW;
    [SerializeField] GameObject All_Panels;
    [SerializeField] GameObject Cat_Panels;
    [SerializeField] GameObject Puff_Panels;
    [SerializeField] GameObject Octo_Panels;
    [SerializeField] GameObject Bird_Panels;
    [SerializeField] GameObject Bear_Panels;
    [SerializeField] GameObject Whale_Panels;
    [SerializeField] GameObject[] Selected_Panels;
    private int total_panels = 0;
    private int total_panels_selected = 0;
    public bool is_selected;
    private int selected;
    [SerializeField] GameObject character_selection;

    // Start is called before the first frame update
    void Start()
    {
        is_selected = false;
        total_panels = All_Panels.transform.childCount;
        Panels = new GameObject[total_panels];

        for (int i = 0; i < total_panels; i++)
        {
            Panels[i] = All_Panels.transform.GetChild(i).gameObject;

        }

        total_panels_selected = Cat_Panels.transform.childCount;
        Selected_Panels = new GameObject[total_panels_selected];
        PanelsC = new GameObject[total_panels_selected];
        PanelsP = new GameObject[total_panels_selected];
        PanelsO = new GameObject[total_panels_selected];
        PanelsBi = new GameObject[total_panels_selected];
        PanelsBe = new GameObject[total_panels_selected];
        PanelsW = new GameObject[total_panels_selected];

        for (int i = 0; i < total_panels_selected; i++)
        {
            PanelsC[i] = Cat_Panels.transform.GetChild(i).gameObject;
            PanelsP[i] = Puff_Panels.transform.GetChild(i).gameObject;
            PanelsO[i] = Octo_Panels.transform.GetChild(i).gameObject;
            PanelsBi[i] = Bird_Panels.transform.GetChild(i).gameObject;
            PanelsBe[i] = Bear_Panels.transform.GetChild(i).gameObject;
            PanelsW[i] = Whale_Panels.transform.GetChild(i).gameObject;
        }

        current_panel = 0;
        Panels[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (((Input.GetKeyDown(KeyCode.Space)) || (Input.GetMouseButtonDown(0))) && !is_selected)
        {
            AdvanceComic();
        }

        else if (((Input.GetKeyDown(KeyCode.Space)) || (Input.GetMouseButtonDown(0))) && is_selected)
        {
            AdvanceComic2();
        }
        Debug.Log(current_panel+"yatta");
        selected = animalStore.animal;
    }

    private void AdvanceComic()
    {
        if (total_panels > current_panel + 1)
        {
            if(current_panel == 0)
            {
                character_selection.SetActive(true);
            }

            Panels[current_panel].SetActive(false);
            Panels[current_panel + 1].SetActive(true);
            current_panel++;
        }
        else if (total_panels <= current_panel + 1)
        {
            selected = animalStore.animal;
            switch (selected)
            {
                case 0:
                    Selected_Panels[0] = PanelsC[0];
                    Selected_Panels[1] = PanelsC[1];
                    Selected_Panels[2] = PanelsC[2];
                    Selected_Panels[3] = PanelsC[3];
                    is_selected = true;
                    break;
                case 1:
                    Selected_Panels[0] = PanelsP[0];
                    Selected_Panels[1] = PanelsP[1];
                    Selected_Panels[2] = PanelsP[2];
                    Selected_Panels[3] = PanelsP[3];
                    is_selected = true;
                    break;
                case 2:
                    Selected_Panels[0] = PanelsO[0];
                    Selected_Panels[1] = PanelsO[1];
                    Selected_Panels[2] = PanelsO[2];
                    Selected_Panels[3] = PanelsO[3];
                    is_selected = true;
                    break;
                case 3:
                    Selected_Panels[0] = PanelsBi[0];
                    Selected_Panels[1] = PanelsBi[1];
                    Selected_Panels[2] = PanelsBi[2];
                    Selected_Panels[3] = PanelsBi[3];
                    is_selected = true;
                    break;
                case 4:
                    Selected_Panels[0] = PanelsBe[0];
                    Selected_Panels[1] = PanelsBe[1];
                    Selected_Panels[2] = PanelsBe[2];
                    Selected_Panels[3] = PanelsBe[3];
                    is_selected = true;
                    break;
                case 5:
                    Selected_Panels[0] = PanelsW[0];
                    Selected_Panels[1] = PanelsW[1];
                    Selected_Panels[2] = PanelsW[2];
                    Selected_Panels[3] = PanelsW[3];
                    is_selected = true;
                    break;
                default:
                    Selected_Panels[0] = PanelsC[0];
                    Selected_Panels[1] = PanelsC[1];
                    Selected_Panels[2] = PanelsC[2];
                    Selected_Panels[3] = PanelsC[3];
                    break;
            }

            if (is_selected)
            {
                Panels[current_panel].SetActive(false);
                current_panel = 0;
                total_panels_selected = 4;
                Selected_Panels[0].SetActive(true);

                character_selection.SetActive(false);
            }
        }

    }
    private void AdvanceComic2()
    {
        if (total_panels_selected > current_panel + 1)
        {
            Selected_Panels[current_panel].SetActive(false);
            Selected_Panels[current_panel + 1].SetActive(true);
            current_panel++;
        }
        else
        {
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}