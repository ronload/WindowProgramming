using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Login {
    public partial class Form2 : Form {
        static double fr = 0; // 摩擦力
        static Graphics g; //繪圖裝置（一個就夠了）
        static int r = 10, r2 = 20;    //半徑，直徑

        class ball { //球 class(類別)
            public double spd = 0; // 球 行進速度
            public void move() { // 移動球
                if (spd > 0) { // 速度>0才移動
                    x += spd * cosA; // x 分量
                    y += spd * sinA; // y 分量
                    spd -= fr; // 速度依摩擦力遞減
                } else spd = 0; // 避免spd <０造成反向移動
            }
            int id;                                      //球編號
            public double x = 0, y = 0;    //球心 坐標
            Color c;                                   //球顏色
            SolidBrush br;                        //刷子（畫球用）
            private double ang = 0;       //ex4：球 行進角度
            public double cosA, sinA;   //ex4：coSine 行進角度, Sine 行進角度
            public ball(int bx, int by, Color cc, int i) {  //建構者
                x = bx;                                 //球心 x 坐標
                y = by;                                 //球心 y 坐標
                c = cc;                                  //球顏色    
                br = new SolidBrush(cc);     //球顏色的刷子
                id = i;                                   //球編號
            }
            public void draw() {      //畫 球物件 自己
                g.FillEllipse(br, (int)(x - r), (int)(y - r), r2, r2);          //畫橢圓（球刷子，左上角 坐標，直徑寬，直徑高）
            }
            public void setAng(double _ang) {    //ex4：角度 改變
                ang = _ang;                       // 存 新角度
                cosA = Math.Cos(ang);     //  重算 coSine
                sinA = Math.Sin(ang);       //  重算 Sine
            }
            public void drawStick() {    //ex4：畫球桿
                double r12 = 12 * r;
                Pen skyBluePen = new Pen(Brushes.DeepSkyBlue);    // 宣告 + new 深藍色  畫筆
                skyBluePen.Width = 3.0F;   // 改變 畫筆 寬度
                g.DrawLine(skyBluePen,      // 深藍色  畫筆
                     (float)(x - r12 * cosA), (float)(y - r12 * sinA),    //  12 倍大的 同心圓周上的點
                     (float)(x - r * cosA), (float)(y - r * sinA)        //  球 圓周上的點
                );                                                        // - r12   -r , 使球杆 畫在滑鼠點的另一邊
            }
        }	//class ball 結束
        private void timer1_Tick(object sender, EventArgs e) {
            double sum_spd = 0; // 球的速度加總
            panel1.Refresh(); // 呼叫panel1_Paint事件處理副程式
            for(int i = 0 ; i < 10 ; i++) {
                balls[i].move(); // 移動球
                sum_spd += balls[i].spd;
            }
            if (sum_spd <= 0.001) {  //  所有球 都停了
                timer1.Stop();		//  停止 計時器
                panel1.Refresh();
            }
        }
        ball[] balls = new ball[10];    // 10 顆球的陣列    宣告，new 
        private void Hit_button_Click(object sender, EventArgs e) {
            // 每次擊球，重新初始化打擊力，摩擦力
            balls[0].spd = vScrollBar1.Maximum - vScrollBar1.Value; // 母球 加 速度
            fr= (vScrollBar2.Maximum - vScrollBar2.Value) / 50.0;  // 摩擦力
            timer1.Enabled = true;  // 開始定時 呼叫timer1_Tick
        }

        public Form2() {
            InitializeComponent();
            g = panel1.CreateGraphics();     //繪圖裝置 初始化
            for (int i = 1; i < 10; i++)            //new 每個球，ball 建構者參數 見 work_note3 說明
                balls[i] = new ball(200, i * (r2 + 8), Color.FromArgb(255, (i * 100) % 256, (i * 50) % 256, (i * 25) % 256), i);
            
            // 0號球(母球)， 白色，放右邊中間
            balls[0] = new ball(400, 140, Color.FromArgb(255, 255, 255, 255), 0);
            balls[0].setAng(Math.PI / 4);
        }


        private void panel1_Paint(object sender, PaintEventArgs e) {
            for (int i = 0; i < 10; i++)     //10 顆球
                balls[i].draw();     //每個球 畫自己

            balls[0].drawStick();     //  ex4：畫指向 0號球(母球) 的球桿
            if (balls[0].spd < 0.0001) balls[0].drawStick();     //  ex5：0號球停止時 才畫指向 0號球(母球) 的球桿
        }

        private void button1_Click(object sender, EventArgs e) {
            this.Hide();
            Owner.Show();
        }

        private void label1_Click(object sender, EventArgs e){
            
        }

        private void panel1_MouseDown_1(object sender, MouseEventArgs e) {
            double a = Math.Atan2(e.Y - balls[0].y, e.X - balls[0].x); // e:滑鼠 點擊處坐標
            balls[0].setAng(a); // 存入母球 行進角度
            panel1.Refresh(); // 重新繪畫轉動過的球桿
            g.DrawRectangle(Pens.HotPink, e.X - 2, e.Y - 2, 4, 4); // 點擊點 畫小方塊
        }

        private void vScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e) {
            label1.Text = ((Form1)Owner).textBox2.Text + "你好!";
        }
    }
}
