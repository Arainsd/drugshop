using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualBasic;

using DrugShop.Entities;
namespace DrugShop.WinUI
{
    public partial class DrugInput : System.Windows.Forms.Form
    {
        public DrugInput()
        {
            InitializeComponent();
            this.controlAutoFocus1.NextKeys = new Keys[] { Keys.Enter };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.DialogResult = DialogResult.None;

            this.BindProvider();

            if (this.Provider != 0)
                this.cbxProvider.SelectedValue = this.Provider;
        }

        public string BitchID
        {
            get
            {
                return this.tbBitchID.Text;
            }
            set
            {
                this.tbBitchID.Text = value;
            }
        }

        public DateTime TimeLimit
        {
            get
            {
                return this.dtpTimeLimit.Value;
            }
            set
            {
                this.dtpTimeLimit.Value = value;
            }
        }

        public int Number
        {
            get
            {
                return int.Parse(this.tbNumber.Text);
            }
            set
            {
                this.tbNumber.Text = value.ToString();
            }
        }        

        public int Provider
        {
            get
            {
                return (int)this.cbxProvider.SelectedValue;
            }
            set
            {
                this.provider = value;
                this.cbxProvider.SelectedValue = value;
            }
        }
        private int provider;

        public string ProviderName
        {
            get
            {
                return this.cbxProvider.Text;
            }            
        }

        internal void BindProvider()
        {
            DrugShop.Entities.Provider provider = new DrugShop.Entities.Provider();
            IList<Provider> PL = provider.GetProviderList();

            this.cbxProvider.DataSource = PL;
            this.cbxProvider.ValueMember = "ID";
            this.cbxProvider.DisplayMember = "Name";
        }

        private bool ValidateNumber()
        {
            string s = this.tbNumber.Text.Trim();

            if (s == "")
            {
                MessageBox.Show("���벻��Ϊ�գ����������룡", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!Information.IsNumeric(s))
            {
                MessageBox.Show("���ָ�ʽ�������������룡", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.tbNumber.SelectAll();
                return false;
            }

            if (s.IndexOf(".") > -1)
            {
                MessageBox.Show("���ָ�ʽ�������������룡", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.tbNumber.SelectAll();
                return false;
            }

            if (int.Parse(this.tbNumber.Text) < 1)
            {
                MessageBox.Show("�������ֲ���С��1�����������룡", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.tbNumber.SelectAll();
                return false;
            }

            return true;
        }

        private void tbNumber_KeyDown(object sender, KeyEventArgs e)
        {
             if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if(ValidateNumber())
                    this.cbxProvider.Focus();
            }
        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateNumber())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}