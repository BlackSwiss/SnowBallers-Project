using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLayer : MonoBehaviour
{
    public GameObject headShop;

    // Start is called before the first frame update
    void Start()
    {;
        Invoke(nameof(changeLayerInvoke),9);
    }

    private void changeLayerInvoke()
    {
        headShop.layer = 13;
    }
}
