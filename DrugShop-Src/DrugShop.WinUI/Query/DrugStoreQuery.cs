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
    /// ҩ�����ѯ
    /// </summary>
     [Module("BC811E54-D093-415C-A5C6-EBC563FA9A75", "����ѯ", "�ṩҩ�����ѯ����")]
    public partial class DrugStoreQuery : UserControl
    {
        private IList<Store> storeList;
        private IList<Store> printStoreList;

        [ModuleStart()]
        public void StartEx()
        {
            this.Initialize();
        }

        public DrugStoreQuery()
        {
            InitializeComponent();
            this.controlAutoFocus1.NextKeys = new Keys[] { Keys.Enter };
        }

        #region ��ʼ��

        /// <summary>
        /// ��ʼ��
        /// </summary>
        internal void Initialize()
        {
            this.LoadDrugType();

            this.printStoreList = new List<Store>();
        }

        /// <summary>
        /// ҩƷ����
        /// </summary>
        private void LoadDrugType()
        {
            DataBindHelper.BindDrugTypeCmbBox2(this.cbxType);

            this.cbxType.SelectedIndexChanged += new EventHandler(cbxType_SelectedIndexChanged);
        }

        #endregion

        void cbxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbxType.SelectedItem != null)
            {
                this.SeachDrugShop();
            }
        }

        /// <summary>
        /// ����ѯ
        /// </summary>
        internal void SeachDrugShop()
        {
            if (this.storeList == null)
            {
                storeList = new List<Store>();
            }

            int type = this.cbxType.SelectedIndex <= 0 ? 0 : ((DrugShop.Entities.DrugType)((IList<DrugType>)this.cbxType.Tag)[this.cbxType.SelectedIndex]).Code;

            if (this.chkUPDown.Checked)
            {
                storeList = ServiceContainer.GetService<IDrugStoreService>().GetDrugList(type,this.txtSearch.Text.Trim(), this.chkUPDown.Checked);
            }
            else
            {
                storeList = ServiceContainer.GetService<IDrugStoreService>().GetDrugList(type, this.txtSearch.Text.Trim());
            }

            this.dmrstoreBindingSource.DataSource = null;
            this.dmrstoreBindingSource.DataSource = this.storeList;

            decimal jobCash = decimal.Zero;
            decimal saleCash = decimal.Zero;
            foreach (DrugShop.Entities.Store drug in storeList)
            {
                if (drug.Number != 0)
                {
                    jobCash += drug.JobPrice * drug.Number;
                    saleCash += drug.SalePrice * drug.Number;
                    this.printStoreList.Add(drug);
                }
            }

            this.lblTip.Text = "    ���м�¼" + storeList.Count.ToString() + "�����ܽ��" + saleCash.ToString("F2") + "Ԫ";

            this.dataGridView1.Tag = storeList;
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.SetReportName("ҩƷ���Ԥ����(ҩ��)");
            this.PrintPreview(this.storeList);
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in this.dataGridView1.Rows)
            {
                if (dr != null)
                {
                    int number = Convert.ToInt32(dr.Cells["numberDataGridViewTextBoxColumn"].Value);
                    int UpLimit = Convert.ToInt32(dr.Cells["UpLimitDataGridViewTextBoxColumn"].Value);
                    int DownLimit = Convert.ToInt32(dr.Cells["DownLimitDataGridViewTextBoxColumn"].Value);
                    if (number < DownLimit)
                    {
                        dr.DefaultCellStyle.BackColor = System.Drawing.Color.Blue;
                    }
                    else if (number > UpLimit)
                    {
                        dr.DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //ҩƷ����
            DataGridViewHelper.GetDrugTypeName(this.dataGridView1, e, "typeDataGridViewTextBoxColumn");
        }
    }
}
