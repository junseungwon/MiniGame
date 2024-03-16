using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class JumpRope : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private GameObject centerPos = null;
    [SerializeField] private GameObject rope = null;
    [SerializeField] private float ropeSpeed = 100f;
    private Vector3 startPos = Vector3.zero;
    private bool isRotation = false;
    public Text[] texts = new Text[2];
    public Text[] nickNameText = new Text[2];
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
    private void RotationRope()//동아줄 회전
    {
        if (isRotation)
        {
            rope.transform.RotateAround(centerPos.transform.position, Vector3.left, ropeSpeed * Time.deltaTime);
        }
    }
    public void StopRotationRope()//줄 회전을 멈춤
    {
        isRotation = false;
        rope.transform.position = new Vector3(0, 6.8f, 0);
    }
    public void RopeReSetting()// 줄 재세팅
    {
        StartCoroutine(StartInN(10f));
    }
    public void ChangeText(Text text, int num)//text 숫자 변경하기
    {
        text.text = num.ToString();
    }
    [PunRPC]
    public void StartRopeRotation()//처음 2명이 있으면 겜 시작
    {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        if (playerCount >= 2)
        {
            StartCoroutine(StartInN(6));
        }
    }

    public void RpcStartGame()//StartRopeRotation 모두에게 전달
    {
        pv.RPC("StartRopeRotation", RpcTarget.All);
    }
    private IEnumerator StartInN(float time)//n초 후 시작
    {
        yield return new WaitForSeconds(time);
        isRotation = true;
        Debug.Log("gamestart");
    }
    private void ChangeNickName()
    {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        if (playerCount >= 2)
        {
            string nickName = JumpRopeSystem.Instance.nickName;
            if (PhotonNetwork.IsMasterClient)
            {
                nickNameText[0].text = nickName;
            }
            else
            {
                nickNameText[1].text = nickName;
            }
            pv.RPC("GetOpperNickName", RpcTarget.OthersBuffered);
        }

    }
    private IEnumerator CountDownReMainTime()// 카운트 다운 아직안함
    {
        float time = 5f;

        while (time > -1)
        {
            countDownText.text = time.ToString();
            yield return new WaitForSeconds(1f);
            time -= 1; if (time < 0) { break; }
        }
    }
    [PunRPC]
    private void GetOpperNickName()
    {
        opperNickName = JumpRopeSystem.Instance.nickName;
    }
    private string opperNickName = "";
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(opperNickName);
        }
        else
        {
            opperNickName = (string)stream.ReceiveNext();
        }
    }
}
