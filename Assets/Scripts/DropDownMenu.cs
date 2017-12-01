using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownMenu : MonoBehaviour {

    public Dropdown dropdown;
    public Inventory inventory;
    public ItemType type;
    public List<string> ActionList;


    private void Start()
    {
        PopulateList();

    }

    void PopulateList()
    {
        //TODO fetch the interactions types from the item library/context

        List<string> names = ActionList;
        dropdown.AddOptions(names);

    }

}
