﻿using UnityEngine;


namespace tm { 
    namespace inner {

        /// <summary>文字列生成時に使用する、変換テーブルを管理するクラスです。</summary>
        /// <example><code>
        /// using tm;
        /// using inner;
        /// 
        ///     ...
        ///     
        /// //初期化処理
        /// ConvertTableMgr table = new ConvertTableMgr();
        /// 
        /// //独自に変換テーブルを指定(一例)
        /// table.SetKeyCode2RomaTable(in csvSrc);
        /// 
        /// //キーコードから文字に変換し、ログに出力
        /// private void OnGUI()
        /// {
        ///     if (Event.current.type == EventType.KeyDown &amp;&amp; Event.current.keyCode != KeyCode.None){
        ///         Debug.Log(table.Key2kanaMid.Convert(Event.current.keyCode, Event.current.shift, Event.current.functionKey));
        ///         Debug.Log(table.Key2Roma.Convert(Event.current.keyCode, Event.current.shift, Event.current.functionKey));
        ///     }
        /// }
        /// 
        /// </code></example>
        public class ConvertTableMgr{


            #region 生成
            /// <summary>文字列生成時に使用する、変換テーブルを管理するクラスです。</summary>
            public ConvertTableMgr() {
                CreateDefaultTable();
            }
            #endregion


            #region メソッド
            /// <summary>キーの入力(KeyCode) → ローマ字文字への変換テーブルを指定</summary>
            /// <param name="asset">
            /// <para>キーの入力(KeyCode)からローマ字文字への変換テーブルを定義したファイルアセット</para>
            /// <para>以下のようなCSV(.csv)形式ファイルを用意してください。文字コードは[UTF-8]としてください。</para>
            /// <para>変換先文字,【UnityEngine.KeyCode】, isShift, isFunction</para>
            /// <para>例)</para>
            /// <para>S,115,1,0</para>
            /// <para>s,115,0,0</para>
            /// </param>
            public void SetKeyCode2RomaTable(in TextAsset asset) {
                Key2Roma = new KeyCode2CharTable(asset);
                Key2Roma.EnabledCapsLock = EnabledCapsLock;
            }

            /// <summary>ローマ字文字列 → ひらがな文字列への変換テーブルを指定</summary>
            /// <param name="asset">
            /// <para>ローマ字文字列からひらがな文字列への変換テーブルを定義したファイルアセット</para>
            /// <para>以下のようなCSV(.csv)形式ファイルを用意してください。文字コードは[UTF-8]としてください。</para>
            /// <para>ローマ字文字列,ひらがな文字列</para>
            /// <para>例)</para>
            /// <para>a,あ</para>
            /// <para>shi,し</para>
            /// </param>
            public void SetRoma2KanaTable(in TextAsset asset) {
                Roma2Kana = new Roma2KanaTable(asset);
                Kana2Roma = new Kana2RomaTable(asset);
            }

            /// <summary>
            /// <para>キーの入力(KeyCode) → ひらがな中間文字への変換テーブルを指定</para>
            /// <para>JISかな入力など、日本語を直接入力する方式を使用する際に参照します。</para>
            /// </summary>
            /// <param name="asset">
            /// <para>キーの入力(KeyCode)からひらがなの中間文字への変換テーブルを定義したファイルアセット</para>
            /// <para>以下のようなCSV(.csv)形式ファイルを用意してください。文字コードは[UTF-8]としてください。</para>
            /// <para>変換先文字,【UnityEngine.KeyCode】, isShift, isFunction</para>
            /// <para>例)</para>
            /// <para>ぬ,49,0,0</para>
            /// <para>ぬ,49,1,0</para>
            /// <para>ふ,50,0,0</para>
            /// </param>
            public void SetKeyCode2KanaMidTable(in TextAsset asset) {
                Key2kanaMid = new KeyCode2CharTable(asset);
                Key2kanaMid.EnabledCapsLock = false; //こちらはCapsLockの影響を受けない
            }

            /// <summary>ひらがな中間文字列　→ ひらがな文字列への変換テーブルを指定</summary>
            /// <param name="asset">
            /// <para>ひらがなの中間文字列からひらがな文字列への変換テーブルを定義したファイルアセット</para>
            /// <para>以下のようなCSV(.csv)形式ファイルを用意してください。文字コードは[UTF-8]としてください。</para>
            /// <para>ひらがな中間文字列,ひらがな文字列</para>
            /// <para>例)</para>
            /// <para>か゛,が</para>
            /// <para>き゛,ぎ</para>
            /// </param>
            public void SetKanaMid2KanaTable(in TextAsset asset) {
                KanaMid2Kana = new KanaMid2KanaTable(asset);
                Kana2KanaMid = new Kana2KanaMidTable(asset);

            }
            #endregion


            #region プロパティ
            ///<summary>キーの入力(KeyCode)からローマ字文字への変換テーブル</summary>
            public KeyCode2CharTable Key2Roma { get; private set; }

            ///<summary>ローマ字文字列からひらがな文字列への変換テーブル</summary>
            public Roma2KanaTable Roma2Kana { get; private set; }

            ///<summary>ひらがな文字列からローマ字文字列への変換テーブル</summary>
            public Kana2RomaTable Kana2Roma { get; private set; }

            ///<summary>キーの入力(KeyCode)からひらがな中間文字への変換テーブル</summary>
            public KeyCode2CharTable Key2kanaMid { get; private set; }

            ///<summary>ひらがな中間文字列からひらがな文字列への変換テーブル</summary>
            public KanaMid2KanaTable KanaMid2Kana { get; private set; }

            ///<summary>ひらがな文字列からひらがな中間文字列　への変換テーブル</summary>
            public Kana2KanaMidTable Kana2KanaMid { get; private set; }

            private bool m_enabledCapsLock = true;
            ///<summary>
            ///<para>CapsLockの状態を反映させるかどうか。</para>
            ///<para>[true]の場合、CapsLock中は、英語の入力に対して大小文字を反転させます。</para>
            ///</summary>
            public bool EnabledCapsLock {
                get { return m_enabledCapsLock; }
                set {
                    m_enabledCapsLock = value;
                    Key2Roma.EnabledCapsLock = value;
                }
            }
            #endregion


            #region 内部メソッド
            ///<summary>デフォルトの変換テーブルを作成</summary>
            private void CreateDefaultTable() {
            
                TextAsset tmp = new TextAsset();
                tmp = Resources.Load("TypeModule/data/KeyCode2Char/qwerty", typeof(TextAsset)) as TextAsset;
                if (tmp != null) {SetKeyCode2RomaTable(in tmp);}
            
                tmp = new TextAsset();
                tmp = Resources.Load("TypeModule/data/Char2Kana/roma", typeof(TextAsset)) as TextAsset;
                if (tmp != null) {SetRoma2KanaTable(in tmp);}
            
                tmp = new TextAsset();
                tmp = Resources.Load("TypeModule/data/KeyCode2Char/JISkana", typeof(TextAsset)) as TextAsset;
                if (tmp != null) {SetKeyCode2KanaMidTable(in tmp);}
          
                tmp = new TextAsset();
                tmp = Resources.Load("TypeModule/data/Char2Kana/JISkana", typeof(TextAsset)) as TextAsset;
                if(tmp != null) {SetKanaMid2KanaTable(in tmp);}
            }
            #endregion
        }
    }
}