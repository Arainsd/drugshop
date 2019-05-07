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
    [Module("CAC02453-E207-466C-8279-16CFD056754E","��Ӧ���˿��ѯ","ҩ���ѯ")]
    public partial class DrugBackProviderQuery : UserControl
    {
        private IList<PBack> backList;

        [ModuleStart()]
        public void StartEx()
        {
            this.Initialize();
        }

        public DrugBackProviderQuery()
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

            this.tbProvider.Tag = 0;
        }

        internal void SeachDrugBack()
        {
            this.drugBackProviderExvBindingSource.DataSource = null;

            if (this.backList == null)
            {
                this.backList = new List<PBack>();
            }

           this.backList= ServiceContainer.GetService<IDrugBackService>().GetDrugBackList(this.tbProvider.Tag==null?string.Empty:this.tbProvider.Tag.ToString(), this.tbSeach.Text, this.dtpStart.Value, this.dtpEnd.Value);

            this.drugBackProviderExvBindingSource.DataSource = this.backList;

            List<string> bills = new List<string>(backList.Count);
            decimal jobCash = decimal.Zero;
            decimal saleCash = decimal.Zero;

            foreach (DrugShop.Entities.PBack drugBack in backList)
            {
                jobCash += drugBack.JobPrice * drugBack.Number;
                saleCash += drugBack.SalePrice * drugBack.Number;
            }

            this.lbTip.Text = "����ҩƷ��¼" + bills.Count.ToString() + "�����ܽ��" + saleCash.ToString("F2") + "Ԫ";

        }

        private void btnProvider_Click(object sender, EventArgs e)
        {
            ProviderSelect input = new ProviderSelect();

            if (input.ShowDialog(this.ParentForm) == DialogResult.OK)
            {
                this.tbProvider.Tag = input.Provider.ID;
                this.tbProvider.Text = input.Provider.Name;
            }
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
            this.SetReportName("��Ӧ����ҩ��ϸ��(ҩ��)");
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
    }
}
