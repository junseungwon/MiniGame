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
    private void Awake()//처음 세팅
    {
        DontDestroyOnLoad(this);
        Screen.SetResolution(960, 540, false);
    }
    public void NextScene()//버튼 누르면 겜씬으로 이동한다.
    {
        GetNickName();
        PhotonNetwork.ConnectUsingSettings();
        SceneManager.LoadScene(1);
    }
    private void GetNickName()//닉넴생성
    {
        nickName = nickNameText.text;
    }
    public override void OnConnectedToMaster()//마스터가 연결되면 방만들기
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new Photon.Realtime.RoomOptions { MaxPlayers = 2 }, null);
    }
    public override void OnJoinedRoom()//방에 참가했을 경우 플레이어 생성
    {
        GameObject player =PhotonNetwork.Instantiate("Player", new Vector3(0,1,0), Quaternion.identity);
        player.name = nickName;
    }
    [PunRPC]
    public void UpIntMaxPlayer()
    {
        
    }
}
