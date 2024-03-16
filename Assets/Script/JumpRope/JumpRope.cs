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
    private void RotationRope()//������ ȸ��
    {
        if (isRotation)
        {
            rope.transform.RotateAround(centerPos.transform.position, Vector3.left, ropeSpeed * Time.deltaTime);
        }
    }
    public void StopRotationRope()//�� ȸ���� ����
    {
        isRotation = false;
        rope.transform.position = new Vector3(0, 6.8f, 0);
    }
    public void RopeReSetting()// �� �缼��
    {
        StartCoroutine(StartInN(10f));
    }
    public void ChangeText(Text text, int num)//text ���� �����ϱ�
    {
        text.text = num.ToString();
    }
    [PunRPC]
    public void StartRopeRotation()//ó�� 2���� ������ �� ����
    {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        if (playerCount >= 2)
        {
            StartCoroutine(StartInN(6));
        }
    }

    public void RpcStartGame()//StartRopeRotation ��ο��� ����
    {
        pv.RPC("StartRopeRotation", RpcTarget.All);
    }
    private IEnumerator StartInN(float time)//n�� �� ����
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
    private IEnumerator CountDownReMainTime()// ī��Ʈ �ٿ� ��������
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
