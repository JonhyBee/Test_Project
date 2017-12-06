using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ecstasy : Item
{
  public override ItemType itemType  { get { return ItemType.Ecstasy; }}
  public override string DisplayName { get { return "MDMA"; } }

  public Ecstasy()
  {
    maxStackSize = 5;
  }

  public override void Use()
  {
    Debug.Log("I just ate a pil of " + DisplayName + ". Hang on tight, the wild ride is starting!");

    //Remove 1 Item in the Slot here...


    if (GameObject.FindObjectOfType<Player>().inventory.Contain(ItemType.Ecstasy))
    {
      Debug.Log("And I still have a shit load. =)");
    }

  }

  public override List<string> GenerateActionList()
  {
    return new List<string>() { "Pick-Up", "Use", "Inspect" };
  }


}
