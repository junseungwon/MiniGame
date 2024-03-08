using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class JumpRopeSystem : MonoBehaviourPunCallbacks
{
    private string nickName = "";
    private int maxPlayer = 0;
    [SerializeField] private Text nickNameText = null;
    private void Awake()//ó�� ����
    {
        DontDestroyOnLoad(this);
        Screen.SetResolution(960, 540, false);
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
        GameObject player =PhotonNetwork.Instantiate("Player", new Vector3(0,1,0), Quaternion.identity);
        player.name = nickName;
    }
    [PunRPC]
    public void UpIntMaxPlayer()
    {
        
    }
}
