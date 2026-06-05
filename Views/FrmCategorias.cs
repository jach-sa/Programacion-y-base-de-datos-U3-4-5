using System;
using System.Collections.Generic;
using System.Text;
using ChefRecetasCS.Controllers;
using ChefRecetasCS.Models;
using System.Drawing;
using System.Windows.Forms;

namespace ChefRecetasCS.Views
{
    public partial class FrmCategorias : Form
    {
        // =====================================
        // USUARIO ACTUAL
        // =====================================

        private Usuario _usuario;

        // =====================================
        // COMPONENTES
        // =====================================

        private DataGridView dgv =
            new DataGridView();

        private TextBox txtBuscar =
            new TextBox();

        private TextBox txtDescripcion =
    new TextBox
    {
        Width = 220,
        Multiline = true,
        Height = 80
    };

        private TextBox txtIcono =
            new TextBox
            {
                Width = 220
            };

        private Button btnImagen =
            new Button
            {
                Text = "Examinar"
            };

        private PictureBox picIcono =
            new PictureBox
            {
                Width = 120,
                Height = 120,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };

        private Button btnBuscar =
            new Button
            {
                Text = "Buscar"
            };

        private TextBox txtNombre =
            new TextBox
            {
                Width = 180
            };

        private Button btnNuevo =
            new Button
            {
                Text = "Nuevo"
            };

        private Button btnGuardar =
            new Button
            {
                Text = "Guardar"
            };

        private Button btnEliminar =
            new Button
            {
                Text = "Eliminar"
            };

        private CategoriaController ctrl =
            new CategoriaController();

        private int idSeleccion = 0;

        // =====================================
        // CONSTRUCTOR
        // =====================================

        public FrmCategorias(Usuario u)
        {
            _usuario = u;

            Dock = DockStyle.Fill;

            BackColor = Color.Beige;

            ConfigurarDGV();

            // =====================================
            // PANEL SUPERIOR BUSQUEDA
            // =====================================

            var topPanel =
                new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 50,
                    BackColor = Color.Beige
                };

            txtBuscar.Width = 220;

            txtBuscar.Location =
                new Point(10, 12);

            btnBuscar.Location =
                new Point(240, 10);

            btnBuscar.BackColor =
                Color.SandyBrown;

            btnBuscar.ForeColor =
                Color.White;

            btnBuscar.FlatStyle =
                FlatStyle.Flat;

            btnBuscar.Click +=
                (s, e) =>
                {
                    string texto =
                        txtBuscar.Text
                            .ToLower();

                    dgv.DataSource =
                        ctrl.ObtenerTodas()
                            .FindAll(c =>
                                c.Nombre
                                    .ToLower()
                                    .Contains(texto));
                };

            topPanel.Controls.Add(
                txtBuscar);

            topPanel.Controls.Add(
                btnBuscar);

            // =====================================
            // SPLIT CONTAINER
            // =====================================

            var panel =
                new SplitContainer
                {
                    Dock = DockStyle.Fill,
                    SplitterDistance = 600
                };

            panel.Panel1.Padding =
                new Padding(5);

            panel.Panel1.Controls.Add(
                dgv);

            panel.Panel2.Controls.Add(
                Formulario());

            // =====================================
            // CONTROLES
            // =====================================

            Controls.Add(panel);

            Controls.Add(topPanel);

            dgv.SelectionChanged +=
                DGV_SelectionChanged;

            btnNuevo.Click +=
                (s, e) => Limpiar();

            btnGuardar.Click +=
                (s, e) => Guardar();

            btnEliminar.Click +=
                (s, e) => Eliminar();

            btnImagen.Click += BtnImagen_Click;

            // =====================================
            // PERMISOS
            // =====================================

            AplicarPermisos();

            CargarDGV();
        }

        // =====================================
        // PERMISOS POR ROL
        // =====================================

        private void AplicarPermisos()
        {
            string rol =
                _usuario.Rol.ToLower();

            // CONSULTOR
            if (rol == "consultor")
            {
                txtNombre.Enabled = false;

                btnNuevo.Enabled = false;
                btnGuardar.Enabled = false;
                btnEliminar.Enabled = false;
            }

            // OPERADOR
            else if (rol == "operador")
            {
                btnEliminar.Enabled = false;
            }

            // ADMIN
            else if (rol == "admin")
            {
                // acceso total
            }
        }

        // =====================================
        // CONFIGURAR DGV
        // =====================================

        private void ConfigurarDGV()
        {
            dgv.Dock = DockStyle.Fill;

            dgv.ReadOnly = true;

            dgv.SelectionMode =
                DataGridViewSelectionMode
                    .FullRowSelect;

            dgv.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode
                    .Fill;

            dgv.AllowUserToAddRows = false;

            dgv.BackgroundColor =
                Color.White;
        }

        // =====================================
        // FORMULARIO
        // =====================================

        private Panel Formulario()
        {
            var p = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.Beige,
                    AutoScroll = true
                };

