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
using EAS.Services;
using EAS.Modularization;
using DrugShop.Entities;using EAS.Data.ORM;
using DrugShop.BLL;

namespace DrugShop.WinUI
{
    /// <summary>
    /// ҩƷ���ۡ�
    /// </summary>
    [Module("6E695C71-AA15-4DF9-9B6D-022D7E17A492", "ҩƷ����", "�ṩҩ��ҩƷ���۹���")]
    public partial class DrugChangePrice : UserControl
    {
        private IList<CPrice> drugChangePriceList;
        private IList<Store> storeList;
        private IList<Store> updateStoreList;

        [ModuleStart()]
        public void StartEx()
        {
            this.Initialize();
        }

        public DrugChangePrice()
        {
            InitializeComponent();
            this.controlAutoFocus1.NextKeys = new Keys[] { Keys.Enter };
        }

        internal void Initialize()
        {
            this.ledClock.DateTime = XContext.CurrentTime;

            DataBindHelper.BindDrugTypeCmbBox2(this.cbxType);

            drugChangePriceList = new List<CPrice>();
            updateStoreList = new List<Store>();
        }

        /// <summary>
        /// ����Ҫ���۵�
        /// </summary>
        internal void SeachDrugShop()
        {
            if (storeList == null)
                storeList = new List<Store>();

            this.dmrstoreBindingSource.DataSource = null;

            IList<Store> dataList = ServiceContainer.GetService<IDrugStoreService>().GetDrugList((int)this.cbxType.SelectedValue, this.tbSeach.Text.Trim());

            for (int i = 0; i < dataList.Count; i++)
            {
                DrugShop.Entities.Store drug = dataList[i];
                if (drug.Number >0)
                {
                    if (!storeList.Contains(drug))
                        storeList.Add(drug);
                }
            }
            this.dmrstoreBindingSource.DataSource = storeList;
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count < 1)
            {
                MessageBox.Show("��ѡ��Ҫ���۵�ҩƷ��", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            try
            {
                ServiceContainer.GetService<IDrugChangePriceService>().ChangePrice(drugChangePriceList, updateStoreList);

                MessageBox.Show("���۳ɹ���");
            }
            catch (System.Exception exc)
            {
                MessageBox.Show("�ڴ���ҩƷ����ʱ�������󣬴�����Ϣ��" + exc.Message, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            this.dmrstoreBindingSource.DataSource = null;
            this.dmrcpriceBindingSource.DataSource = null;
        }

        private void tbSeach_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SeachDrugShop();
            }
        }

        private void btnSeach_Click(object sender, EventArgs e)
        {
            this.SeachDrugShop();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //this.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            this.ChangePriceInfo(e.RowIndex);

            this.dmrcpriceBindingSource.DataSource = null;

            this.dmrcpriceBindingSource.DataSource = drugChangePriceList;
        }

        /// <summary>
        /// ��ʼ��Ҫ������ҩƷ��Ϣ
        /// </summary>
        /// <param name="rowIndex">��������</param>
        private void ChangePriceInfo(int rowIndex)
        {
            DataGridViewRow row = dataGridView1.Rows[rowIndex];

            DrugShop.Entities.Store store = row.DataBoundItem as DrugShop.Entities.Store;

            if (store == null)
                return;

            if (updateStoreList.Contains(store))
                return;

            SalePriceInput input = new SalePriceInput();
            input.OldPrice = store.SalePrice;

            if (input.ShowDialog(this.ParentForm) != DialogResult.OK)
            {
                return;
            }

            DrugShop.Entities.Store dataStore = new Entities.Store();
            DrugShop.Entities.CPrice dataObject = new DrugShop.Entities.CPrice();

            ColumnCollection cols = store.GetColumns();
            foreach (Property prop in cols)
            {
                if (dataObject.ContainsProperty(prop.Name))
                {
                    dataObject[prop.Name] = store[prop];
                }

                if (dataStore.ContainsProperty(prop.Name))
                {
                    dataStore[prop.Name] = store[prop];
                }
            }

            //dataObject.Billcode = this.tbBillCode.Tag.ToString();
            dataObject.NSalePrice = input.Number;
            dataObject.Cause = this.tbCause.Text;
            //dataObject.Operator = 0;

            //��ӵ����ۼ�����
            drugChangePriceList.Add(dataObject);

            dataStore.SalePrice = input.Number;
            dataStore.JobPrice = input.Number;

            //��ӵ�Ҫ���µĿ���¼��
            updateStoreList.Add(dataStore);
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            int index = dataGridView1.CurrentCell.RowIndex;

            if (index < 0)
                return;

            this.ChangePriceInfo(index);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.SetReportName("ҩƷ������ϸ��(ҩ��)");
            this.PrintPreview(this.drugChangePriceList);
        }
    }
}
