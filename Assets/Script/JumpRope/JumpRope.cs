using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class JumpRope : MonoBehaviour
{
    private static JumpRope instance;
    public static JumpRope Instance
    {
        get { if (instance == null)  instance=FindObjectOfType<JumpRope>(); return instance; }
    }
    [SerializeField]private GameObject centerPos = null;
    [SerializeField] private GameObject rope = null;
    [SerializeField] private float ropeSpeed = 100f;
    private Vector3 startPos = Vector3.zero;
    private bool isRotation = false;
    public Text[] texts = new Text[2];
    public Text countDownText = null;
    private PhotonView pv = null;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        startPos = rope.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        RotationRope();
       
    }
    private void RotationRope()
    {
        if(isRotation)
        {
            rope.transform.RotateAround(centerPos.transform.position, Vector3.left, ropeSpeed * Time.deltaTime);
        }
    }
    public void StopRotationRope()//ÁÙ È¸ÀüÀ» ¸ØÃã
    {
        isRotation = false;
        rope.transform.position = new Vector3(0, 6.8f,0);
    }
    public void RopeReSetting()
    {
        StartCoroutine(StartInN(10f));
    }
    public void ChangeText(Text text, int num)
    {
        text.text = num.ToString();
    }
    [PunRPC]
    public void StartRopeRotation()
    {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        if (playerCount >= 2)
        {
            StartCoroutine(StartInN(6));
        }
    }
    
    public void RpcStartGame()
    {
        pv.RPC("StartRopeRotation", RpcTarget.All);
    }
    private IEnumerator StartInN(float time)//nÃÊ ÈÄ ½ÃÀÛ
    {
       yield return new WaitForSeconds(time);
        isRotation = true;
        Debug.Log("gamestart");
    }
    private IEnumerator CountDownReMainTime()
    {
        float time = 5f;

        while (time>-1) {
            countDownText.text = time.ToString();
            yield return new WaitForSeconds(1f);
            time -= 1; if (time < 0) { break; } 
        }
    }
}
