using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Filter : MonoBehaviour
{
    public enum FilterOn
    {
        None,
        Selected,
        Unselected
    }

    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean menuButton;

    private FilterOn curFilter = FilterOn.None;

    /*
    // Start is called before the first frame update
    void Start() { }
    */

    // Update is called once per frame
    void Update() 
    {
        if(GetMenuButton())
        {
            if(curFilter == FilterOn.None)
            {
                curFilter = FilterOn.Selected;
            }
            else
            {
                curFilter = FilterOn.None;
            }

            FilterVoxels(curFilter);
        }
    }

    public void FilterVoxels(FilterOn mode)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);

            bool selected = false;

            switch (mode)
            {
                case FilterOn.None:
                    child.gameObject.SetActive(true);
                    break;

                case FilterOn.Selected:
                    selected = child.GetComponent<OutlineController>().GetSelected();
                    child.gameObject.SetActive(selected);
                    break;

                case FilterOn.Unselected:
                    selected = child.GetComponent<OutlineController>().GetSelected();
                    child.gameObject.SetActive(!selected);
                    break;

                default:
                    break;
            }
                
        }
    }

    private bool GetMenuButton()
    {
        return menuButton.GetStateDown(handType);
    }
}