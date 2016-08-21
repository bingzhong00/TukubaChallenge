using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Axiom.Math;

namespace LocationPresumption
{
    /// <summary>
    /// ���Ȉʒu���� �}�b�v���W�ϊ��v�Z�@�N���X
    /// </summary>
    public partial class LocPreSumpSystem
    {
        /// <summary>
        /// ���[�p�X�t�B���^�[
        /// </summary>
        private KalmanFilter.SimpleLPF lpfV1X = new KalmanFilter.SimpleLPF();
        private KalmanFilter.SimpleLPF lpfV1Y = new KalmanFilter.SimpleLPF();
        private KalmanFilter.SimpleLPF lpfV1Theta = new KalmanFilter.SimpleLPF();


        // ����]��+�@�E��-
        public const int ParticleSize = 200;        // �p�[�e�B�N����
        private ParticleFilter ParticleFilter;              // �T���v�����O���A�p�[�e�B�N���̃����W
        private List<Particle> Particles;           // �p�[�e�B�N�����X�g


        // �������x�v���J�E���^
        public static System.Diagnostics.Stopwatch swCNT_Update = new System.Diagnostics.Stopwatch();
        public static System.Diagnostics.Stopwatch swCNT_MRF = new System.Diagnostics.Stopwatch();


        /// <summary>
        /// �C�Ӎ��W�ł̂o�e�␳
        /// </summary>
        /// <param name="numPF"></param>
        public void CalcLocalize(MarkPoint mkp, bool useLowFilter, int numPF = 1)
        {
            // �p�[�e�B�N���t�B���^�[���Z

            if (useLowFilter)
            {
                KalmanFilter.SimpleLPF lpfX = null;
                KalmanFilter.SimpleLPF lpfY = null;
                KalmanFilter.SimpleLPF lpTheta = null;

                for (int i = 0; i < numPF; i++)
                {
                    MarkPoint locMkp = new MarkPoint(worldMap.GetAreaX(mkp.X), worldMap.GetAreaY(mkp.Y), mkp.Theta);

                    ParticleFilter.Localize(LRF.getData(), MRF, locMkp, Particles);

                    locMkp.X = worldMap.GetWorldX(locMkp.X);
                    locMkp.Y = worldMap.GetWorldY(locMkp.Y);

                    // ���ʂɃ��[�p�X�t�B���^�[��������
                    if (null == lpfX)
                    {
                        // �ŏ��̒l�������l�ɂ���
                        lpfX = new KalmanFilter.SimpleLPF(locMkp.X);
                        lpfY = new KalmanFilter.SimpleLPF(locMkp.Y);
                        lpTheta = new KalmanFilter.SimpleLPF(locMkp.Theta);
                    }
                    else
                    {
                        lpfX.update(locMkp.X);
                        lpfY.update(locMkp.Y);
                        lpTheta.update(locMkp.Theta);
                    }
                }

                if (null != lpfX)
                {
                    mkp.X = lpfX.value();
                    mkp.Y = lpfY.value();
                    mkp.Theta = lpTheta.value();
                }
            }
            else
            {
                ParticleFilter.Localize(LRF.getData(), MRF, mkp, Particles);
            }
        }

        /// <summary>
        /// PF���Ȉʒu�v�Z �p��
        /// </summary>
        /// <param name="mkp"></param>
        /// <param name="useLowFilter"></param>
        /// <param name="numPF"></param>
        public void ParticleFilterLocalize(bool useLPF)
        {
            MarkPoint locMkp = new MarkPoint(worldMap.GetAreaX(V1.X), worldMap.GetAreaY(V1.Y), V1.Theta);

            // �p�[�e�B�N���t�B���^�[���Z
            ParticleFilter.Localize(LRF.getData(), MRF, locMkp, Particles);

            if (useLPF)
            {
                // ���ʂɃ��[�p�X�t�B���^�[��������
                V1.X = lpfV1X.update(worldMap.GetWorldX(locMkp.X));
                V1.Y = lpfV1Y.update(worldMap.GetWorldY(locMkp.Y));
                V1.Theta = lpfV1Theta.update(locMkp.Theta);
            }
            else
            {
                V1.X = worldMap.GetWorldX(locMkp.X);
                V1.Y = worldMap.GetWorldY(locMkp.Y);
                V1.Theta = locMkp.Theta;
            }
        }

        /// <summary>
        /// ���Ȉʒu���� �X�V
        /// </summary>
        /// <param name="useAlwaysPF">����p�[�e�B�N���t�B���^�[���v�Z����</param>
        /// <returns>true...�␳����, false..�␳�s�v</returns>
        public bool ParticleFilter_Update()
        {
            bool bResult = false;   // �␳�������H

            {
                // �������Ԍv��
                swCNT_Update.Start();

                // PF���Z
                ParticleFilterLocalize(true);

                // �������Ԍv������
                swCNT_Update.Stop();
            }

            return bResult;
        }

    }
}