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
    /// ҩƷ����
    /// </summary>
    [Module("1836CAB4-BC14-4A5E-81DB-C3A3D07E1B38", "ҩƷ����", "�ṩҩƷ������")]
    public partial class DrugLost : UserControl
    {
        private string LastBillCode;

        private IList<Lost>   lostList = null;
        private IList<Store> storeList = null;
        private IList<Store> updateStoreList = null;

        [ModuleStart()]
        public void StartEx()
        {
            this.Initialize();
        }

        public DrugLost()
        {
            InitializeComponent();
            this.controlAutoFocus1.NextKeys = new Keys[] { Keys.Enter };
        }

        internal void Initialize()
        {
            lostList = new List<Lost>();
            storeList = new List<Store>();
            updateStoreList = new List<Store>();

            this.ledClock.DateTime = XContext.CurrentTime;
            this.LastBillCode = string.Empty;
        }

        internal void SeachDrugIn()
        {
            this.dmrstoreBindingSource.DataSource = null;

            IList<Store> dataList = ServiceContainer.GetService<IDrugStoreService>().GetDrugList(this.tbSeach.Text.Trim());

            this.storeList.Clear();

            foreach (DrugShop.Entities.Store drug in dataList)
            {
                if (drug.Number > 0)
                {
                    if (!this.storeList.Contains(drug))
                        this.storeList.Add(drug);
                }
            }

            this.dmrstoreBindingSource.DataSource = this.storeList;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count < 1)
            {
                MessageBox.Show("��ѡ��Ҫ�˿��ҩƷ��", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dataGridView1.Focus();
                return;
            }

            if (this.tbCause.Text.Trim() == string.Empty)
            {
                MessageBox.Show("�����뱨��ԭ��", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            try
            {
                ServiceContainer.GetService<IDrugLostService>().DrugLostSave(updateStoreList, this.lostList);

                MessageBox.Show("����ɹ��� ");
            }
            catch (System.Exception exc)
            {
                MessageBox.Show("�ڴ���Ӧ����ҩʱ�������󣬴�����Ϣ��" + exc.Message, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            this.dmrlostBindingSource.DataSource = null;
            this.dmrstoreBindingSource.DataSource = null;

            this.btnPrint.Focus();
        }

        protected void btnSeach_Click(object sender, EventArgs e)
        {
            this.SeachDrugIn();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            this.SetReportName("ҩƷ������ϸ��(ҩ��)");
            this.PrintPreview(this.lostList);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            EAS.Application.Instance.CloseModule(this);
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

            DrugShop.Entities.Store DrugShop = row.DataBoundItem as DrugShop.Entities.Store;

            if (DrugShop == null)
                return;

            NumberInput input = new NumberInput();

            input.StoreNumber = DrugShop.Number;

            if (input.ShowDialog(this.ParentForm) != DialogResult.OK)
            {
                return;
            }

            DrugShop.Entities.Store store = new Entities.Store();

            DrugShop.Entities.Lost drugLost = new DrugShop.Entities.Lost();

            ColumnCollection cols = DrugShop.GetColumns();
            foreach (Property prop in cols)
            {
                if (drugLost.ContainsProperty(prop.Name))
                {
                    drugLost[prop.Name] = DrugShop[prop.Name];
                }

                if (store.ContainsProperty(prop.Name))
                {
                    store[prop.Name] = DrugShop[prop.Name];
                }
            }

            //���湩Ӧ����ҩ��¼
            drugLost.ID = ServiceContainer.GetService<IDrugLostService>().GetMaxID();
            drugLost.Number = input.Number;
            this.lostList.Add(drugLost);

            //���¿������
            store.Number -= drugLost.Number;
            this.updateStoreList.Add(store);

            this.dmrlostBindingSource.DataSource = null;
            this.dmrlostBindingSource.DataSource = lostList;

            CaclSumCash();
        }

        void CaclSumCash()
        {
            decimal sumCash1 = decimal.Zero;
            decimal sumCash2 = decimal.Zero;

            foreach (DataGridViewRow row in this.dataGridView2.Rows)
            {
                DrugShop.Entities.Lost item = row.DataBoundItem as DrugShop.Entities.Lost;
                if (item != null)
                {
                    decimal cash1 = item.JobPrice * item.Number;
                    decimal cash2 = item.SalePrice * item.Number;

                    sumCash1 += cash1;
                    sumCash2 += cash2;
                }
            }

            this.lbTip.Text = "����ҩƷ����" + this.dataGridView2.Rows.Count.ToString() + "�����ܽ��" + sumCash2.ToString("F2") + "Ԫ";
        }
    }
}
