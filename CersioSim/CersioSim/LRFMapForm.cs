using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CersioSim
{
    public partial class LRFMapForm : Form
    {
        CarSim carSim;

        public LRFMapForm(CarSim _carSim )
        {
            InitializeComponent();

            carSim = _carSim;
        }

        // ※車のセンサー情報からLRF画像表示

        // ※位置座標から、MAP画像書き込み（←LRF情報からのMAP生成の整合性確認）
        //　のちに、RE、地磁気のセンサー情報から自己位置座標算出（←この自己位置計算がキモの一つ） 
    }
}
