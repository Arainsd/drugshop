using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using EAS.Modularization;
using DrugShop.Entities;using EAS.Data.ORM;

namespace DrugShop.WinUI
{
    /// <summary>
    /// ��Ӧ��ά���б�
    /// </summary>
    [Module("4921F5B3-5A2A-402B-95AA-D97A327C7FB0", "��Ӧ��", "�ṩ��Ӧ����Ϣά������")]
    public partial class ProvideList : UserControl
    {
        public event EventHandler Close;

        private IList<Provider> providerList;
        /// <summary>
        /// ���� Exited �¼���
        /// </summary>
        /// <param name="e">�¼�������</param>
        protected void OnExited(EventArgs e)
        {
            if (this.Close != null)
            {
                this.Close(this, e);
            }
        }

        [ModuleStart()]
        public void StartEx()
        {
            this.Initialize();
        }

        public ProvideList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        public void Initialize()
        {
            this.InitList();
            this.EnabledItemMenu(false);
        }

        #region �˵���������

        private void btnItemAdd_Click(object sender, EventArgs e)
        {
            this.ItemAdd();
        }

        private void btnItemProperty_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;

            if (rowIndex < 0)
            {
                return;
            }

            DataGridViewRow row = dataGridView1.Rows[rowIndex];

            this.ItemProperty(row);
        }

        private void btnItemDelete_Click(object sender, EventArgs e)
        {
            this.ItemDelete();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ListSearch();
        }

        private void cmiItemAdd_Click(object sender, EventArgs e)
        {
            this.ItemAdd();
        }

        private void cmiItemProperty_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;

            if (rowIndex < 0)
            {
                return;
            }

            DataGridViewRow row = dataGridView1.Rows[rowIndex];

            this.ItemProperty(row);
        }

        private void cmiItemDelete_Click(object sender, EventArgs e)
        {
            this.ItemDelete();
        }

        private void cmiItemRefresh_Click(object sender, EventArgs e)
        {
            this.ListRefresh();
        }

        private void cmiItemSearch_Click(object sender, EventArgs e)
        {
            this.ListSearch();
        }

        private void tsmiNoShowAll_Click(object sender, EventArgs e)
        {
            this.InitList();
        }

        private void tsmiShowAll_Click(object sender, EventArgs e)
        {
            this.InitList();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.SetReportName("ҩƷ����");
            this.PrintPreview(this.providerList);
        }

        #endregion

        #region ��¼ά��

        /// <summary>
        /// ��¼��ӡ�
        /// </summary>
        protected void ItemAdd()
        {
            ProviderEditorForm o = new ProviderEditorForm();

            if(o.ShowDialog() == DialogResult.OK)
            {
                DrugShop.Entities.Provider var = o.Provider;

                this.providerList.Insert(0,var);

                this.providerBindingSource.DataSource = null;
                this.providerBindingSource.DataSource = this.providerList;
            }
        }

        private void InitRowInfo(DataGridViewRow row, DrugShop.Entities.Provider var)
        {
            row.Cells[0].Value = var.Name;
            row.Cells[1].Value = var.Tel;
            row.Cells[2].Value = var.Fax;
            row.Cells[3].Value = var.EMail;
            row.Cells[4].Value = var.Contact;
            row.Cells[5].Value = var.Phone;
            row.Cells[6].Value = var.Mobile;
            row.Cells[7].Value = var.Address;
            row.Cells[8].Value = var.InputCode;
        }

        /// <summary>
        /// ��¼���ԡ�
        /// </summary>
        protected void ItemProperty(DataGridViewRow row)
        {
            DrugShop.Entities.Provider provider = row.DataBoundItem as DrugShop.Entities.Provider;

            if(provider == null)
            {
                return;
            }

            ProviderEditorForm o = new ProviderEditorForm();
            o.Provider = provider;

            if(o.ShowDialog() == DialogResult.OK)
            {
                DrugShop.Entities.Provider var = o.Provider;

                this.InitRowInfo(row, var);
            }
        }

        /// <summary>
        /// ��¼ɾ����
        /// </summary>
        protected void ItemDelete()
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;

            if (rowIndex < 0)
            {
                return;
            }

            DataGridViewRow row = dataGridView1.Rows[rowIndex];

            DrugShop.Entities.Provider provider = row.DataBoundItem as DrugShop.Entities.Provider;

            if(provider == null)
            {
                return;
            }

            string tip = "����" ;
            if(MessageBox.Show("��ȷ��Ҫɾ����ѡ���" + tip + "��¼ô��\n��ȷ�����Ĳ�����", "ȷ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                provider.Delete();

                this.dataGridView1.Rows.Remove(row);
            }
        }

        /// <summary>
        /// ˢ�µ�ǰ�б����ݡ�
        /// </summary>
        protected void ListRefresh()
        {
            this.InitList();
        }

        /// <summary>
        /// ��¼������
        /// </summary>
        protected void ListSearch()
        {
            MessageBox.Show("search");
        }

        #endregion

        #region ��ʼ���б����

        /// <summary>
        /// ����ָ�����Ե����м�¼���б�
        /// </summary>
        protected void InitList()
        {
            DrugShop.Entities.Provider provider = new Entities.Provider();

            if (providerList == null)
                providerList = new List<Provider>();

            providerList= provider.GetAll();

            this.InitList(providerList);
        }

        /// <summary>
        /// ��ָ��������Դ�еļ�¼�󶨵��б�
        /// </summary>
        protected void InitList(IList<Provider> list)
        {
            //����ص��ṩ������Դ��Ϣ
            this.providerBindingSource.DataSource = list;
        }

        #endregion

        /// <summary>
        /// ��¼�˵���Ŀ��������ơ�
        /// </summary>
        private void EnabledItemMenu(bool enabled)
        {
            this.cmiItemProperty.Enabled = enabled;
            this.cmiItemDelete.Enabled = enabled;

            this.btnItemProperty.Enabled = enabled;
            this.btnItemDelete.Enabled = enabled;
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.OnExited(e);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool enabled = e.RowIndex !=-1;
            this.EnabledItemMenu(enabled);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            this.ItemProperty(row);
        }
    }
}
