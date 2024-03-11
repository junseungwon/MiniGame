using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class JumpRopeSystem : MonoBehaviourPunCallbacks
{
    private static JumpRopeSystem instance = null;
    public static JumpRopeSystem Instance
    {
        get {  if (instance == null) instance = FindObjectOfType<JumpRopeSystem>(); return instance; }
    }
    private string nickName = "";
    [SerializeField] private Text nickNameText = null;
    private void Awake()//ó�� ����
    {
        DontDestroyOnLoad(this);
        Screen.SetResolution(600, 600, false);
    }
    public void NextScene()//��ư ������ �׾����� �̵��Ѵ�.
    {
        GetNickName();
        PhotonNetwork.ConnectUsingSettings();
        SceneManager.LoadScene(1);
    }
    private void GetNickName()//�гۻ���
    {
        nickName = nickNameText.text;
    }
    public override void OnConnectedToMaster()//�����Ͱ� ����Ǹ� �游���
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new Photon.Realtime.RoomOptions { MaxPlayers = 2 }, null);
    }
    public override void OnJoinedRoom()//�濡 �������� ��� �÷��̾� ����
    {
        GameObject player = PhotonNetwork.Instantiate("Player", SetStartPoint(), Quaternion.identity);
        player.name = nickName;
        Debug.Log(player.transform.position);
        FindObjectOfType<JumpRope>().RpcStartGame();
    }
    public Vector3 SetStartPoint()//�ʱ���ġ ����
    {
        if (PhotonNetwork.IsMasterClient)
        {
            return new Vector3(1,0.3f,0);
        }
        else
        {
            return new Vector3(-1, 0.3f, 0);
        }     
    }
    [PunRPC]
    public void UpIntMaxPlayer()
    {
        
    }
}
