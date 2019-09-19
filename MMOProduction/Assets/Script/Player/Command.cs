/////////////////////////////////
// FCOコマンド(FULLCHINONLINE) //
/////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Command : MonoBehaviour
{
    [SerializeField, Header("コマンド")]
    private string command = "";
    
    public UnityAction Function;
    // コマンドの文字数
    private int commandLenght;

    private int currentNum;

    private UnityEvent ue;

    // Start is called before the first frame update
    void Start()
    {
        ue = new UnityEvent();
        ue.AddListener(Message);

        commandLenght = command.Length;

        currentNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
		if(Input.anyKeyDown)
		{
        	if(Input.GetKeyDown(command[currentNum].ToString()))
        	{
        	    currentNum++;
        	}
			else
			{
				currentNum = 0;
			}
		}

        if(currentNum == commandLenght)
        {
            ue.Invoke();
            currentNum = 0;
        }
    }

    private void Message()
    {
        Debug.Log("コマンドが入力されました");
    }

    public void SetAction(UnityAction ua)
    {
        ue.AddListener(ua);
    }
}
