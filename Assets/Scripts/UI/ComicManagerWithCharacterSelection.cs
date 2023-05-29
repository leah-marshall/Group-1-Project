using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComicManagerWithCharacterSelection : MonoBehaviour
{ 
    public int current_panel;
    public GameObject[] Panels;
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
    public GameObject[] Selected_Panels;
    private int total_panels = 0;
    public int total_panels_selected = 0;
    public bool is_selected;
    static public int characterPanel;
    [SerializeField] GameObject character_selection;
    private int CAT = 0;
    private int PUFF = 1;
    private int OCTO = 2;
    private int BIRD = 3;
    private int BEAR = 4;
    private int WHALE = 5;
    private loadingScreen loadManager;
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
        loadManager = GameObject.Find("LoadingManager").GetComponent<loadingScreen>();
        updateCharacterPanel();
    }

    // Update is called once per frame
    void Update()
    {
        if (((Input.GetKeyDown(KeyCode.Space)) || (Input.GetMouseButtonDown(0))) && !is_selected)
        {
            updateCharacterPanel();
            AdvanceComic();
        }

        else if (((Input.GetKeyDown(KeyCode.Space)) || (Input.GetMouseButtonDown(0))) && is_selected)
        {
            updateCharacterPanel();
            AdvanceComic2();
        }
   //     Debug.Log(current_panel+"yatta");
    }

    void updateCharacterPanel(){
        if (characterPanel == CAT){
                Selected_Panels[0] = PanelsC[0];
                Selected_Panels[1] = PanelsC[1];
                Selected_Panels[2] = PanelsC[2];
                Selected_Panels[3] = PanelsC[3];
                Debug.Log("displaying cat");
            } else if (characterPanel == PUFF){
                Selected_Panels[0] = PanelsP[0];
                Selected_Panels[1] = PanelsP[1];
                Selected_Panels[2] = PanelsP[2];
                Selected_Panels[3] = PanelsP[3];
                Debug.Log("puffer");
            } else if (characterPanel == OCTO){
                Selected_Panels[0] = PanelsO[0];
                Selected_Panels[1] = PanelsO[1];
                Selected_Panels[2] = PanelsO[2];
                Selected_Panels[3] = PanelsO[3];
                Debug.Log("octo");
            } else if (characterPanel == BIRD){
                Selected_Panels[0] = PanelsBi[0];
                Selected_Panels[1] = PanelsBi[1];
                Selected_Panels[2] = PanelsBi[2];
                Selected_Panels[3] = PanelsBi[3];
                Debug.Log("bird");
            } else if (characterPanel == BEAR){
                Selected_Panels[0] = PanelsBe[0];
                Selected_Panels[1] = PanelsBe[1];
                Selected_Panels[2] = PanelsBe[2];
                Selected_Panels[3] = PanelsBe[3];
                Debug.Log("bear");
            } else if (characterPanel == WHALE){
                Selected_Panels[0] = PanelsW[0];
                Selected_Panels[1] = PanelsW[1];
                Selected_Panels[2] = PanelsW[2];
                Selected_Panels[3] = PanelsW[3];
                Debug.Log("displaying whale");
            }
    }

    public void AdvanceComic()
    {
        Debug.Log("selected: ");
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
            updateCharacterPanel();
            /*
            switch (selected)
            {
                case 0:
                    Selected_Panels[0] = PanelsC[0];
                    Selected_Panels[1] = PanelsC[1];
                    Selected_Panels[2] = PanelsC[2];
                    Selected_Panels[3] = PanelsC[3];
                    is_selected = true;
                    Debug.Log("displaying cat");
                    break;
                case 1:
                    Selected_Panels[0] = PanelsP[0];
                    Selected_Panels[1] = PanelsP[1];
                    Selected_Panels[2] = PanelsP[2];
                    Selected_Panels[3] = PanelsP[3];
                    is_selected = true;
                     Debug.Log("puffer");
                    break;
                case 2:
                    Selected_Panels[0] = PanelsO[0];
                    Selected_Panels[1] = PanelsO[1];
                    Selected_Panels[2] = PanelsO[2];
                    Selected_Panels[3] = PanelsO[3];
                    is_selected = true;
                     Debug.Log("octo");
                    break;
                case 3:
                    Selected_Panels[0] = PanelsBi[0];
                    Selected_Panels[1] = PanelsBi[1];
                    Selected_Panels[2] = PanelsBi[2];
                    Selected_Panels[3] = PanelsBi[3];
                     Debug.Log("bird");
                    is_selected = true;
                    break;
                case 4:
                    Selected_Panels[0] = PanelsBe[0];
                    Selected_Panels[1] = PanelsBe[1];
                    Selected_Panels[2] = PanelsBe[2];
                    Selected_Panels[3] = PanelsBe[3];
                    is_selected = true;
                     Debug.Log("bear");
                    break;
                case 5:
                    Selected_Panels[0] = PanelsW[0];
                    Selected_Panels[1] = PanelsW[1];
                    Selected_Panels[2] = PanelsW[2];
                    Selected_Panels[3] = PanelsW[3];
                     Debug.Log("displaying whale");
                    is_selected = true;
                    break;
                default:
                    Selected_Panels[0] = PanelsC[0];
                    Selected_Panels[1] = PanelsC[1];
                    Selected_Panels[2] = PanelsC[2];
                    Selected_Panels[3] = PanelsC[3];
                     Debug.Log("displaying cat by default");
                    break;
            }
            */
            /*
            if (is_selected)
            {
                Panels[current_panel].SetActive(false);
                current_panel = 0;
                total_panels_selected = 4;
                Selected_Panels[0].SetActive(true);
            }
            */
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
           loadManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
        }
    }
}