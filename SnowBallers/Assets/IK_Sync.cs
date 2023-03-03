using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class IK_Sync : MonoBehaviourPun, IPunObservable
{
    public Vector3 objectPosition;
    public Quaternion objectRotation;
    public float LerpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // if(!photonView.IsMine)
        // {
        //     Hand hand = GetComponent<Hand>();
        //     hand.SetFollowTarget(gameObject);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if(LerpSpeed == 0)
        {
            LerpSpeed = 1;
        }
        if(!photonView.IsMine)
        {
            UpdateTransform();
        }
    }

    void UpdateTransform()
    {
        transform.position = Vector3.Lerp(transform.position, objectPosition, LerpSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, objectRotation, LerpSpeed * Time.deltaTime);
        // transform.position = objectPosition;
        // transform.rotation = objectRotation;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else if(stream.IsReading)
        {
            objectPosition = (Vector3)stream.ReceiveNext();
            objectRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
