using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;

namespace K12.Behavior.StudentExtendControls
{
    //���m�n��(�S������)
    public partial class PeriodControl : UserControl
    {
        public PeriodControl()
        {
            InitializeComponent();            

            //this.Font = DevComponents.DotNetBar.FontStyles.General;
            this.Width = 45;
        }

        public LabelX Label
        {
            get { return label; }
        }

        public TextBoxX TextBox
        {
            get { return textBox; }
        }
    }
}