            // NOMBRE

            p.Controls.Add(
                new Label
                {
                    Text = "Nombre:",
                    Location = new Point(10, 30),
                    AutoSize = true,
                    Font = new Font(
                        "Segoe UI",
                        10,
                        FontStyle.Bold)
                });

            txtNombre.Location =
                new Point(90, 28);

            p.Controls.Add(txtNombre);

            // DESCRIPCION

            p.Controls.Add(
                new Label
                {
                    Text = "Descripción:",
                    Location = new Point(10, 70),
                    AutoSize = true,
                    Font = new Font(
                        "Segoe UI",
                        10,
                        FontStyle.Bold)
                });

            txtDescripcion.Location =
                new Point(90, 70);

            p.Controls.Add(txtDescripcion);

            // ICONO

            p.Controls.Add(
                new Label
                {
                    Text = "Imagen:",
                    Location = new Point(10, 170),
                    AutoSize = true,
                    Font = new Font(
                        "Segoe UI",
                        10,
                        FontStyle.Bold)
                });

            txtIcono.Location =
                new Point(90, 168);

            p.Controls.Add(txtIcono);

            btnImagen.Location =
                new Point(90, 200);

            btnImagen.BackColor =
                Color.SandyBrown;

            btnImagen.ForeColor =
                Color.White;

            btnImagen.FlatStyle =
                FlatStyle.Flat;

            p.Controls.Add(btnImagen);

            // PICTUREBOX

            picIcono.Location =
                new Point(90, 240);

            p.Controls.Add(picIcono);

            // BOTONES

            btnNuevo.Location =
                new Point(10, 390);

            btnGuardar.Location =
                new Point(90, 390);

            btnEliminar.Location =
                new Point(170, 390);

            btnNuevo.BackColor =
                Color.SandyBrown;

            btnGuardar.BackColor =
                Color.SandyBrown;

            btnEliminar.BackColor =
                Color.Firebrick;

            btnNuevo.ForeColor =
                Color.White;

            btnGuardar.ForeColor =
                Color.White;

            btnEliminar.ForeColor =
                Color.White;

            btnNuevo.FlatStyle =
                FlatStyle.Flat;

            btnGuardar.FlatStyle =
                FlatStyle.Flat;

            btnEliminar.FlatStyle =
                FlatStyle.Flat;

            p.Controls.AddRange(
                new Control[]
                {
            btnNuevo,
            btnGuardar,
            btnEliminar
                });

            return p;
        }

        // =====================================
        // CARGAR TABLA
        // =====================================

        private void CargarDGV()
        {
            dgv.DataSource = null;

            dgv.DataSource =
                ctrl.ObtenerTodas();
        }

        // =====================================
        // GUARDAR IMAGEN

        private void BtnImagen_Click(
    object sender,
    EventArgs e)
        {
            using var ofd =
                new OpenFileDialog();

            ofd.Filter =
                "Imágenes|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog()
                == DialogResult.OK)
            {
                txtIcono.Text =
                    ofd.FileName;

                picIcono.Image =
                    Image.FromFile(
                        ofd.FileName);
            }
        }

        // =====================================
        // SELECCION
        // =====================================

        private void DGV_SelectionChanged(
    object sender,
    EventArgs e)
        {
            if (dgv.CurrentRow == null)
                return;

            var c =
                (Categoria)dgv
                    .CurrentRow
                    .DataBoundItem;

            idSeleccion =
                c.IdCategoria;

            txtNombre.Text =
                c.Nombre;

            txtDescripcion.Text =
                c.Descripcion;

            txtIcono.Text =
                c.Icono;

            try
            {
                if (!string.IsNullOrWhiteSpace(c.Icono)
                    && System.IO.File.Exists(c.Icono))
                {
                    picIcono.Image =
                        Image.FromFile(c.Icono);
                }
                else
                {
                    picIcono.Image = null;
                }
            }
            catch
            {
                picIcono.Image = null;
            }
        }

        // =====================================
        // GUARDAR
        // =====================================

        private void Guardar()
        {
            var c =
    new Categoria
    {
        IdCategoria = idSeleccion,

        Nombre =
            txtNombre.Text.Trim(),

        Descripcion =
            txtDescripcion.Text.Trim(),

        Icono =
            txtIcono.Text.Trim()
    };
        }

        // =====================================
        // ELIMINAR
        // =====================================

        private void Eliminar()
        {
            if (idSeleccion == 0)
                return;

            if (MessageBox.Show(
                "¿Eliminar categoría?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                ctrl.Eliminar(idSeleccion);

                CargarDGV();

                Limpiar();
            }
        }

        // =====================================
        // LIMPIAR
        // =====================================

        private void Limpiar()
        {
            idSeleccion = 0;

            txtNombre.Clear();

            txtDescripcion.Clear();

            txtIcono.Clear();

            picIcono.Image = null;
        }
    }
}