﻿using System.Collections.Generic;
using UnityEngine;


namespace tm {
    namespace inner {

        /// <summary>ひらがなの中間文字列から、ひらがな文字列に変換する為のテーブルを管理するクラスです。</summary>
        /// <example><code>
        /// using tm;
        /// using inner;
        /// 
        ///     ...
        ///     
        /// //初期化処理
        /// KanaMid2KanaTable table = new KanaMid2KanaTable(in csvSrc);
        /// 
        /// 
        /// //ひらがなの中間文字列から、変換できるひらがながあるかをチェック及び取得
        /// string outCvt;
        /// Debug.Log(table.TryConvert("あ", out outCvt));          // false
        /// Debug.Log(table.TryConvert("か", out outCvt));          // false
        /// Debug.Log(table.TryConvert("か゛", out outCvt));        // true
        /// //将来変換できる可能性があるかもチェック
        /// Debug.Log(table.TryConvert("か", out outCvt, true));    // true
        /// Debug.Log(table.TryConvert("か゛", out outCvt, true));  // true
        /// 
        /// </code></example>
        public class KanaMid2KanaTable {


            #region 生成
            ///<summary>ひらがなの中間文字列から、ひらがな文字列に変換する為のテーブルを管理するクラスです。</summary>
            ///<param name="aCSV">
            ///<para> ひらがなの中間文字列から、ひらがな文字列への変換テーブルを定義したファイルアセット</para>
            ///<para>［形式］ひらがな中間文字列, ひらがな文字列,</para>
            ///<para>［例］か゛,が</para>
            ///</param>
            public KanaMid2KanaTable(in TextAsset aCSV) {
                CreateTable(in aCSV);
            }
            #endregion


            #region メソッド
            /// <summary>ひらがなの中間文字列[aKanaMid]に対して、変換できるひらがながあるかをチェック及び取得</summary>
            /// <param name="aKanaMid">ひらかな中間文字列</param>
            /// <param name="aOutKana">(変換できる場合)変換先ひらがな文字列</param>
            /// <param name="aIsPossibility">true:[aKanaMid]に、追加でひらがなの中間文字列を足すことで、打つ方法があるかもチェックする</param>
            /// <returns>true:打つことができる文字列がある</returns>
            public bool TryConvert(string aKanaMid, out string aOutKana, bool aIsPossibility = false) {
                if (m_mid2Kana.TryGetValue(aKanaMid, out aOutKana)) {
                    return true;
                }
                if (aIsPossibility) {
                    //「゛」と「゜」を後ろに付けることで、打てるひらがながあるかチェック
                    if (m_mid2Kana.TryGetValue(aKanaMid + "゛", out aOutKana)) {
                        return true;
                    }
                    if (m_mid2Kana.TryGetValue(aKanaMid + "゛", out aOutKana)) {
                        return true;
                    }
                }
                return false;
            }
            #endregion


            #region プロパティ
            /// <summary>ひらがな文字列に変換できるひらがな中間文字列の最大文字数</summary>
            public int KanaMidMaxLength { get; private set; }
            #endregion


            #region 内部メソッド
            ///<summary>ローマ字列からひらがな文字列に変換するためのテーブルを作成</summary>
            ///<param name="aCSV">変換テーブルを定義したファイル</param>
            private void CreateTable(in TextAsset aCSV) {
                const int CSV_KANA_MID_FIELD = 0;
                const int CSV_KANA_FIELD = 1;

                m_mid2Kana = new Dictionary<string, string>();
                m_Kana2Mid = new Dictionary<string, string>();
                KanaMidMaxLength = 0;

                CsvReadHelper csv = new CsvReadHelper(in aCSV);
                foreach (List<string> record in csv.Datas) {
                    m_mid2Kana.Add(record[CSV_KANA_MID_FIELD], record[CSV_KANA_FIELD]);
                    KanaMidMaxLength = Mathf.Max(KanaMidMaxLength, record[CSV_KANA_MID_FIELD].Length);

                    if (!m_Kana2Mid.ContainsKey(record[CSV_KANA_FIELD])) {
                        m_Kana2Mid.Add(record[CSV_KANA_FIELD], record[CSV_KANA_MID_FIELD]);
                    }
                }
            }
            #endregion


            #region メンバ
            private Dictionary<string, string> m_mid2Kana;  //SortedDictionaryだと、なぜか 「は゜」「は゛」が同じキーとして認識されてエラーが出る
            private Dictionary<string, string> m_Kana2Mid;
            #endregion
        }
    }
}