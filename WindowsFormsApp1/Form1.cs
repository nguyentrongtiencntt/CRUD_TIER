using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using CRUD.Core;

namespace WindowsFormsApp1
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        EntityState objState = EntityState.Unchanged;
        public Form1()
        {
            InitializeComponent();
        }


        void ClearInput()
        {
            txtFullname.Text = null;
            txtEmail.Text = null;
            chkGender.Checked = false;
            txtBirthday.Text = null;
            txtAddress.Text = null;
            pic.Image = null;
        }

        private void htmlLabel5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                studentBindingSource.DataSource = MyService.Student.GetAll();
                    pContainer.Enabled = false;

                    Student obj = studentBindingSource.Current as Student;
                    if (obj != null)
                    {
                        if (!string.IsNullOrEmpty(obj.ImageUrl))
                            pic.Image = Image.FromFile(obj.ImageUrl);

                    }
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnBrow_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG|*.jpg|PNG|*.png", ValidateNames = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pic.Image = Image.FromFile(ofd.FileName);
                    Student obj = studentBindingSource.Current as Student;
                    if (obj != null)
                        obj.ImageUrl = ofd.FileName;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            objState = EntityState.Added;
            pic.Image = null;
            pContainer.Enabled = true;
            studentBindingSource.Add(new Student());
            studentBindingSource.MoveLast();
            txtFullname.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pContainer.Enabled = false;
            studentBindingSource.ResetBindings(false);
            //ClearInput();
            //this.Close();
            this.Form1_Load(sender, e);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            objState = EntityState.Changed;
            pContainer.Enabled = true;
            txtFullname.Focus();
        }

        private void metroGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Student obj = studentBindingSource.Current as Student;
                if (obj != null)
                {
                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                        pic.Image = Image.FromFile(obj.ImageUrl);

                }
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            objState = EntityState.Deleted;
            if (MetroFramework.MetroMessageBox.Show(this, "Are you sure want to delete this record?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Student obj = studentBindingSource.Current as Student;
                    if (obj != null)
                    {                       
                            bool result = MyService.Student.Delete(obj.StudentID);
                            if(result)
                            {
                                studentBindingSource.RemoveCurrent();
                                pContainer.Enabled = false;
                                pic.Image = null;
                                objState = EntityState.Unchanged;
                            }
                    }
                }
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(this, ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                studentBindingSource.EndEdit();
                Student obj = studentBindingSource.Current as Student;
                if (obj != null)
                {
                   
                        if (objState == EntityState.Added)
                            obj.StudentID = MyService.Student.Insert(obj);
                        else 
                        if(objState==EntityState.Changed) 
                            MyService.Student.Update(obj);
                        
                        metroGrid1.Refresh();
                        pContainer.Enabled = false;
                        objState = EntityState.Unchanged;

                }
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void chkGender_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkGender.CheckState == CheckState.Checked)
                chkGender.Text = "Female";
            else
                if (chkGender.CheckState == CheckState.Unchecked)
                chkGender.Text = "Male";
            else
                chkGender.Text = "???";
        }
    }
}
