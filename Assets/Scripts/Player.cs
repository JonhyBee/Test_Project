using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

  Animator a;
  public float speed;
  bool right;

  public Inventory inventory { get; set; }

  void Start()
  {
    a = GetComponent<Animator>();
  }

  void Flip()
  {
    Vector3 scale = transform.localScale;
    scale.x *= -1;
    transform.localScale = scale;
    right = !right;
  }

  // Update is called once per frame
  void Update()

  {
    HandleMovement();
  }

  private void HandleMovement()
  {
    a.SetBool("Walking", false);
    if (Input.GetKey(KeyCode.D))
    {
      if (right == false)
      {
        Flip();
      }
      a.SetBool("Walking", true);
      transform.Translate(Vector2.right * speed);
    }

    if (Input.GetKey(KeyCode.A))
    {
      if (right == true)
      {
        Flip();
      }
      a.SetBool("Walking", true);
      transform.Translate(Vector2.left * speed);
    }

    if (Input.GetKey(KeyCode.W))
    {
      a.SetBool("Walking", true);
      transform.Translate(Vector2.up * speed);
    }

    if (Input.GetKey(KeyCode.S))
    {
      a.SetBool("Walking", true);
      transform.Translate(Vector2.down * speed);
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Item")
    {
      inventory.AddItem(other.GetComponent<Item>());
    }
  }
}