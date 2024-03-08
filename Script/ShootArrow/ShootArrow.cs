using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrow : MonoBehaviour
{
   
    [SerializeField] private Camera playerEye = null;
    [SerializeField] private GameObject arrow = null;
    [SerializeField] private GameObject bow = null;
    [SerializeField] private GameObject player = null;
    private Transform arrowPos = null;
    void Update()
    {
        Zoom();
       
    }
    private void MoveBow()
    {
        
    }

    private void InstanceArrow()
    {
       
    }
    private void Zoom()//카메라화면을 확대 축소
    {
        float zoomScroll = Input.GetAxis("Mouse ScrollWheel");
        float newZoom = playerEye.fieldOfView- zoomScroll*5f;
        newZoom = Mathf.Clamp(newZoom, 20f, 60f);
        playerEye.fieldOfView = newZoom;
    }
    private void ChangePlayerEyeCamera(int num) //플레이어 눈에 해당되는 카메라로 변경
    {
        playerEye.depth = num;
    }
    /*
     * 활 사용 방법 
     * 1. 줌을 해서 목표를 겨냥한다.
     * 2. 활을 댕겨서 활의 속도를 선택한다.
     * 3. 버튼을 누르면 화살이 나간다.
     * # 3번 쏴서 가장 많이 맞추는 사람이 승리 
     * +추가한다면 반동 낙차가 있고 줌을 더 땡기거나 
     */
}
