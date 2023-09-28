using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsVisible : MonoBehaviour
{
    public bool Visible
    {
        get; private set;
    }
    private void OnBecameVisible()
    {
        Visible = true;
    }
    private void OnBecameInvisible()
    {
        Visible = false;
    }

}
