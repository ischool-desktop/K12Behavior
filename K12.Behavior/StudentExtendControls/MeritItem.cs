using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using DevComponents.DotNetBar;
using Framework;
using Framework.Security;
using FISCA.LogAgent;
using K12.Data;

namespace K12.Behavior.StudentExtendControls
{
    [FISCA.Permission.FeatureCode("K12.Student.MeritItem", "獎勵記錄")]
    internal partial class MeritItem : DetailContentBase
    {

        //public static string FeatureCode = (Attribute.GetCustomAttribute(typeof(MeritItem), typeof(FeatureCodeAttribute)) as FeatureCodeAttribute).FeatureCode;
        //private FeatureAce _permission;

        internal static FISCA.Permission.FeatureAce UserPermission;
        private List<MeritRecord> _records = new List<MeritRecord>();

        BackgroundWorker BGW = new BackgroundWorker();
        bool BkWBool = false;

        public MeritItem()
        {
            InitializeComponent();

            BGW.DoWork += new DoWorkEventHandler(BkW_DoWork);
            BGW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BkW_RunWorkerCompleted);

            Merit.AfterInsert += new EventHandler<K12.Data.DataChangedEventArgs>(Merit_Changed);
            Merit.AfterDelete += new EventHandler<K12.Data.DataChangedEventArgs>(Merit_Changed);
            Merit.AfterUpdate += new EventHandler<K12.Data.DataChangedEventArgs>(Merit_Changed);

            //暫解法
            //Merit.Instance.ItemUpdated += new EventHandler<ItemUpdatedEventArgs>(Instance_ItemUpdated);

            //Merit.Instance.ItemUpdated += new EventHandler<ItemUpdatedEventArgs>(this.refreshUIData); //當 Cache Manager 中的資料有被更新時就重新更新畫面。

            this.Group = "獎勵記錄";    //設定毛毛蟲顯示的 Title ，如不設定會有 Exception。

            UserPermission = FISCA.Permission.UserAcl.Current[FISCA.Permission.FeatureCodeAttribute.GetCode(GetType())];
            //UserPermission = User.Acl[FCode.GetCode(GetType())];

            btnInsert.Visible = UserPermission.Editable;
            btnUpdate.Visible = UserPermission.Editable;
            btnDelete.Visible = UserPermission.Editable;
            btnView.Visible = UserPermission.Viewable & !UserPermission.Editable;
        }

        //暫解法
        //void Instance_ItemUpdated(object sender, ItemUpdatedEventArgs e)
        //{
        //    if (!BgW.IsBusy)
        //        BgW.RunWorkerAsync();
        //}

