// using：ほかの場所にある機能を「使います」と宣言する行
// UnityEngine の中に MonoBehaviour や Debug が入っている
using UnityEngine;

// namespace：クラスの「住所」。同じ名前のクラスがよそにあってもぶつからないようにする
namespace DeadlineDeck
{
    // HelloDeadline という名前のクラス（設計図）
    // 「: MonoBehaviour」を付けると、GameObject に貼り付けられる部品（コンポーネント）になる
    // ※ クラス名とファイル名（HelloDeadline.cs）は必ず同じにすること！
    public class HelloDeadline : MonoBehaviour
    {
        // Start：再生ボタンを押したあと、最初に 1 回だけ Unity が自動で呼んでくれるメソッド
        private void Start()
        {
            // Debug.Log：Console ウィンドウに文字を表示する命令
            // 動作確認や「ここまで動いたか」を調べるのに使う、開発者の一番の味方
            Debug.Log("デッドライン・デッキ起動！");

            // gameObject.name：このスクリプトを貼り付けた GameObject の名前
            // 「+」で文字どうしをつなげられる
            Debug.Log("このスクリプトは「" + gameObject.name + "」に付いています");
        }
    }
}
