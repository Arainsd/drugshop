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
    /// ��Ӧ���˿��ѯ
    /// </summary>
    [Module("CAC02453-E207-466C-8279-16CFD056754E","�˿���ҩ��ѯ","�˿���ҩ��Ϣ��ѯ")]
    public partial class DrugBackCustomQuery : UserControl
    {
        private IList<SBack> backList;

        [ModuleStart()]
        public void StartEx()
        {
            this.Initialize();
        }

        public DrugBackCustomQuery()
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

        internal void SeachDrugBack()
        {
            this.dmrcbackBindingSource.DataSource = null;

            if (this.backList == null)
            {
                this.backList = new List<SBack>();
            }

           this.backList= ServiceContainer.GetService<IDrugBackService>().GetDrugCustomBackList(this.tbCustomName.Text.Trim(),this.tbSeach.Text, this.dtpStart.Value, this.dtpEnd.Value);

           this.dmrcbackBindingSource.DataSource = this.backList;

            List<string> bills = new List<string>(backList.Count);
            decimal jobCash = decimal.Zero;
            decimal saleCash = decimal.Zero;

            foreach (DrugShop.Entities.SBack drugBack in backList)
            {
                jobCash += drugBack.JobPrice * drugBack.Number;
                saleCash += drugBack.SalePrice * drugBack.Number;
            }

            this.lbTip.Text = "����ҩƷ��¼" + bills.Count.ToString() + "�����ܽ��" + saleCash.ToString("F2") + "Ԫ";

        }

        private void tbSeach_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SeachDrugBack();
            }
        }

        private void btnSeach_Click(object sender, EventArgs e)
        {
            this.SeachDrugBack();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.SetReportName("�˿���ҩ��ϸ��");
            this.PrintPreview(this.backList);
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //ҩƷ����
            DataGridViewHelper.GetDrugTypeName(this.dataGridView1, e, "typeDataGridViewTextBoxColumn");
        }

        private void tbSeach_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            this.SeachDrugBack();
        }

        private void tbCustomName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            this.SeachDrugBack();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tbSeach_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
