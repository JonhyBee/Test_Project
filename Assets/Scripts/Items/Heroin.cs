using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heroin : Item
{
  public override ItemType itemType  { get { return ItemType.DimeBag; }}
  public override string DisplayName { get { return "Uncooked Heroin"; } }

  public Heroin()
  {
    maxStackSize = 5;
  }

  public override void Use()
  {
    Debug.Log("I just ate a gram of " + DisplayName + ", what a waste...");

    //Remove 1 Item in the Slot here...


    if (GameObject.FindObjectOfType<Player>().inventory.Contain(ItemType.DimeBag))
    {
      Debug.Log("And I still have a shit load. =)");
    }

  }

  public override List<string> GenerateActionList()
  {
    return new List<string>() { "Pick-Up", "Use", "Inspect" };
  }


}
