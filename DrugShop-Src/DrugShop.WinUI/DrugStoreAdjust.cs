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
    /// ҩƷ�̵������
    /// </summary>
    [Module("A4950C76-FEF5-4C25-861B-94214D10FACD", "�̵����", "�ṩҩ��ҩƷ����������")]
    public partial class DrugStoreAdjust : UserControl
    {
        private IList<Inventory> storeCountList = null;
        public DrugStoreAdjust()
        {
            InitializeComponent();
        }

        internal void Initialize()
        {
            this.ledClock.DateTime = XContext.CurrentTime;

            this.storeCountList = new List<Inventory>();

            this.BindStoreCountDate();
        }

        internal void BindStoreCountDate()
        {
            IList<object> dateList = ServiceContainer.GetService<IDrugStoreCountService>().GetDrugStoreCountDateList();

            this.cbxDate.Items.Clear();

            foreach (object task in dateList)
            {
                DateTime time = DateTime.Parse(task.ToString());
                this.cbxDate.Items.Add(time.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            if (this.cbxDate.Items.Count == 0)
            {
                this.btnSave.Enabled = this.btnSeach.Enabled = this.cbMid.Enabled = true;
            }
            else
            {
                this.cbxDate.SelectedIndexChanged += new EventHandler(cbxDate_SelectedIndexChanged);
            }
        }

        internal void SeachStoreCount()
        {
            this.dmrcountBindingSource.DataSource = null;

            DateTime dt = this.cbxDate.Text == "" ? DateTime.Now.Date : Convert.ToDateTime(this.cbxDate.Text);

            int isAll = this.cbMid.Checked ? 1 : 0;

            this.storeCountList = ServiceContainer.GetService<IDrugStoreCountService>().GetDrugStoreCountList(isAll, dt);

            this.dmrcountBindingSource.DataSource = this.storeCountList;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count <= 0)
            {
                MessageBox.Show("����ѡ��ҩƷ�̵����ݣ�", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            DrugShop.Entities.Inventory count = ServiceContainer.GetService<IDrugStoreCountService>().GetDrugStoreCountList(Convert.ToDateTime(this.cbxDate.Text.Trim()));

            if (count == null)
                return;

            if (count.State>0)
            {
                MessageBox.Show(this.cbxDate.Text + "�̴������Ѿ����й��������������ٴν��п�������", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ServiceContainer.GetService<IDrugStoreCountService>().IsNeedUpdate(Convert.ToDateTime(this.cbxDate.Text.Trim())))
            {
                MessageBox.Show(this.cbxDate.Text + "�̴����ݲ�����ӯ�����ݣ����ܽ��п�������", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            try
            {
                ServiceContainer.GetService<IDrugStoreCountService>().DrugStoreCountAdjust(this.storeCountList);
            }
            catch (System.Exception exc)
            {
                MessageBox.Show("�ڱ���ҩƷ�̵�ʱ�������󣬴�����Ϣ��" + exc.Message, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            this.BindStoreCountDate();
            this.dmrcountBindingSource.DataSource = null;
        }

        #region �����¼�

        private void btnSeach_Click(object sender, EventArgs e)
        {
            this.SeachStoreCount();
        }

        private void cbxDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Information.IsDate(this.cbxDate.Text))
            {
                this.SeachStoreCount();
            }
        }

        private void cbMid_CheckedChanged(object sender, EventArgs e)
        {
            this.SeachStoreCount();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewRow dr = dataGridView1.Rows[e.RowIndex];
            int RealNumber = 0;
            if (dr != null)
            {
                RealNumber = Convert.ToInt32(dr.Cells["colRealNumber"].Value);
                int number = Convert.ToInt32(dr.Cells["numberDataGridViewTextBoxColumn"].Value);
                decimal JobPrice = Convert.ToDecimal(dr.Cells["JobPriceDataGridViewTextBoxColumn"].Value);
                decimal SalePrice = Convert.ToDecimal(dr.Cells["SalePriceDataGridViewTextBoxColumn"].Value);
                int x = number - RealNumber;
                int subNumber = Math.Abs(x);
                // 
                if (this.dataGridView1.Columns[e.ColumnIndex].Name == "colflag")
                {
                    string flag = x == 0 ? "��ƽ" : x < 0 ? "��ӯ" : "�̿�";
                    e.Value = flag;
                }
                // 
                else if (this.dataGridView1.Columns[e.ColumnIndex].Name == "colykNum")
                {
                    e.Value = subNumber.ToString("F2");
                }
                else if (this.dataGridView1.Columns[e.ColumnIndex].Name == "colJobCash")
                {
                    decimal cash1 = subNumber * JobPrice;
                    e.Value = cash1.ToString("F2");
                }
                else if (this.dataGridView1.Columns[e.ColumnIndex].Name == "colSaleCash")
                {
                    decimal cash2 = subNumber * SalePrice;
                    e.Value = cash2.ToString("F2");
                }
            }
        }

        #endregion
    }
}
