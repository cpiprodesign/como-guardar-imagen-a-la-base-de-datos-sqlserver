using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace imagen
{
    public partial class Form1 : Form
    {
        string imagePath;//variable global de ruta de imagen
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=CPIPRODESIGN\SQLEXPRESS;Initial Catalog=imagen;Integrated Security=True";
            //string imagePath = @"C:\Users\LENOVO\OneDrive\Imágenes\logo-systfarma-ico-nuevo.png"; // Cambia esto a la ruta de tu imagen

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Lee la imagen desde el archivo y conviértela en bytes
                    byte[] imageData;
                    using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                    {
                        imageData = new byte[fs.Length];
                        fs.Read(imageData, 0, (int)fs.Length);
                    }

                    // Guarda la imagen en la base de datos
                    string insertQuery = "INSERT INTO Imagenes (Id, Nombre, Imagen) VALUES (@Id, @Nombre, @Imagen)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", textBox1.Text); // Cambia el valor del Id según tus necesidades
                        cmd.Parameters.AddWithValue("@Nombre",textBox2.Text); // Cambia el nombre de la imagen según tus necesidades
                        cmd.Parameters.AddWithValue("@Imagen", imageData);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Abre un cuadro de diálogo para seleccionar la imagen
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen|*.png;*.jpg;*.bmp;*.gif";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Obtiene la ruta del archivo seleccionado
                 imagePath = openFileDialog.FileName;

                // Carga la imagen en el PictureBox
                pictureBox1.Image = Image.FromFile(imagePath);
            }
        }
    }
}
