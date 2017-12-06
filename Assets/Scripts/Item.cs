using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType { DimeBag,Coke,Heroin,Hashish,Ecstasy }

public abstract class Item : MonoBehaviour
{
  public abstract ItemType itemType { get; }
  public abstract string DisplayName { get; }

  public abstract void Use();
  public abstract List<string> GenerateActionList();

  public int maxStackSize = 1;

  public Renderer Inv { get; set; }

  public Sprite spriteNeutral;
  public Sprite spriteHighlighted;
  
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

