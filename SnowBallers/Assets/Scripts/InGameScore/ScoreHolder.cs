using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHolder : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    [PunRPC]
    public void rebuildLayout()
    {
        StartCoroutine(rebuildLayoutCoroutine());
    }
    IEnumerator rebuildLayoutCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());
    }
}
