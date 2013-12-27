using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Configuration;

namespace K12.ClassMeritDemerit.Detail
{
    public partial class DisciplineDetailForm : SelectDateRangeForm
    {
        private int _sizeIndex = 0;
        public int PaperSize
        {
            get { return _sizeIndex; }
        }

        public Dictionary<string, bool> SetupDic = new Dictionary<string, bool>();

        public DisciplineDetailForm()
        {
            InitializeComponent();

            LoadPreference();
        }

        private void LoadPreference()
        {
            #region Ū�� Preference
            SetupDic.Clear();
            ConfigData cd = K12.Data.School.Configuration["�Z�ż��g�O������"];
            XmlElement config = cd.GetXml("XmlData", null);
            //XmlElement config = CurrentUser.Instance.Preference["�Z�ż��g���Ӫ�"];

            if (config != null)
            {
                DSXmlHelper DSXML = new DSXmlHelper(config);

                _sizeIndex = int.Parse(DSXML.GetAttribute("Print/@PaperSize"));

                if (DSXML.GetElement("MeritDemerit") == null) //�B�z�U�եثe�]�w��
                {
                    DSXML.AddElement("MeritDemerit");
                    DSXML.SetAttribute("MeritDemerit", "Merit", "True");
                    DSXML.SetAttribute("MeritDemerit", "DemeritClear", "True");
                    DSXML.SetAttribute("MeritDemerit", "DemeritNotClear", "True");
                    cd.SetXml("XmlData", DSXML.BaseElement);
                    cd.Save();
                }

                SetupDic.Add("���y", bool.Parse(DSXML.GetAttribute("MeritDemerit/@Merit")));
                SetupDic.Add("�g�٥��P�L", bool.Parse(DSXML.GetAttribute("MeritDemerit/@DemeritClear")));
                SetupDic.Add("�g�پP�L", bool.Parse(DSXML.GetAttribute("MeritDemerit/@DemeritNotClear")));

            }
            else
            {
                //�ϥιw�]
                DSXmlHelper DSXML = new DSXmlHelper("XmlData");
                DSXML.AddElement("Print");
                DSXML.SetAttribute("Print", "PaperSize", "0");
                DSXML.AddElement("MeritDemerit");
                DSXML.SetAttribute("MeritDemerit", "Merit", "True");
                DSXML.SetAttribute("MeritDemerit", "DemeritClear", "True");
                DSXML.SetAttribute("MeritDemerit", "DemeritNotClear", "True");
                cd.SetXml("XmlData", DSXML.BaseElement);
                cd.Save();

                SetupDic.Add("���y", bool.Parse(DSXML.GetAttribute("MeritDemerit/@Merit")));
                SetupDic.Add("�g�٥��P�L", bool.Parse(DSXML.GetAttribute("MeritDemerit/@DemeritClear")));
                SetupDic.Add("�g�پP�L", bool.Parse(DSXML.GetAttribute("MeritDemerit/@DemeritNotClear")));
            }
            #endregion
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WeekReportCountConfig config = new WeekReportCountConfig("�Z�ż��g�O������");
            if (config.ShowDialog() == DialogResult.OK)
            {
                LoadPreference();
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}