        void Merit_Changed(object sender, K12.Data.DataChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, K12.Data.DataChangedEventArgs>(Merit_Changed), sender, e);
            }
            else
            {
                if (this.PrimaryKey != "")
                {
                    this.Loading = true;

                    if (BGW.IsBusy)
                    {
                        BkWBool = true;
                    }
                    else
                    {
                        BGW.RunWorkerAsync();
                    }
                }
            }
        }

        /// <summary>
        /// 當主畫面檢視不同學生時，此毛毛蟲就被指定新的學生ID，就會呼叫此方法。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPrimaryKeyChanged(EventArgs e)
        {
            //this.Loading = true;    //畫面會呈現資料下載中的狀態(圓圈一直轉)
            //Merit.Instance.SyncDataBackground(this.PrimaryKey);   //SyncDataBackground 方法後會觸發 Demerit.Instance.ItemUpdateed 事件

            this.Loading = true;

            if (BGW.IsBusy)
            {
                BkWBool = true;
            }
            else
            {
                BGW.RunWorkerAsync();
            }
        }

        void BkW_DoWork(object sender, DoWorkEventArgs e)
        {
            _records.Clear();
            _records = Merit.SelectByStudentIDs(new string[] { this.PrimaryKey });
        }

        void BkW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (BkWBool)
            {
                BkWBool = false;
                BGW.RunWorkerAsync();
                return;
            }

            refreshUIData();

            this.Loading = false;
        }

        private void refreshUIData()
        {
            if (!this.SaveButtonVisible && !this.CancelButtonVisible && this.PrimaryKey.Contains(PrimaryKey))
            {
                //從Cache Manager 找到該學生的懲戒記錄，並更新到畫面上。
                this.listView.Items.Clear();

                _records.Sort(new Comparison<MeritRecord>(SchoolYearComparer));

                foreach (MeritRecord item in _records)
                {
                    #region 填值

                    ListViewItem itm = new ListViewItem(item.SchoolYear.ToString());
                    itm.SubItems.Add(item.Semester.ToString());
                    itm.SubItems.Add(item.OccurDate.ToShortDateString());
                    itm.SubItems.Add(item.MeritA.ToString());
                    itm.SubItems.Add(item.MeritB.ToString());
                    itm.SubItems.Add(item.MeritC.ToString());
                    itm.SubItems.Add(item.Reason);
                    itm.SubItems.Add(item.RegisterDate.HasValue ? item.RegisterDate.Value.ToShortDateString() : "");


                    //將資料加入ListView
                    itm.Tag = item;
                    listView.Items.Add(itm);
                    #endregion

                }
                //this.Loading = false;
                this.CancelButtonVisible = false;
                this.SaveButtonVisible = false;
                this.ContentValidated = true;
            }

            this.Loading = false;   //畫面就回覆資料已下載完成的畫面
        }

        //private string chengDateTime(DateTime x)
        //{
        //    if (x == null)
        //        return "";
        //    string time = x.ToString();
        //    int y = time.IndexOf(' ');
        //    return time.Remove(y);
        //}

        private void btnInsert_Click(object sender, EventArgs e)
        {
            List<StudentRecord> studs = new List<StudentRecord>();
            studs.Add(Student.SelectByID(this.PrimaryKey));
            MeritEditForm editForm = new MeritEditForm(studs);  //此編輯表單在新增模式下允許一次對多位學生新增相同的懲戒記錄，所以 Constructor 要傳入學生的集合。
            editForm.ShowDialog();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
            {
                MsgBox.Show("請先選擇一筆您要修改的資料");
                return;
            }
            if (listView.SelectedItems.Count > 1)
            {
                MsgBox.Show("選擇資料筆數過多，一次只能修改一筆資料");
                return;
            }

            MeritRecord record = (MeritRecord)this.listView.SelectedItems[0].Tag;
            MeritEditForm editForm = new MeritEditForm(record, UserPermission); 
            //此編輯表單在修改模式下，一次只能對一位學生的某一筆懲戒記錄進行修改，所以 Constructor 就傳入一個 Editor 物件。
            editForm.ShowDialog();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            btnUpdate_Click(sender, e);
            //if (listView.SelectedItems.Count == 0)
            //{
            //    MsgBox.Show("請先選擇一筆您要檢視的資料");
            //    return;
            //}
            //if (listView.SelectedItems.Count > 1)
            //{
            //    MsgBox.Show("選擇資料筆數過多，一次只能檢視一筆資料");
            //    return;
            //}

            //MeritRecord record = (MeritRecord)this.listView.SelectedItems[0].Tag;
            //MeritEditForm editForm = new MeritEditForm(record, UserPermission); 
            ////此編輯表單在修改模式下，一次只能對一位學生的某一筆懲戒記錄進行修改，所以 Constructor 就傳入一個 Editor 物件。
            //editForm.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
            {
                MsgBox.Show("必須選擇一筆以上資料!!");
                return;
            }

            List<MeritRecord> MeritList = new List<MeritRecord>();

            if (MsgBox.Show("確定將刪除所選擇之獎勵資料?", "確認", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            foreach (ListViewItem item in listView.SelectedItems)
            {
                MeritRecord record = item.Tag as MeritRecord;
                MeritList.Add(record);
            }     

            try
            {
                Merit.Delete(MeritList);
            }
            catch (Exception ex)
            {
                MsgBox.Show("刪除獎勵資料失敗!" + ex.Message);
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("學生「" + K12.Data.Student.SelectByID(this.PrimaryKey).Name + "」");
            foreach (MeritRecord merit in MeritList)
            {
                sb.AppendLine("日期「" + merit.OccurDate.ToShortDateString() + "」");
            }
            sb.AppendLine("獎勵資料已被刪除。");

            ApplicationLog.Log("學務系統.獎勵資料", "刪除學生獎勵資料", "student", this.PrimaryKey, sb.ToString());
            
            MsgBox.Show("刪除獎勵資料成功");
        }

        private int SchoolYearComparer(MeritRecord x, MeritRecord y)
        {
            return y.OccurDate.CompareTo(x.OccurDate);

            //string DataXX = x.OccurDate.ToShortDateString();



            //string xx = x.SchoolYear.ToString() + x.Semester.ToString();
            //string yy = y.SchoolYear.ToString() + y.Semester.ToString();



            //return xx.CompareTo(yy);
        }

        private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView.SelectedItems.Count == 1)
            {
                MeritEditForm editor = new MeritEditForm(listView.SelectedItems[0].Tag as MeritRecord, UserPermission);
                editor.ShowDialog();
            }
        }
    }
}