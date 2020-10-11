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
            var temps = convert(textBox_temp.Text, "温度");
            if (temps != null)
            {
                stringBuilder.Append(string.Format("体温：{0}~{1}℃", temps.Min(), temps.Max()));
            }

            // 心率转化
            var hrs = convert(textBox_hr.Text, "心率");
            if (hrs != null)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append('，');
                }
                stringBuilder.Append(string.Format("心率：{0}~{1}次/分", hrs.Min(), hrs.Max()));
            }

            string bp = textBox_bp.Text;
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

                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append('，');
                }
                stringBuilder.Append(string.Format("血压: {0}~{1}/{2}~{3}mmHg", systolic.Min(), systolic.Max(), diastolic.Min(), diastolic.Max()));
            }

            textBox_result.Text = stringBuilder.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox_temp.Clear();
            textBox_hr.Clear();
            textBox_bp.Clear();
            textBox_result.Clear();
        }
    }
}
