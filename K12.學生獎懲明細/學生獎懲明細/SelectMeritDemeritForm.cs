using System;
using System.Windows.Forms;
using FISCA.Presentation.Controls;

namespace K12.學生獎勵懲戒明細
{
    public partial class SelectMeritDemeritForm : BaseForm
    {
        public SelectMeritDemeritForm()
        {
            InitializeComponent();

            comboBoxEx1.SelectedIndex = 0;

            #region 學年度學期
            string schoolYear = K12.Data.School.DefaultSchoolYear;
            cbSchoolYear.Text = schoolYear;
            cbSchoolYear.Items.Add((int.Parse(schoolYear) - 2).ToString());
            cbSchoolYear.Items.Add((int.Parse(schoolYear) - 1).ToString());
            cbSchoolYear.Items.Add((int.Parse(schoolYear)).ToString());

            string semester = K12.Data.School.DefaultSemester;
            cbSemester.Text = semester;
            cbSemester.Items.Add("1");
            cbSemester.Items.Add("2");
            #endregion

            string TimmeInput1 = DateTime.Now.AddMonths(-1).ToString("yyyy/MM/dd");
            dateTimeInput1.Text = TimmeInput1;

            string TimmeInput2 = DateTime.Now.ToString("yyyy/MM/dd");
            dateTimeInput2.Text = TimmeInput2;
        }

        #region 依日期還是依學期之狀態 SelectDayOrSchoolYear
        public bool SelectDayOrSchoolYear
        {
            get
            {
                if (radioButton1.Checked)
                {
                    return true; //依日期
                }
                else
                {
                    return false; //依學年度學期
                }
            }
        } 
        #endregion

        #region 依日期後是否參考登錄日期還是發生日期 SetupTime

        public bool SetupTime
        {
            get
            {
                if (comboBoxEx1.SelectedIndex == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        #endregion

        public string SchoolYear
        {
            get
            {
                return cbSchoolYear.Text;
            }
        }

        public string Semester
        {
            get
            {
                return cbSemester.Text;
            }
        }

        /// <summary>
        /// 列印所有學年度
        /// </summary>
        public bool checkBoxX1Bool
        {
            get
            {
                return checkBoxX1.Checked;
            }
        }

        /// <summary>
        /// 是否排除已銷過資料
        /// </summary>
        public bool checkBoxX2Bool
        {
            get
            {
                return checkBoxX2.Checked;
            }
        }

        #region 日期的值 StartDay,EndDay
        public string StartDay //取得開始日期
        {
            get
            {
                return dateTimeInput1.Text;
            }
        }

        public string EndDay //取得結束日期
        {
            get
            {
                return dateTimeInput2.Text;
            }
        }
        #endregion

        protected virtual void buttonX1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void checkBoxX1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxX1.Checked)
            {
                cbSchoolYear.Enabled = false;
                cbSemester.Enabled = false;
            }
            else
            {
                cbSchoolYear.Enabled = true;
                cbSemester.Enabled = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxEx1.Enabled = radioButton1.Checked;
            dateTimeInput1.Enabled = radioButton1.Checked;
            dateTimeInput2.Enabled = radioButton1.Checked;
            labelX3.Enabled = radioButton1.Checked;
            labelX4.Enabled = radioButton1.Checked;
            labelX5.Enabled = radioButton1.Checked;
            checkBoxX1.Checked = false;
            cbSchoolYear.Enabled = false;
            cbSemester.Enabled = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            cbSchoolYear.Enabled = radioButton3.Checked;
            cbSemester.Enabled = radioButton3.Checked;
            labelX1.Enabled = radioButton3.Checked;
            labelX2.Enabled = radioButton3.Checked;
            checkBoxX1.Enabled = radioButton3.Checked;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}