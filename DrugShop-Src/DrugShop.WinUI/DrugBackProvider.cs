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
    /// ��Ӧ����ҩ��
    /// </summary>
    [Module("10043D84-060C-4DDD-AACB-2D0103D6A5CF", "��Ӧ����ҩ", "�ṩҩ�깩Ӧ���˻�����")]
    public partial class DrugBackProvider : UserControl
    {
        //private int checkIndex = -1;
        private string LastBillCode;

        private IList<PBack>   backList = null;
        private IList<Store> storeList = null;
        private IList<Store> updateStoreList = null;

        [ModuleStart()]
        public void StartEx()
        {
            this.Initialize();
            this.controlAutoFocus1.NextKeys = new Keys[] { Keys.Enter };
        }

        public DrugBackProvider()
        {
            InitializeComponent();
        }

        internal void Initialize()
        {
            backList = new List<PBack>();
            storeList = new List<Store>();
            updateStoreList = new List<Store>();

            this.ledClock.DateTime = XContext.CurrentTime;
            this.tbProvider.Tag = 0;
            this.LastBillCode = string.Empty;
        }

        internal void SeachDrugIn()
        {
            this.dmrstoreBindingSource.DataSource = null;

            IList<Store> dataList = ServiceContainer.GetService<IDrugStoreService>().GetDrugList(this.tbSeach.Text, this.tbProvider.Text.Trim());

            foreach (DrugShop.Entities.Store drug in dataList)
            {
                if (drug.Number > 0)
                {
                    if (!storeList.Contains(drug))
                        storeList.Add(drug);
                }
            }

            this.dmrstoreBindingSource.DataSource = storeList;
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
                ServiceContainer.GetService<IDrugBackService>().DrugBackSave(updateStoreList,this.backList);

                MessageBox.Show("�˿�ɹ��� ");
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

            this.dmrpbackBindingSource.DataSource = null;
            this.dmrstoreBindingSource.DataSource = null;

            this.btnPrint.Focus();
        }

        protected void btnSeach_Click(object sender, EventArgs e)
        {
            this.SeachDrugIn();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            this.SetReportName("��Ӧ����ҩ��ϸ��(ҩ��)");
            this.PrintPreview(this.backList);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            //this.Close();
        }
        
        protected void tbSeach_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SeachDrugIn();
            }
        }

        protected void cbxProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SeachDrugIn();
        }

        protected void btnProvider_Click(object sender, EventArgs e)
        {
            ProviderSelect input = new ProviderSelect();

            if (input.ShowDialog(this.ParentForm) == DialogResult.OK)
            {
                this.tbProvider.Tag = input.Provider.ID;
                this.tbProvider.Text = input.Provider.Name;
            }
        }

        protected void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            this.InitProviderBackInfo(e.RowIndex);
        }

        protected void InitProviderBackInfo(int rowIndex)
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

            DrugShop.Entities.PBack drugBack = new DrugShop.Entities.PBack();

            ColumnCollection cols = DrugShop.GetColumns();
            foreach (Property prop in cols)
            {
                if (drugBack.ContainsProperty(prop.Name))
                {
                    drugBack[prop.Name] = DrugShop[prop.Name];
                }

                if (store.ContainsProperty(prop.Name))
                {
                    store[prop.Name] = DrugShop[prop.Name];
                }
            }

            //���湩Ӧ����ҩ��¼
            drugBack.Id = ServiceContainer.GetService<IDrugBackService>().GetPBackMaxID(); 
            drugBack.Number = input.Number;
            this.backList.Add(drugBack);

            //���¿������
            store.Number -= drugBack.Number;
            this.updateStoreList.Add(store);
        }
    }
}
