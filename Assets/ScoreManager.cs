using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;  // DOTween ���g������

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Text _scoreText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }





//   public void SetScore()
//    {
//        int tempScore = _player2Score; // �ǉ��O�̃X�R�A


//        _player2Score += addScore;
//        if (_player2Score >= 0)
//        {
//            // DOTween.To() ���g���ĘA���I�ɕω�������
//            DOTween.To(() => tempScore, // �A���I�ɕω�������Ώۂ̒l
//                x => tempScore = x, // �ω��������l x ���ǂ��������邩������
//                _player2Score, // x ���ǂ̒l�܂ŕω������邩�w������
//                0.5f)   // ���b�����ĕω������邩�w������
//                .OnUpdate(() => _scoreP2.text = tempScore.ToString())   // ���l���ω�����x�Ɏ��s���鏈��������
//                .OnComplete(() => _scoreP2.text = _player2Score.ToString());   // ���l�̕ω��������������Ɏ��s���鏈��������
//        }
//        else
//        {
//            _player2Score = 0;
//        }
//        _scoreP2.text = _player2Score.ToString();
//    }
//}
}
