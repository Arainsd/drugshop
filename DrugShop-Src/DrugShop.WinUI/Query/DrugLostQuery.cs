using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using EAS.Data.ORM;
using EAS.Data.Access;

using Microsoft.VisualBasic;
using EAS.Modularization;
using DrugShop.Entities;
using DrugShop.BLL;

namespace DrugShop.WinUI
{
    /// <summary>
    /// ҩƷ�����ѯ
    /// </summary>
    [Module("F596EEE6-9C61-4A9D-8875-EFA94007DD83", "�����ѯ", "��Ϣ��ѯ")]
    public partial class DrugLostQuery : UserControl
    {
        private IList<Lost> lostList = null;

        [ModuleStart()]
        public void StartEx()
        {
            this.Initialize();
        }

        public DrugLostQuery()
        {
            InitializeComponent();
            this.controlAutoFocus1.NextKeys = new Keys[] { Keys.Enter };
        }

        internal void Initialize()
        {
            this.ledClock.DateTime = XContext.CurrentTime;

            this.dtpStart.Value = XContext.CurrentTime;
            this.dtpEnd.Value = XContext.CurrentTime;
        }

        internal void SeachDrugLost()
        {
            this.dmrlostBindingSource.DataSource = null;

            if (this.lostList == null)
            {
                lostList = new List<Lost>();
            }

            lostList = EAS.Services.ServiceContainer.GetService<IDrugLostService>().GetDrugLostList(this.tbCause.Text.Trim(), this.tbSeach.Text.Trim(), this.dtpStart.Value, this.dtpEnd.Value);

            this.dmrlostBindingSource.DataSource = this.lostList;

            List<string> bills = new List<string>(lostList.Count);
            decimal jobCash = decimal.Zero;
            decimal saleCash = decimal.Zero;

            foreach (DrugShop.Entities.Lost drugLost in lostList)
            {
                jobCash += drugLost.JobPrice * drugLost.Number;
                saleCash += drugLost.SalePrice * drugLost.Number;
            }

            this.lbTip.Text = "���м�¼" + bills.Count.ToString() + "�����������" + jobCash.ToString("F2") + "Ԫ�����۽��" + saleCash.ToString("F2") + "Ԫ";

            if (this.dataGridView1.Rows.Count > 0)
            {
                this.dataGridView1.Tag = lostList;

                this.dataGridView1.Focus();
            }
        }

        private void tbSeach_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SeachDrugLost();
            }
        }

        private void btnSeach_Click(object sender, EventArgs e)
        {
            this.SeachDrugLost();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.SetReportName("ҩƷ������ϸ��(ҩ��)");
            this.PrintPreview(this.lostList);
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewHelper.GetDrugTypeName(this.dataGridView1, e, "Type");
        }

        private void tbCause_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.SeachDrugLost();
        }
    }
}
