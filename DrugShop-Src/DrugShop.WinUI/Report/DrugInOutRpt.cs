using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using EAS.Data.Access;
using EAS.Data.ORM;
using EAS.Services;
using EAS.Explorer.Entities;
using EAS.Modularization;
using DrugShop.Entities;using EAS.Data.ORM;
using DrugShop.BLL;
using DrugShop.WinUI.RDL;

namespace DrugShop.WinUI
{
    [Module("D0EF19B9-A38B-4965-A7C1-783CDDB82B33", "��֧����", "ҩƷ�̵���֧����")]
    public partial class DrugInOutCount : BaseReport
    {
        //[ModuleStart()]
        //public void StartEx()
        //{
        //    this.InitInfo();
        //}

        public DrugInOutCount()
        {
            InitializeComponent();
        }

        public override void InitInfo()
        {
            this.ReportTitle = "ҩƷ��֧����";
            this.ContentControl = new DrugInOutSearchControl();
        }

        public override void Begin_Seach(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                DrugInOutSearchControl searchControl = this.ContentControl as DrugInOutSearchControl;

                this.Report.Name = "ҩ����֧����";
                this.Report.Refresh();
                //ͳ������
                IList<DrugInOut> DataList = new List<DrugInOut>();
                DataList = ServiceContainer.GetService<IDrugStoreCountService>().GetDrugInOutList(searchControl.StartTime,searchControl.EndTime);

                this.ShowReport = this.Report;
                this.DataSource = DataList;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}