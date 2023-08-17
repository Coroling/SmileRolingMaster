using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SubPanel : MonoBehaviour
{
    MainPanel mainPanel;
    public TextMeshProUGUI texX;
    public TextMeshProUGUI texY;

    public Button UpBtn;
    public Button DownBtn;
    public Button DeleteBtn;
    public Vector3 posData;
    GameObject PosObj;

    //�ʱ�ȭ
    //AddNewData���� ������ �����ǳڿ� ���� �Լ� �̷��� �ϸ� �ڱ��ڽ��� ����Ʈ�� �������� ���������. 
    public void PanelInit(Vector2 pos, MainPanel mainPanel)
    {
        this.mainPanel = mainPanel;
        posData = pos;
        if (mainPanel.PosObj != null) // ��ġ ǥ�ÿ� ������Ʈ ����
            PosObj = Instantiate(mainPanel.PosObj, pos, Quaternion.identity);

        texX.text = pos.x.ToString();
        texY.text = pos.y.ToString();

        btnInit();
    }

    #region ButtonInit
    private void btnInit()
    {
        //��ư���� ����� �ʱ�ȭ �Ѵ�.
        UpBtn?.onClick.RemoveAllListeners();
        DownBtn?.onClick.RemoveAllListeners();
        DeleteBtn?.onClick.RemoveAllListeners();

        UpBtn?.onClick.AddListener(OnUpBtnClick);
        DownBtn?.onClick.AddListener(OnDownBtnClick);
        DeleteBtn?.onClick.AddListener(OnDeleteBtnClick);
    }

    void OnUpBtnClick()
    {
        if (mainPanel == null)
        {
            Debug.LogError("�������� �����ǳ��� ��ϵ��� �ʾҽ��ϴ�.");
            return;
        }
        mainPanel.UpSubPanel(this);
    }
    void OnDownBtnClick()
    {
        if (mainPanel == null)
        {
            Debug.LogError("�������� �����ǳ��� ��ϵ��� �ʾҽ��ϴ�.");
            return;
        }
        mainPanel.DownSubPanel(this);
    }
    void OnDeleteBtnClick()
    {
        if (mainPanel == null)
        {
            Debug.LogError("�������� �����ǳ��� ��ϵ��� �ʾҽ��ϴ�.");
            return;
        }
        mainPanel.RemoveSubPanel(this);
    }

    #endregion


    private void OnDestroy()
    {
        if (PosObj)
            Destroy(PosObj);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
