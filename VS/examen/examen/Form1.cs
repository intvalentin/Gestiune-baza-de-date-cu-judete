using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace examen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            A1();

        }

        private void btnAdaugare_Click(object sender, EventArgs e)
        {
            A2();
        }

        private void A1()
        {
            dataTable1TableAdapter.Fill(dataSet1.DataTable1);
            

            cmbJudet.DropDownStyle = ComboBoxStyle.Simple;

            PB.SizeMode = PictureBoxSizeMode.StretchImage;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;

            txtIDL.ReadOnly = true;

            A3();
        }

        private void A2()
        {
            //Configurare butoane
            configureazaButoane(false);
            //Dezlegare controale Panel
            legareControale(false);
            //Ridicare protectie controale Panel
            protectiePanel(false);
            //Configurare(ComboBox - Uri)
            cmbJudet.DropDownStyle = ComboBoxStyle.DropDownList;
            //Legare comboBox-uri la sursa de date
            cmbJudet.DataSource = dataTable1BindingSource;
            cmbJudet.DisplayMember = "DJudet";
            cmbJudet.ValueMember = "Judete_IdJudet";
            
            //Modifcare lblOp
            lblOp.Text = "ADAUGARE";
            //Pozitionare cursor pe primul camp
            txtDL.Focus();
            // Golire campuri
            golireCampuri();
        }


        private void configureazaButoane(bool v)
        {
            btnRenuntare.Visible = !v;
            btnConfirmare.Visible = !v;
            btnAdaugare.Enabled = v;
            
        }

        private void legareControale(bool v)
        {
            if (v)
            {
                txtDL.DataBindings.Add("Text", dataTable1BindingSource, "DLocalitate");
                txtCL.DataBindings.Add("Text", dataTable1BindingSource, "CodLocalitate");

                cmbJudet.DataBindings.Add("Text", dataTable1BindingSource, "DJudet");
                txtSPI.DataBindings.Add("Text", dataTable1BindingSource, "SpImagine");
                txtIDL.DataBindings.Add("Text", dataTable1BindingSource, "IdLocalitate");
                PB.DataBindings.Add("ImageLocation", dataTable1BindingSource, "SpImagine");
            }
            else
            {
                txtDL.DataBindings.Clear();
                txtCL.DataBindings.Clear();
                
                cmbJudet.DataBindings.Clear();
                txtSPI.DataBindings.Clear();
                txtIDL.DataBindings.Clear();
                PB.DataBindings.Clear();
            }
        }

        private void protectiePanel(bool v)
        {
            txtDL.ReadOnly = v;
            txtCL.ReadOnly = v;

            txtSPI.ReadOnly = v;
            cmbJudet.Enabled = !v;
        }

        private void golireCampuri()
        {
            txtDL.Text = "";
            txtCL.Text = "";
            txtIDL.Text = "";

            txtSPI.Text = "";
            cmbJudet.SelectedIndex = -1;
            PB.ImageLocation = "";
        }

        private void btnRenuntare_Click(object sender, EventArgs e)
        {
            A3();
        }

        private void A3()
        {
            //Configurare comboBox-uri
            cmbJudet.DropDownStyle = ComboBoxStyle.Simple;
            //Modifcare lblOp
            lblOp.Text = "";
            //Çonfigurare(butoane)
            configureazaButoane(true);
            //Protectie componente Panel
            protectiePanel(true);
            //Legare controale
            legareControale(true);
        }

        private void btnConfirmare_Click(object sender, EventArgs e)
        {
            A4();
        }

        private void A4()
        {
            if (lblOp.Text == "ADAUGARE")
            {
                if (!validareCampuriObligatorii()) return;
                adauga_inregistrare();
                golireCampuri();
                //Pune cursor pe primul camp
                txtDL.Focus();
                refresh_grid(dataTable1BindingSource.Position);
            }
            else
                MessageBox.Show("Operatie actualizare neefectuata");
        }

        private bool validareCampuriObligatorii()
        {
            //Validare de completare obligatorie campurile: Nume, CNP, Salariu, Localitate
            if (txtDL.Text == "")
            {
                MessageBox.Show("Completati Denumirea !");
                txtDL.Focus();
                return false;
            }
            if (txtCL.Text == "")
            {
                MessageBox.Show("Completati Codul !");
                txtCL.Focus();
                return false;
            }
            //Validare Cod
            if (verificaSemantic(txtCL.Text))
            {
                MessageBox.Show("Codul exista deja !");
                txtCL.Focus();
                return false;
            }
            if (cmbJudet.Text == "")
            {
                MessageBox.Show("Completati Judetul !");
                cmbJudet.Focus();
                return false;
            }
            if (txtSPI.Text == "")
            {
                MessageBox.Show("Alegeti Imaginea !");
                cmbJudet.Focus();
                return false;
            }
            
            return true;
        }

        private void adauga_inregistrare()
        {
            string listaCampuri;
            string listaValori;
            OleDbConnection con = new OleDbConnection();
            OleDbCommand cmd = new OleDbCommand();
            con.ConnectionString = localitatiTableAdapter.Connection.ConnectionString;
            cmd.Connection = con;
            listaCampuri = "Dlocalitate, CodLocalitate, IdJudet,SpImagine";
            listaValori = "'" + txtDL.Text + "'" +
            ",'" + txtCL.Text + "'" +
            "," + cmbJudet.SelectedValue +
            ",'" + txtSPI.Text + "'";
            cmd.CommandText = "Insert into Localitati(" + listaCampuri + ") " +
            "Select " + listaValori;
            MessageBox.Show(cmd.CommandText);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void refresh_grid(int p)
        {
            dataTable1TableAdapter.Fill(dataSet1.DataTable1);
            dataTable1BindingSource.Position = p;
        }

        private void txtCL_Leave(object sender, EventArgs e)
        {
            A5(txtCL);
        }

        private void A5(TextBox txtB)
        {
            decimal p;
            if (txtCL.Text == "") return;
            if (verificaSemantic(txtCL.Text))
            {
                MessageBox.Show("Codul exista deja !");
                txtCL.Focus();
                
            }
            try
            {
                p = Convert.ToDecimal(txtB.Text);
            }
            catch
            {
                MessageBox.Show("Format eronat");
                txtB.Focus();
            }


        }

        private void txtSPI_DoubleClick(object sender, EventArgs e)
        {
            A6();
        }

        private void A6()
        {
            if (lblOp.Text == "")
                return;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtSPI.Text = openFileDialog1.FileName;
                PB.ImageLocation = txtSPI.Text;
            }
        }

        private void cmbJudet_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtIDL.Text = cmbJudet.SelectedValue.ToString();
            }catch{}
        }

        private Boolean verificaSemantic(string s) 
        {
           
            string cldatabase;
            foreach (DataRow r in dataSet1.DataTable1)
            {
                cldatabase = r["CodLocalitate"].ToString();

                if (string.Equals(cldatabase, s))
                    return true;
            }
            return false;
        }
    }
}
