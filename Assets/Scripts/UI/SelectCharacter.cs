using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    public ComicManagerWithCharacterSelection comic_manager;

    private void Start()
    {
        comic_manager = GameObject.Find("Render Comic").GetComponent<ComicManagerWithCharacterSelection>();
    }
    public void cat()
    {
        animalStore.animal = 0;
        Debug.Log("0");
    }
    public void puff()
    {
        animalStore.animal = 1;
        Debug.Log("1");
    }
    public void octo()
    {
        animalStore.animal = 2;
        Debug.Log("2");
    }
    public void bird()
    {
        animalStore.animal = 3;
        Debug.Log("3");
    }
    public void bear()
    {
        animalStore.animal = 4;
        Debug.Log("4");
    }
    public void whale()
    {
        animalStore.animal = 5;
        Debug.Log("5");
    }

}
