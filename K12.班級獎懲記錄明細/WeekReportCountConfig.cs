using System;
using System.Windows.Forms;
using System.Xml;
using FISCA.DSAUtil;
using FISCA.Presentation.Controls;
using K12.Data.Configuration;

namespace K12.ClassMeritDemerit.Detail
{
    public partial class WeekReportCountConfig : BaseForm
    {
        private string _reportName = "";

        /// <summary>
        /// �ǤJ�]�w�r��
        /// </summary>
        /// <param name="reportname"></param>
        /// <param name="sizeIndex"></param>
        /// <param name="xml"></param>
        public WeekReportCountConfig(string reportname)
        {
            InitializeComponent();

            //�]�w��
            _reportName = reportname;
            //�]�w�e��
            BingData();
        }

        private void BingData()
        {
            ConfigData cd = K12.Data.School.Configuration[_reportName];
            XmlElement config = cd.GetXml("XmlData", null);

            DSXmlHelper DSXML = new DSXmlHelper(config);
            comboBoxEx1.SelectedIndex = int.Parse(DSXML.GetAttribute("Print/@PaperSize"));
            checkBoxX1.Checked = bool.Parse(DSXML.GetAttribute("MeritDemerit/@Merit"));
            checkBoxX3.Checked = bool.Parse(DSXML.GetAttribute("MeritDemerit/@DemeritClear"));
            checkBoxX4.Checked = bool.Parse(DSXML.GetAttribute("MeritDemerit/@DemeritNotClear"));
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            #region �x�s Preference

            //XmlElement config = CurrentUser.Instance.Preference[_reportName];
            ConfigData cd = K12.Data.School.Configuration[_reportName];
            XmlElement config = cd.GetXml("XmlData", null);

            DSXmlHelper DSXML = new DSXmlHelper(config);

            DSXML.SetAttribute("Print", "PaperSize", comboBoxEx1.SelectedIndex.ToString());

            DSXML.SetAttribute("MeritDemerit", "Merit", checkBoxX1.Checked.ToString());
            DSXML.SetAttribute("MeritDemerit", "DemeritClear", checkBoxX3.Checked.ToString());
            DSXML.SetAttribute("MeritDemerit", "DemeritNotClear", checkBoxX4.Checked.ToString());

            cd.SetXml("XmlData", DSXML.BaseElement);
            cd.Save();

            #endregion

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void checkBoxX1_CheckedChanged(object sender, EventArgs e)
        {
            CheckCheckBoxMode();
        }

        private void checkBoxX3_CheckedChanged(object sender, EventArgs e)
        {
            CheckCheckBoxMode();
        }

        private void checkBoxX4_CheckedChanged(object sender, EventArgs e)
        {
            CheckCheckBoxMode();
        }


        private void CheckCheckBoxMode()
        {
            if (!checkBoxX1.Checked && !checkBoxX3.Checked && !checkBoxX4.Checked)
            {
                FISCA.Presentation.Controls.MsgBox.Show("���y/�g��/�P�L...��3�ﶵ,�ܤ֥�����ܤ@��\n�_�h�N�|�L�k�C�L�X������!", "ĵ�i", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}