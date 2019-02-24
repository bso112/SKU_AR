﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    delegate void NoFactorDel();
    delegate void MissionDel(Mission mission);

    //난이도, 스코어, 타이머 등 전반적인 게임진행에 관한 스크립트
    private GameManager() { }
    public static GameManager instance;
    public Text timer;
    [HideInInspector] public int min;
    [HideInInspector] public float second;
    [HideInInspector] public float time;

    public Text score;
    public GameObject hp;
    public Mission mission;
    private bool[] flags = new bool[10]; // 같은 조건(Mission.isMissionOn = true)에서 각 미션을 한번만 실행하기 위한 일회용 플래그



    public static GameManager GetInstance()
    {
        if (!instance)
        {
            instance = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
            if (!instance)
                Debug.LogError("There needs to be one active MyClass script on a GameObject in your scene.");

        }
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        //게임 시작과 동시에 첫 미션 스타트
        Mission.isMissionOn = true;    
        for (int i = 0; i < flags.Length; i++)
        {
            flags[i] = true;
        }


    }


    float timelimit = 0;
    private void Update()
    {
        Debug.Log("게임매니저의 update 실행");
        

        second += Time.deltaTime;
        time += Time.deltaTime;
        timer.text = string.Format("{0:D2} : {1:D2}", min, (int)second);
        if (second >= 60)
        {
            min += 1;
            second = 0;
        }
        
        if (time > 6f)
        {
            if (flags[2]) { mission = new TestMission(); flags[2] = false; } //한번만 실행 if (flags[i]) { [실행할 내용] flags[i] = false; } 이걸 메소드롤 만들어보려다가 실패. 왜냐면 그 메소드 또한 한번만 실행되어야하기 때문임.(인덱스 이동 문제로 인해)
            mission.MissionEvent();
            timelimit = 5f;
            if (time > mission.timeSnapshot + timelimit) { if (flags[3]) { TimeOut(); flags[3] = false; } }
        }
        else
        {
            if (flags[0]) { mission = new StartMission(); flags[0] = false; }
            
            mission.MissionEvent();
            timelimit = 5f;
            if(time > mission.timeSnapshot + timelimit) { if (flags[1]) { TimeOut(); flags[1] = false; } }

        }

        Debug.Log("타임스냅샷:" + mission.timeSnapshot);
    }


    void TimeOut() //한번만 실행되어야함
    {   
        
        Debug.Log("타임아웃");
        mission.dialog.text = mission.missionDialog[1];
        mission.dialogPanel.SetActive(true);
        Mission.isMissonSucced = false;
        Mission.isMissionOn = false;
    }


   

}
