using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    public ComicManagerWithCharacterSelection comic_manager;
    private GameObject character_selection;

    private void Start()
    {
        comic_manager = GameObject.Find("Render Comic").GetComponent<ComicManagerWithCharacterSelection>();
        character_selection = this.gameObject;
    }
    public void cat()
    {
     //   animalStore.animal = 0;
        comic_manager.characterPanel = 0;
        characterTracker.animalToInstance = 0;
        updateComic();
        Debug.Log("0");
        character_selection.SetActive(false);
    }
    public void puff()
    {
    //    animalStore.animal = 1;
        comic_manager.characterPanel = 1;
        characterTracker.animalToInstance = 1;
        updateComic();
        Debug.Log("1");
        character_selection.SetActive(false);
    }
    public void octo()
    {
    //    animalStore.animal = 2;
        comic_manager.characterPanel = 2;
        characterTracker.animalToInstance = 2;
        updateComic();
        Debug.Log("2");
        character_selection.SetActive(false);
    }
    public void bird()
    {
    //    animalStore.animal = 3;
        comic_manager.characterPanel = 3;
        characterTracker.animalToInstance = 3;
        updateComic();
        Debug.Log("3");
        character_selection.SetActive(false);
    }
    public void bear()
    {
     //   animalStore.animal = 4;
        comic_manager.characterPanel = 4;
        characterTracker.animalToInstance = 4;
        updateComic();
         Debug.Log("4");
        character_selection.SetActive(false);
    }
    public void whale()
    {
     //   animalStore.animal = 5;
        comic_manager.characterPanel = 5;
        characterTracker.animalToInstance = 5;
        updateComic();
        Debug.Log("5");
        character_selection.SetActive(false);
    }

    private void updateComic(){
        comic_manager.AdvanceComic();
        comic_manager.is_selected = true;
        comic_manager.Panels[comic_manager.current_panel].SetActive(false);
        comic_manager.current_panel = 0;
        comic_manager.total_panels_selected = 4;
        comic_manager.Selected_Panels[0].SetActive(true);
    }

}
