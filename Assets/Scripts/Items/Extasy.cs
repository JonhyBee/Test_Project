using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Extasy : Item
{
  public override ItemType itemType  { get { return ItemType.Extasy; }}
  public override string DisplayName { get { return "Extasy"; } }

  public override void Use()
  {
    Debug.Log("I just ate a gram of " + DisplayName + ", I feel great");
  }

  public override List<string> GenerateActionList()
  {
    return new List<string>() { "Pick-Up", "Use", "Inspect" };
  }


}
