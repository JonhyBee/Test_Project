using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    private RectTransform inventoryRect;

    private float inventoryWidth, inventoryHight;

    public int slots;
    public int rows;
    
    public float slotPaddingLeft;
    public float slotPaddingTop;

    public float slotSize;

    public GameObject slotPrefab;

    private static Slot from, to;

    private List<GameObject> allSlots;

    public GameObject iconPrefab;

    private static GameObject hoverObject;

    private static int emptySlots;

    public Canvas canvas;

    private static CanvasGroup canvasGroup;

    private bool fadingIn;
    private bool fadingOut;

    public float fadeTime;

    private static Inventory instance;

    private static GameObject clicked;

    //some stuff to split stack size
    public GameObject selectStackSize;
    public Text stackText;
    private int splitAmount;
    private int maxStackCount;
    private static Slot movingSlot; //tmp slot for use only when splitting stacks

    private float hoverYOffset;

    public EventSystem eventSystem;

    public static int EmptySlots
    {
        get { return emptySlots; }
        set { emptySlots = value; }
    }

    public static CanvasGroup CanvasGroup
    {
        get{return Inventory.canvasGroup;}
    }

    //Cree une instance the l'inventaire pour pouvoir acceder a ces proprieter a partir de d'autres classes
    public static Inventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Inventory>();
            }
            return Inventory.instance;
        }

    }

    // Use this for initialization
    void Start () 
    {

        canvasGroup = transform.parent.GetComponent<CanvasGroup>();

        CreateLayout();

        movingSlot = GameObject.Find("MovingSlot").GetComponent<Slot>();

    }

    // Called once per Frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //check if the user just lifted the first mouse botton
        {
            //Remove the selected item from the inventory
            if (!eventSystem.IsPointerOverGameObject(-1) && from != null) //if we click outside the inventory
            {
                from.GetComponent<Image>().color = Color.white; //reset the slots color
                from.ClearSlot(); //remove items from the slots
                Destroy(GameObject.Find("Hover"));  //remove the hover icon
                to = null;
                from = null;
                
                emptySlots++;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {

            ToggleVisibility();

        }

        if (hoverObject != null) //check is hover object exist
        {
            //position of hover object
            Vector2 position;
            //translate the current screen mouse position into a local position and store it in the position 
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            // Add the offset (from Unity GUI) to the position
            position.Set(position.x, position.y - hoverYOffset);
            // Sets the hoverObject position
            hoverObject.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    // Use the FadeIn FadeOut Routines to hide/show the inventory
    void ToggleVisibility() 
    {
        
        //summon up the renderer for our Main canvas (where the inventory is drawn)
        CanvasRenderer InvRend = GetComponent<CanvasRenderer>();

        //check if inventory is currently open, its present in the frame by default at execution right now
        if (CanvasGroup.alpha > 0)
        {
            //if it was open we close it
            StartCoroutine("FadeOut");
            PutItemBack();
        }

        else
        {
            StartCoroutine("FadeIn");
        }
    }

    // Place all the Slots in the Inventory Window
    private void CreateLayout()
    
    {
        allSlots = new List<GameObject>();

        hoverYOffset = slotSize * 0.01f;

        emptySlots = slots;

        inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
        inventoryHight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

        inventoryRect = GetComponent<RectTransform>();

        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHight);

        //Find the amount of Column based on Unity GUI selected slots and row values

        int columns = slots / rows;

        for (int y = 0; y < rows; y++)//Runs through the rows
        {
            for (int x = 0; x < columns; x++)//Runs through the rows
            {
                GameObject newSlot = (GameObject)Instantiate(slotPrefab);

                RectTransform slotRect = newSlot.GetComponent<RectTransform>();

                newSlot.name = "Slot";

                newSlot.transform.SetParent(this.transform.parent);

                slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));

                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);

                allSlots.Add(newSlot);

            } 

        }  
    }

    // Add an item (from outside the inventory, doest include moving items around) to the inventory
    public bool AddItem(Item item)
    {
        if (item.maxSize == 1) //if the item isn't stackable
        {
            //Places the item an empty slot
            PlaceEmpty(item);
            return true;
        }
        else //If the item is stackable
        {
            foreach (GameObject slot in allSlots) // Runs through all the slots in the inventory
            {
                Slot tmp = slot.GetComponent<Slot>(); // Create a reference to the slot

                if (!tmp.IsEmpty) //If the item isn't empty
                {
                    //check if the item in the slot is the same as the item we want to pick up
                    if (tmp.CurrentItem.type == item.type && tmp.IsAvailable)
                    {
                        tmp.AddItem(item); //add the item to the incentory
                        return true;
                    }
                }
            }
            if (emptySlots > 0) // Place the item on an empty slot
            {
                PlaceEmpty(item);
            }
        }

        return false;
    }

    //Place an item in an empty slot
    private bool PlaceEmpty(Item item)
    {
        if (emptySlots > 0) //if we have at least one empty slots
        {
            foreach (GameObject slot in allSlots) //Runs through all slots
            {
                Slot tmp = slot.GetComponent<Slot>(); //Create a reference to the slot

                if (tmp.IsEmpty) //If the slot is empty
                {
                    tmp.AddItem(item); //Adds the item
                    emptySlots--; //reduce the number of empty slots
                    return true;
                }
            }
        }

        return false;
    }

    // Move an item to another slot in the inventory
    public void MoveItem(GameObject clicked) //apply on the gameobject we just clicked
    {

        Inventory.clicked = clicked;

        if (!movingSlot.IsEmpty) //this code is run whenever we split a stack and click en item in inventory
        {
            Slot tmp = clicked.GetComponent<Slot>(); //ref to the slot

            if (tmp.IsEmpty) // if its empty place all of our items there
            {
                tmp.AddItems(movingSlot.Items);
                movingSlot.Items.Clear();
                Destroy(GameObject.Find("Hover"));
            }
            else if (!tmp.IsEmpty && movingSlot.Items.Peek().type == tmp.CurrentItem.type && tmp.IsAvailable) //then we can merge the stacks
            {
                MergeStacks(movingSlot, tmp);
            }
        }

        else if (from == null && canvasGroup.alpha == 1 && !Input.GetKey(KeyCode.LeftShift)) //If we have not already picked up something and not splitting this stack
        {
            if (!clicked.GetComponent<Slot>().IsEmpty && !GameObject.Find("Hover")) //if the slot we clicked is NOT empty
            {
                from = clicked.GetComponent<Slot>(); //the slot we are moving from
                from.GetComponent<Image>().color = Color.gray; //make it greyish so it is marked as >we are moving from

                CreateHoverIcon();
            }
        }
        else if (to == null && !Input.GetKey(KeyCode.LeftShift)) //we have clicked the slot we are moving to
        {
            to = clicked.GetComponent<Slot>(); //place the object in the slot
            Destroy(GameObject.Find("Hover")); //destroy the hover object
        }
        if (to != null && from != null) // If to and from are null we are done moving stuff
        {
            if (!to.IsEmpty && from.CurrentItem.type == to.CurrentItem.type && to.IsAvailable) 
            {
                MergeStacks(from, to);
            }

            Stack<Item> tmpTo = new Stack<Item>(to.Items);
            to.AddItems(from.Items);

            if (tmpTo.Count == 0)
            {
                from.ClearSlot();
            }
            else
            {
                from.AddItems(tmpTo);
            }

            from.GetComponent<Image>().color = Color.white;
            to = null;
            from = null;
            Destroy(GameObject.Find("Hover"));
        }
    }

    private void CreateHoverIcon()
    {

        hoverObject = (GameObject)Instantiate(iconPrefab); // Instantiate the hover object
        hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite; // Sets the sprite on the hover object so it reflect the object we picked up
        hoverObject.name = "Hover"; // set the name of this new object in Unity

        //Reference to the transform
        RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
        RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

        //Sets the size of the hover Object so that it has the same size as the object we clicked
        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

        //
        hoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);
        hoverObject.transform.localScale = clicked.gameObject.transform.localScale;

        hoverObject.transform.GetChild(0).GetComponent<Text>().text = movingSlot.Items.Count > 1 ? movingSlot.Items.Count.ToString() : string.Empty;

    }

    private void PutItemBack()
    {
        if (from != null)
        {
            Destroy(GameObject.Find("Hover"));
            from.GetComponent<Image>().color = Color.white;
            from = null;
        }
    }

    public void SetStackInfo(int maxStackCount)
    {
        selectStackSize.SetActive(true);
        splitAmount = 0;
        this.maxStackCount = maxStackCount;
        stackText.text = splitAmount.ToString();
    }

    public void SplitStack() //Called when I press OK on the split stack GUI
    {
        selectStackSize.SetActive(false); //hides the split stack GUI

        if (splitAmount == maxStackCount) //then we are just picking up all of them
        {
            MoveItem(clicked);
        }

        else if (splitAmount > 0) //cant split 0 items
        {
            movingSlot.Items = clicked.GetComponent<Slot>().RemoveItems(splitAmount);

            CreateHoverIcon();
        }
    }

    public void ChangeStackText(int i)
    {
        splitAmount += i;
        if (splitAmount < 0)
        {
            splitAmount = 0;
        }
        if (splitAmount > maxStackCount)
        {
            splitAmount = maxStackCount;
        }

        stackText.text = splitAmount.ToString();
    }

    public void MergeStacks(Slot source, Slot destination)
    {
        //calculate the max amount we can stack for the item

        int max = destination.CurrentItem.maxSize - destination.Items.Count;

        int count = source.Items.Count < max ? source.Items.Count : max; //make sure we done overflow items (go over max amount)

        for (int i = 0; i < count; i++)
        {
            destination.AddItem(source.RemoveItem());
        }
        if (source.Items.Count == 0)
        {
            source.ClearSlot();
            Destroy(GameObject.Find("Hover"));
        }
    }


    private IEnumerator FadeOut()
    {
        if (!fadingOut)
        {
            fadingOut = true;
            fadingIn = false;
            StopCoroutine("FadeIn");

            float startAlpha = CanvasGroup.alpha;

            float rate = 1.0f / fadeTime;

            float progress = 0.0f;

            while (progress < 1.0)
            {
                CanvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);
                progress += rate * Time.deltaTime;
                yield return null; //this runs constantly and doest return anything
            }

            CanvasGroup.alpha = 0;
            fadingOut = false;
        }
    }

    private IEnumerator FadeIn()
    {
        if (!fadingOut)
        {
            fadingOut = false;
            fadingIn = true;
            StopCoroutine("FadeOut");

            float startAlpha = CanvasGroup.alpha;

            float rate = 1.0f / fadeTime;

            float progress = 0.0f;

            while (progress < 1.0)
            {
                CanvasGroup.alpha = Mathf.Lerp(startAlpha, 1, progress); //lerping??? apparament sa marche
                progress += rate * Time.deltaTime;
                yield return null; //this runs constantly and doest return anything
            }

            CanvasGroup.alpha = 1;
            fadingIn = false;
        }
    }
}
