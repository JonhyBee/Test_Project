﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hashish : Item
{
  public override ItemType itemType  { get { return ItemType.Hashish; }}
  public override string DisplayName { get { return "Weed"; } }

  public Hashish()
  {
    maxStackSize = 5;
  }

  public override void Use()
  {
    Debug.Log("I just ate a gram of " + DisplayName + ", it doesn't taste very good.");

    //Remove 1 Item in the Slot here...


    if (GameObject.FindObjectOfType<Player>().inventory.Contain(ItemType.Hashish))
    {
      Debug.Log("And I still have a shit load. =)");
    }

  }

  public override List<string> GenerateActionList()
  {
    return new List<string>() { "Pick-Up", "Use", "Inspect" };
  }


}