﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWaterWheelQuest : MonoBehaviour, QuestBehavior
{
    Vector2 pre;
    Vector2 curr;
    private Vector2 m_firstDirection;
    private Vector2 m_preDirection;
    private float angleCounter;
    private int currState;
    private ChangeShadows changeShadows;
    private ParticleSystem m_clearSign;
    private CircleCollider2D m_clearSignCollider;
    private SelfRotate m_selfRotate;

    void Awake()
    {
        QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
        angleCounter = 0f;
        currState = 0;
        changeShadows = GameObject.Find("Shadows").GetComponent<ChangeShadows>();
        m_clearSign = GameObject.Find("Clear Sign").GetComponent<ParticleSystem>();
        m_clearSignCollider = GameObject.Find("Clear Sign").GetComponent<CircleCollider2D>();
        m_selfRotate = GetComponent<SelfRotate>();
    }

    // Update is called once per frame
    void Update()
    {
        // PC --stable
        if (Input.GetMouseButton(0))
        {
            Vector2 touchPos = (Vector2)(transform.position);
            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 currDirection = touchPos - (Vector2)(transform.position);

            Vector3 preDirectionVec3 = new Vector3(m_preDirection.x, m_preDirection.y, transform.position.z).normalized;
            Vector3 currDirectionVec3 = new Vector3(currDirection.x, currDirection.y, transform.position.z).normalized;

            float angle = Vector3.Angle(preDirectionVec3, currDirectionVec3);
            Vector3 normal = Vector3.Cross(preDirectionVec3, currDirectionVec3);

            //计算顺时针还是逆时针
            angle *= Mathf.Sign(Vector3.Dot(normal, transform.forward));
            transform.Rotate(new Vector3(0, 0, angle));
            m_preDirection = currDirection;

            angleCounter += angle;
            int state = (int)(Mathf.Abs(angleCounter) / 720);
            if (state != currState)
            {
                currState = state;
                changeShadows.SetUpNewShadowState(state);
            }

        }

        // Android --stable
        //if (Input.touchCount > 0)
        //{
        //    Touch currTouch = Input.GetTouch(0);
        //    Vector2 touchPos = (Vector2)(transform.position);
        //    touchPos = Camera.main.ScreenToWorldPoint(currTouch.position);
        //    Vector2 currDirection = touchPos - (Vector2)(transform.position);

        //    if(currTouch.phase == TouchPhase.Moved)
        //    {
        //        Debug.Log("Moved");
        //        Vector3 preDirectionVec3 = new Vector3(m_preDirection.x, m_preDirection.y, transform.position.z).normalized;
        //        Vector3 currDirectionVec3 = new Vector3(currDirection.x, currDirection.y, transform.position.z).normalized;

        //        Debug.Log("PreDirection" + preDirectionVec3);
        //        Debug.Log("CurrDirection" + currDirectionVec3);

        //        float angle = Vector3.Angle(preDirectionVec3, currDirectionVec3);
        //        Vector3 normal = Vector3.Cross(preDirectionVec3, currDirectionVec3);
        //
        //        //计算顺时针还是逆时针
        //        angle *= Mathf.Sign(Vector3.Dot(normal, transform.forward));
        //        Debug.Log(angle);
        //        transform.Rotate(new Vector3(0, 0, angle));
        //        m_preDirection = currDirection;

        //        angleCounter += angle;
        //        int state = (int)(Mathf.Abs(angleCounter) / 720);
        //        if (state != currState)
        //        {
        //            currState = state;
        //            changeShadows.SetUpNewShadowState(state);
        //        }
        //    }
        //} 

        else
        {
            if (currState >= 4)
            {
                GetComponent<CircleCollider2D>().enabled = false;
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                m_clearSign.Play();
                m_clearSignCollider.enabled = true;
            }
            m_selfRotate.enabled = true;
            this.enabled = false;
        }
    }

    public void OnUpdate()
    {
        m_selfRotate.enabled = false;
        this.enabled = true;

        Vector2 touchPos = (Vector2)(transform.position);

        // Android --stable
        //Touch currTouch = Input.GetTouch(0);
        //touchPos = Camera.main.ScreenToWorldPoint(currTouch.position);

        // PC --stable
        Vector2 currTouch = (Vector2)Input.mousePosition;
        touchPos = Camera.main.ScreenToWorldPoint(currTouch);

        Vector2 currDirection = touchPos - (Vector2)(transform.position);
        m_preDirection = currDirection;
        Debug.Log("Began");
    }

}
