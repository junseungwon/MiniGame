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
    private void Zoom()//ī�޶�ȭ���� Ȯ�� ���
    {
        float zoomScroll = Input.GetAxis("Mouse ScrollWheel");
        float newZoom = playerEye.fieldOfView- zoomScroll*5f;
        newZoom = Mathf.Clamp(newZoom, 20f, 60f);
        playerEye.fieldOfView = newZoom;
    }
    private void ChangePlayerEyeCamera(int num) //�÷��̾� ���� �ش�Ǵ� ī�޶�� ����
    {
        playerEye.depth = num;
    }
    /*
     * Ȱ ��� ��� 
     * 1. ���� �ؼ� ��ǥ�� �ܳ��Ѵ�.
     * 2. Ȱ�� ��ܼ� Ȱ�� �ӵ��� �����Ѵ�.
     * 3. ��ư�� ������ ȭ���� ������.
     * # 3�� ���� ���� ���� ���ߴ� ����� �¸� 
     * +�߰��Ѵٸ� �ݵ� ������ �ְ� ���� �� ����ų� 
     */
}
