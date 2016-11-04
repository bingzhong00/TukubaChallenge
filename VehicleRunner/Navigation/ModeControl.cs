using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocationPresumption;

namespace Navigation
{
    public class ModeControl
    {
        /// <summary>
        /// 動作モード
        /// 数字が大きいほど、優先度高い
        /// </summary>
        public enum ActionMode {
            None = 0,

            /// <summary>
            /// チェックポイント
            /// </summary>
            CheckPoint,

            /// <summary>
            /// ターゲット(人)探索
            /// </summary>
            MarkerSearch,

            /// <summary>
            /// 回避
            /// </summary>
            Avoid,

            /// <summary>
            /// 緊急停止
            /// </summary>
            EmergencyStop,

            /// <summary>
            /// 袋小路脱出バック動作
            /// </summary>
            MoveBack,
        };

        /// <summary>
        /// 動作モード
        /// </summary>
        private ActionMode ActMode = ActionMode.CheckPoint;
        private ActionMode ActModeOld = ActionMode.None;

        /// <summary>
        /// モードカウンタ
        /// </summary>
        private int ActModeCnt = 0;

        private DateTime ActSetTime;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionMode GetActionMode()
        {
            return ActMode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetActionModeString()
        {
            return ActMode.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_actMode"></param>
        /// <returns></returns>
        public bool SetActionMode(ActionMode _actMode )
        {
            ActMode = _actMode;
            ActSetTime = DateTime.Now;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool update()
        {
            ActModeCnt++;
            if (ActModeOld != ActMode)
            {
                ActModeOld = ActMode;
                ActModeCnt = 0;
            }

            switch (ActMode)
            {
                // 
                case ActionMode.None:
                    ActMode = ActionMode.CheckPoint;
                    break;

                // 
                case ActionMode.CheckPoint:
                    break;

                case ActionMode.EmergencyStop:
                    break;
            }

            return true;
        }

        /// <summary>
        /// モードになってからの経過時間
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public bool GetModePassSeconds(int sec )
        {
            return (DateTime.Now > ActSetTime.AddSeconds(sec));
        }

        // ※ルーティング実行

        // ※チェックポイントごとのシーケンス制御
    }
}
