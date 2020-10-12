using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ICUTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private IList<double> convert(string text, string title)
        {
            if (text == string.Empty)
            {
                return null;
            }

            var values = new List<double>();
            foreach (var s in Regex.Split(text, "\\s+"))
            {
                double d;
                if (!double.TryParse(s, out d))
                {
                    MessageBox.Show(string.Format("{0}错误，必须是数字", title));
                    return null;
                }
                values.Add(d);
            }
            return values;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();

            // 体温转化
            var temps = convert(textBox_temp.Text.Trim(), "温度");
            if (temps != null)
            {
                stringBuilder.Append(string.Format("体温：{0}~{1}℃", temps.Min(), temps.Max()));
            }

            // 心率转化
            var hrs = convert(textBox_hr.Text.Trim(), "心率");
            if (hrs != null)
            {
                CheckStringBuilder(stringBuilder);
                stringBuilder.Append(string.Format("心率：{0}~{1}次/分", hrs.Min(), hrs.Max()));
            }

            // 呼吸
            var rr = convert(textBox_rr.Text.Trim(), "呼吸");
            if (rr != null)
            {
                CheckStringBuilder(stringBuilder);
                stringBuilder.Append(string.Format("呼吸次数:{0}~{1}次/分", rr.Min(), rr.Max()));
            }

            // 呼吸
            var spo2 = convert(textBox_spo2.Text.Trim(), "SPO2");
            if (spo2 != null)
            {
                CheckStringBuilder(stringBuilder);
                stringBuilder.Append(string.Format("SPO2:{0}~{1}%", spo2.Min(), spo2.Max()));
            }

            // 血压
            string bp = textBox_bp.Text.Trim();
            if (bp != string.Empty)
            {
                List<double> systolic = new List<double>();
                List<double> diastolic = new List<double>();

                foreach (var single in Regex.Split(bp, "\\s+"))
                {
                    try
                    {
                        var parts = single.Split('/').Select(v => double.Parse(v)).ToArray();
                        if (parts.Count() != 2)
                        {
                            MessageBox.Show("血压输入的格式不对，必须都是 100/60 格式");
                            return;
                        }
                        systolic.Add(parts[0]);
                        diastolic.Add(parts[1]);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("血压输入格式错误，必须是140/90");
                        return;
                    }
                }

                CheckStringBuilder(stringBuilder);
                stringBuilder.Append(string.Format("血压: {0}~{1}/{2}~{3}mmHg", systolic.Min(), systolic.Max(), diastolic.Min(), diastolic.Max()));
            }

            if (numericUpDown1.Value > 0)
            {
                CheckStringBuilder(stringBuilder);
                stringBuilder.Append(string.Format("总入量:{0}ml", numericUpDown1.Value));
            }

            if (numericUpDown2.Value > 0)
            {
                CheckStringBuilder(stringBuilder);
                stringBuilder.Append(string.Format("尿量:{0}ml", numericUpDown2.Value));
            }

            if (numericUpDown3.Value > 0)
            {
                CheckStringBuilder(stringBuilder);
                stringBuilder.Append(string.Format("大便:{0}ml", numericUpDown3.Value));
            }

            if (numericUpDown4.Value > 0)
            {
                CheckStringBuilder(stringBuilder);
                stringBuilder.Append(string.Format("总出量:{0}ml", numericUpDown4.Value));
            }

            if (numericUpDown2.Value + numericUpDown3.Value > numericUpDown4.Value)
            {
                MessageBox.Show("总出量必须大于尿量+大便");
                return;
            }

            textBox_result.Text = stringBuilder.ToString();
        }

        private void CheckStringBuilder(StringBuilder sb)
        {
            if (sb.Length > 0)
            {
                sb.Append('，');
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox_temp.Clear();
            textBox_hr.Clear();
            textBox_bp.Clear();
            textBox_result.Clear();

            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;
        }
    }
}
