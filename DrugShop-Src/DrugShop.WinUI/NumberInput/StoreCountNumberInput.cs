using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace DrugShop.WinUI
{
    public partial class StoreCountNumberInput : System.Windows.Forms.Form
    {
        private bool validate;

        public StoreCountNumberInput()
        {
            InitializeComponent();

            this.validate = false;
        }

        /// <summary>
        /// ���������¼���
        /// </summary>
        public event System.EventHandler NumberValidate;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.DialogResult = DialogResult.None;
        }

        private decimal storeNumber = 0;
        public decimal StoreNumber
        {
            get
            {
                return this.storeNumber;
            }
            set
            {
                this.storeNumber = value;
                if (value > 0)
                    this.lbNumber.Text = Convert.ToInt32(storeNumber).ToString()+" ��";
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

        /// <summary>
        /// ָʾ��֤�Ƿ���ɡ�
        /// </summary>
        public bool ValidateFinished
        {
            get
            {
                return this.validate;
            }
            set
            {
                this.validate = value;
            }
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
                string s = this.tbNumber.Text.Trim();

                if (s == "")
                {
                    MessageBox.Show("���벻��Ϊ�գ����������룡", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.validate =false;
                    return;
                }

                if (!Information.IsNumeric(s))
                {
                    MessageBox.Show("���ָ�ʽ�������������룡", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.tbNumber.SelectAll();
                    this.validate =false;
                    return;
                }

                if (s.IndexOf(".") > -1)
                {
                    MessageBox.Show("���ָ�ʽ�������������룡", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.tbNumber.SelectAll();
                    this.validate =false;
                    return;
                }

                if (int.Parse(this.tbNumber.Text) < 1)
                {
                    MessageBox.Show("�������ֲ���С��1�����������룡", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.tbNumber.SelectAll();
                    this.validate =false;
                    return;
                }

                if (this.NumberValidate != null)
                {
                    this.NumberValidate(this, new System.EventArgs());

                    if (this.validate)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        this.tbNumber.SelectAll();
                    }
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void NumberInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}