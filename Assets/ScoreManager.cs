using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  // DOTween ���g������

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Text _scoreText;

    int _score = 0;

    void Start()
    {
        _scoreText.text = _score.ToString();
    }


    void Update()
    {
        
    }





    public void SetScore(int addScore)
    {
        int tempScore = _score; // �ǉ��O�̃X�R�A


        _score += addScore;
        if (_score >= 0)
        {
            // DOTween.To() ���g���ĘA���I�ɕω�������
            DOTween.To(() => tempScore, // �A���I�ɕω�������Ώۂ̒l
                x => tempScore = x, // �ω��������l x ���ǂ��������邩������
                _score, // x ���ǂ̒l�܂ŕω������邩�w������
                0.5f)   // ���b�����ĕω������邩�w������
                .OnUpdate(() => _scoreText.text = tempScore.ToString())   // ���l���ω�����x�Ɏ��s���鏈��������
                .OnComplete(() => _scoreText.text = _score.ToString());   // ���l�̕ω��������������Ɏ��s���鏈��������
        }
        else
        {
            _score = 0;
        }
        _scoreText.text = _score.ToString();
    }
}

