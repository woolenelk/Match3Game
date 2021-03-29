using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    public TileValueScriptableObj tilevalue;
    Renderer renderer;
    BoardManager boardManager;
    [SerializeField]
    public bool selected = false;
    [SerializeField]
    public bool inPlace = false;
    public Vector3 position;
    public bool matched = false;
    Animator animator;

    private void Start()
    {
        boardManager = BoardManager.instance;
        renderer = GetComponent<Renderer>();
        //position = transform.position;
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        animator.SetBool("selected", selected);
        
        if (Vector3.Distance(transform.position, position)<=0.02)
        {
            //Debug.Log(transform.position + "  to   " + position);
            transform.position = position;
            inPlace = true;
        }
        else
        {
            inPlace = false;
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, position.x, Time.deltaTime*5),
                                             Mathf.Lerp(transform.position.y, position.y, Time.deltaTime*5),
                                             Mathf.Lerp(transform.position.z, position.z, Time.deltaTime*5));
        }

        if (tilevalue != null)
            renderer.material.color = tilevalue.color;
        else
            renderer.material.color = new Color(0, 0, 0, 0);
    }

    void OnMouseDown()
    {
        if (tilevalue == null || BoardManager.instance.IsShifting || BoardManager.instance.IsChecking)
        {
            return;
        }
        boardManager.SelectTile(gameObject);
    }
}
