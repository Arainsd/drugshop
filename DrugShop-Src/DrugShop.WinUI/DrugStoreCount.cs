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
    /// ҩƷ�̴档
    /// </summary>
    [Module("94CA27D2-EA13-407E-BE9C-1B4E15A98C50", "ҩƷ�̴�", "ҩƷ�̴����")]
    public partial class DrugStoreCount : UserControl
    {
        private IList<Inventory> storeCountList = null;
        private IList<Inventory> updateStoreCountList = null;
        private IList<Inventory> printStoreCountList = null;

        public DrugStoreCount()
        {
            InitializeComponent();
        }

        [ModuleStart]
        public void StartEX()
        {
            this.Initialize();
        }

        #region ��ʼ��

        internal void Initialize()
        {
            this.BindStoreCountDate();

            storeCountList = new List<Inventory>();
            updateStoreCountList = new List<Inventory>();
            printStoreCountList = new List<Inventory>();
        }

        /// <summary>
        /// �����̴����������Ϣ
        /// </summary>
        internal void BindStoreCountDate()
        {
            IList<object> dateList = ServiceContainer.GetService<IDrugStoreCountService>().GetDrugStoreCountDateList();

            this.cbxDate.Items.Clear();

            foreach (object task in dateList)
            {
                DateTime time = DateTime.Parse(task.ToString());
                this.cbxDate.Items.Add(time.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            this.cbxDate.Text = string.Empty;
            this.cbxDate.SelectedIndexChanged += new EventHandler(cbxDate_SelectedIndexChanged);
        }

        #endregion

        /// <summary>
        /// �����̵���
        /// </summary>
        internal void SeachStoreCount()
        {
            if (this.cbxDate.Text == String.Empty)
            {
                MessageBox.Show("��ѡ��һ���������ڣ�", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            storeCountList=  ServiceContainer.GetService<IDrugStoreCountService>().GetDrugStoreCountList(this.tbSeach.Text, Convert.ToDateTime(this.cbxDate.Text));

            this.dmrcountBindingSource1.DataSource = null;
            this.dmrcountBindingSource1.DataSource = storeCountList;

        }

        #region ����

        private void UpdateStoreCountInfo(int index)
        {
            DataGridViewRow row = dataGridView1.Rows[index];

            DrugShop.Entities.Inventory count = row.DataBoundItem as DrugShop.Entities.Inventory;

            if (count == null)
                return;

            if (count.State == 1)
            {
                MessageBox.Show("�̵���Ϣ�ѽ��й����������޷��༭��", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //��������ʵ�ʿ��ĶԻ���
            StoreCountNumberInput input = new StoreCountNumberInput();
            input.StoreNumber = count.Number;

            if (input.ShowDialog(this.ParentForm) != DialogResult.OK)
                return;

            //���������ʵ��ֵ��ʼ����ص���Ϣ

            DrugShop.Entities.Inventory item = new Entities.Inventory();

            ColumnCollection cols = count.GetColumns();
            foreach (Property prop in cols)
            {
                if (item.ContainsProperty(prop.Name))
                    item[prop.Name] = count[prop];
            }

            item.RealNumber = input.Number;

            this.updateStoreCountList.Add(item);

            //���°��б��е���Ϣ
            this.dmrcountBindingSource.DataSource = null;
            this.dmrcountBindingSource.DataSource = this.updateStoreCountList;
        }

        //����
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.dataGridView2.Rows.Count <= 0)
            {
                MessageBox.Show("��ѡ�����³������������ʴ���Ϣ��", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            //ִ������
            try
            {
                ServiceContainer.GetService<IDrugStoreCountService>().DrugStoreCountSave(this.updateStoreCountList);
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
        }

        #endregion

        #region ��ӡ

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.SetReportName("ҩƷ�̵�ͳ�Ʊ�(ҩ��)");
            this.PrintPreview(this.printStoreCountList);
        }

        #endregion

        #region �������

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            this.UpdateStoreCountInfo(e.RowIndex);
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            if (index < 0)
                return;

            this.UpdateStoreCountInfo(index);
        }

        private void cbxDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Information.IsDate(this.cbxDate.Text))
            {
                this.SeachStoreCount();
            }
        }

        private void tbSeach_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                  this.SeachStoreCount();
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            this.SeachStoreCount();
        }

        #endregion

        private void btnCount_Click(object sender, EventArgs e)
        {
            //�̴�
            this.Cursor = Cursors.WaitCursor;

            try
            {
                ServiceContainer.GetService<IDrugStoreCountService>().DrugStoreCount();
            }
            catch(System.Exception ex)
            {
                MessageBox.Show("�ڽ��п���̴�Ĺ����г������´���" + ex.InnerException.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}
