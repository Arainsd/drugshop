using System;
using System.Collections.Generic;
using System.Windows.Forms;



using EAS.Modularization;
using EAS.Services;
using DrugShop.Entities;
using DrugShop.BLL;

namespace DrugShop.WinUI
{
    /// <summary>
    /// ҩƷ���۲�ѯ
    /// </summary>
     [Module("372E2C98-FF4A-4EF8-9734-472A3DA44836", "���۲�ѯ", "��Ϣ��ѯ")]
    public partial class DrugChangePriceQuery : UserControl
    {
        private IList<CPrice> cpList = null;

        [ModuleStart()]
        public void StartEx()
        {
            this.Initialize();
        }

        public DrugChangePriceQuery()
        {
            InitializeComponent();
            this.controlAutoFocus1.NextKeys = new Keys[] { Keys.Enter };
            //this.controlAutoFocus1.NextKeys = new Keys[] { Keys.Escape };
        } 

        internal void Initialize()
        {
            this.ledClock.DateTime = XContext.CurrentTime;
            this.dtpStart.Value = XContext.CurrentTime;
            this.dtpEnd.Value = XContext.CurrentTime;
        }

        internal void SeachDrugChangePrice()
        {
            if (this.cpList == null)
            {
                this.cpList = new List<CPrice>();
            }

            this.cpList = ServiceContainer.GetService<IDrugChangePriceService>().GetDrugChangePriceList(this.tbCause.Text, this.tbSeach.Text, this.dtpStart.Value, this.dtpEnd.Value);

            this.dmrcpriceBindingSource.DataSource = this.cpList;

            decimal saleCash = decimal.Zero;

            foreach (DrugShop.Entities.CPrice drugCP in cpList)
            {
                saleCash += (drugCP.NSalePrice - drugCP.SalePrice) * drugCP.Number;
            }

            this.lbTip.Text = "����ҩƷ��¼" + cpList.Count.ToString() + "�������۲��" + saleCash.ToString("F2") + "Ԫ";
        }

        private void tbSeach_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SeachDrugChangePrice();
            }
        }

        private void btnSeach_Click(object sender, EventArgs e)
        {
            this.SeachDrugChangePrice();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.SetReportName("ҩƷ������ϸ��(ҩ��)");
            this.PrintPreview(this.cpList);
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //ҩƷ����
            //ҩƷ����
            DataGridViewHelper.GetDrugTypeName(this.dataGridView1, e, "Type");
        }

        private void tbCause_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            this.SeachDrugChangePrice();
        }  
    }
}
