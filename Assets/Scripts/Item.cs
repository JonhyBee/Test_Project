using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType {DimeBag}

public class Item : MonoBehaviour 

{
    public Renderer Inv { get; set; }
    
    public Inventory inventory;

    public ItemType type;

    public List<string> ActionList;

    public Sprite spriteNeutral;

    public Sprite spriteHighlighted;

    public int maxSize; //Max stack size

    public void Use()
    {
        switch (type)
        {
            case ItemType.DimeBag:
                Debug.Log("I just ate a gram of weed, it doesn't taste very good.");
                break;
            default:
                break;
        }

    }

    public List<string> GenerateActionList()
    {
        switch (type)
        {
            case ItemType.DimeBag:
                ActionList = new List<string>() { "Pick-Up", "Use", "Inspect" };
                break;
            default:
                break;
        }

        return ActionList;
    }


    void OnMouseOver()
    {
        Component halo = GetComponent("Halo");
        halo.GetType().GetProperty("enabled").SetValue(halo, true, null);

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("OpenDropDownMenu");
        }
    }

    void OnMouseExit()
    {

        Component halo = GetComponent("Halo");
        halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
        
    }

 
}
