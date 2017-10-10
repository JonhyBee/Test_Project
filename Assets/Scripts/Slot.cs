using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler {

    private Stack<Item> items;

    public Text stackTxt;

    public Sprite slotEmpty;
    public Sprite slotHighlight;

    public bool IsEmpty
    {
        get { return Items.Count == 0; }
    }

    public bool IsAvailable
    {
        get { return CurrentItem.maxSize > Items.Count; }
    }

    public Item CurrentItem
    {
        get { return Items.Peek(); }
    }

    public Stack<Item> Items
    {
        get
        {
            return items;
        }

        set
        {
            items = value;
        }
    }

    // Use this for initialization
    void Start ()
    {
        Items = new Stack<Item>();
        RectTransform slotRect = GetComponent<RectTransform>();
        RectTransform txtRect = stackTxt.GetComponent<RectTransform>();

        int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.60);
        stackTxt.resizeTextMaxSize = txtScaleFactor;
        stackTxt.resizeTextMinSize = txtScaleFactor;

        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);

    }
	

	// Update is called once per frame
	void Update ()
    {
		
	}

    public void AddItem(Item item)
    {
        Items.Push(item);

        if (Items.Count > 1)
        {
            stackTxt.text = Items.Count.ToString();
        }

        ChangeSprite(item.spriteNeutral, item.spriteHighlighted);

    }

    public void UpdateSprite()
    {
        ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted);
    }


    public void AddItems(Stack<Item> items)
    {
        this.Items = new Stack<Item>(items);

        stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

        ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted);
    }

    private void ChangeSprite(Sprite neutral, Sprite highlight)
    {
        GetComponent<Image>().sprite = neutral;

        SpriteState st = new SpriteState();

        st.highlightedSprite = highlight;
        st.pressedSprite = neutral;

        GetComponent<Button>().spriteState = st;
    }

    private void UseItem()
    {
        if (!IsEmpty)
        {
            Items.Pop().Use();

            stackTxt.text = Items.Count > 1 ? Items.Count.ToString() : string.Empty;

            if (IsEmpty)
            {
                ChangeSprite(slotEmpty, slotHighlight);
                Inventory.EmptySlots++;
            }
        }
    }

    public void ClearSlot()
    {
        items.Clear();
        ChangeSprite(slotEmpty, slotHighlight);
        stackTxt.text = string.Empty;
    }

    public Stack<Item> RemoveItems(int amount)
    {
        Stack<Item> tmp = new Stack<Item>();

        for (int i = 0; i < amount; i++)
        {
            tmp.Push(items.Pop());
        }

        stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

        return tmp;
    }

    public Item RemoveItem()
    {
        Item tmp;

        tmp = items.Pop();

        stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

        return tmp;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover") && Inventory.CanvasGroup.alpha > 0)
        {
            UseItem();
        }

        else if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift) && !IsEmpty && !GameObject.Find("Hover")) //shift left-click with something in the slot 
        {
            Debug.Log("TESTTING ONE TWO TESTING");
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Inventory.Instance.canvas.transform as RectTransform, Input.mousePosition, Inventory.Instance.canvas.worldCamera, out position);

            Inventory.Instance.selectStackSize.SetActive(true);
            Inventory.Instance.selectStackSize.transform.position = Inventory.Instance.canvas.transform.TransformPoint(position);
            Inventory.Instance.selectStackSize.transform.Translate(Vector2.right * 50);
            Inventory.Instance.selectStackSize.transform.SetAsLastSibling(); //bring the new menu to the front

            Inventory.Instance.SetStackInfo(items.Count); //telling the inventory we have X item on this slot
        }
    }
}
