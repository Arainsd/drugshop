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
    public partial class SalePriceInput : System.Windows.Forms.Form
    {
        private bool validate;

        public SalePriceInput()
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


        public decimal OldPrice
        {
            set
            {
                this.lbOldPrice.Text = value.ToString("F2");
            }
        }

        public decimal Number
        {
            get
            {
                return decimal.Parse(this.tbNumber.Text);
            }
            set
            {
                this.tbNumber.Text = value.ToString("F2");
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

                if (decimal.Parse(this.tbNumber.Text) <= decimal.Zero)
                {
                    MessageBox.Show("�������ֲ���С�ڻ�����㣬���������룡", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void SalePriceInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}