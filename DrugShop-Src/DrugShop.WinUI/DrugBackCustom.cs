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
using EAS.Report.Controls;
using EAS.Explorer.Entities;
using EAS.Services;
using EAS.Modularization;
using DrugShop.Entities;using EAS.Data.ORM;
using DrugShop.BLL;

namespace DrugShop.WinUI
{
    /// <summary>
    /// �˿���ҩ��
    /// </summary>
    [Module("B4813A17-C097-4EC2-90BB-BC06489EE7C1", "�˿���ҩ", "�ṩ�˿���ҩ����")]
    public partial class DrugBackCustom : UserControl
    {
         //private int checkIndex = -1;
        private string LastBillCode;

        private IList<SBack>   backList = null;
        private IList<SOut> outList = null;
        private IList<Store> updateStoreList = null;

        [ModuleStart()]
        public void StartEx()
        {
            this.Initialize();
        }

        public DrugBackCustom()
        {
            InitializeComponent();
            this.controlAutoFocus1.NextKeys = new Keys[] { Keys.Enter };
        }

        internal void Initialize()
        {
            backList = new List<SBack>();
            outList = new List<SOut>();
            updateStoreList = new List<Store>();

            this.ledClock.DateTime = XContext.CurrentTime;
            this.LastBillCode = string.Empty;
        }

        internal void SeachDrugIn()
        {
            this.dmroutBindingSource.DataSource = null;

            IList<SOut> dataList = ServiceContainer.GetService<IDrugOutService>().GetDrugOutList(this.tbCustom.Text.Trim()
                , this.tbSeach.Text.Trim());

            this.outList.Clear();

            foreach (DrugShop.Entities.SOut drug in dataList)
            {
                if (drug.Number > 0)
                {
                    if (!this.outList.Contains(drug))
                        this.outList.Add(drug);
                }
            }

            this.dmroutBindingSource.DataSource = this.outList;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count < 1)
            {
                MessageBox.Show("��ѡ��Ҫ�˿��ҩƷ��", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dataGridView1.Focus();
                return;
            }

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            try
            {
                ServiceContainer.GetService<IDrugBackService>().DrugBackSave(updateStoreList, this.backList);

                MessageBox.Show("��ҩ�ɹ��� ");
            }
            catch (System.Exception exc)
            {
                MessageBox.Show("�ڴ���˿���ҩʱ�������󣬴�����Ϣ��" + exc.InnerException.Message, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            this.dmrcbackBindingSource.DataSource = null;
            this.dmroutBindingSource.DataSource = null;

            this.backList.Clear();

            this.btnPrint.Focus();
        }

        protected void btnSeach_Click(object sender, EventArgs e)
        {
            this.SeachDrugIn();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            this.SetReportName("�˿���ҩ��ϸ��");
            this.PrintPreview(this.backList);
        }

        protected void tbSeach_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SeachDrugIn();
            }
        }
        
        protected void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            this.InitCustomBackInfo(e.RowIndex);
        }

        protected void InitCustomBackInfo(int rowIndex)
        {
            DataGridViewRow row = dataGridView1.Rows[rowIndex];

            DrugShop.Entities.SOut drugOut = row.DataBoundItem as DrugShop.Entities.SOut;

            if (drugOut == null)
                return;

            BackNumberInput input = new BackNumberInput();

            input.StoreNumber = drugOut.Number;

            if (input.ShowDialog(this.ParentForm) != DialogResult.OK)
            {
                return;
            }

            var service = ServiceContainer.GetService<IDrugStoreService>();
            Store store = service.GetDrugStore(drugOut.Code, drugOut.DrugID, drugOut.ChinseName, drugOut.Spec, drugOut.TimeLimit);

            DrugShop.Entities.SBack drugBack = new DrugShop.Entities.SBack();

            ColumnCollection cols = drugOut.GetColumns();
            foreach (Property prop in cols)
            {
                if (drugBack.ContainsProperty(prop.Name))
                {
                    drugBack[prop.Name] = drugOut[prop.Name];
                }
            }

            //���湩Ӧ����ҩ��¼
            drugBack.ID = ServiceContainer.GetService<IDrugBackService>().GetSBackMaxID();
            drugBack.Number = input.Number;
            drugBack.Customname = drugOut.CustomName;

            this.backList.Add(drugBack);

            //���¿������
            store.Number += drugBack.Number;
            this.updateStoreList.Add(store);


            this.dmrcbackBindingSource.DataSource = null;
            this.dmrcbackBindingSource.DataSource = backList;
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            int index = this.dataGridView1.CurrentCell.RowIndex;

            if (index < 0)
                return;

            this.InitCustomBackInfo(index);
        }
    }
}
