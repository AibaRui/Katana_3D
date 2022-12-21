using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlashDelegate : MonoBehaviour
{

    /// <summary>�^�[���J�n���ɌĂ΂�郁�\�b�h</summary>
    public static event Action OnBeginSlash;
    /// <summary>�^�[���I�����ɌĂ΂�郁�\�b�h</summary>
    public static event Action OnEndSlash;

    static bool m_isTurnStarted = false;

    /// <summary>���݂̃^�[����</summary>
    static int m_turnCount = 1;


    /// <summary>
    /// �^�[���J�n���ɌĂ�
    /// </summary>
    public static void BeginSlash()
    {
        OnBeginSlash();
        m_isTurnStarted = true;
    }

    /// <summary>
    /// �^�[���I�����ɌĂ�
    /// </summary>
    public static void EndSlash()
    {
        //// �^�[�����J�n�����ɏI�������ꍇ�͂܂������I�Ƀ^�[�����J�n����
        //if (!m_isTurnStarted)
        //{
        //    BeginTurn();
        //}

        OnEndSlash();
        m_isTurnStarted = false;
    }
}
