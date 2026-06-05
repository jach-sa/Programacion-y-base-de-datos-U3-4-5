using System;
using System.Collections.Generic;
using System.Text;
using ChefRecetasCS.Controllers;
using ChefRecetasCS.Models;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ChefRecetasCS.Views
{
    public partial class FrmIngredientes : Form
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

        private TextBox txtUnidad =
            new TextBox
            {
                Width = 180
            };

        private TextBox txtCalorias =
            new TextBox
            {
                Width = 100
            };

        private PictureBox pic =
            new PictureBox
            {
                Width = 120,
                Height = 120,

                SizeMode =
                    PictureBoxSizeMode.Zoom,

                BorderStyle =
                    BorderStyle.FixedSingle
            };

        private Button btnFoto =
            new Button
            {
                Text = "Foto..."
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

        // =====================================
        // CONTROLADOR
        // =====================================

        private IngredienteController ctrl =
            new IngredienteController();

        // =====================================
        // VARIABLES
        // =====================================

        private byte[] imagenBytes = null;

        private int idSeleccion = 0;

        // =====================================
        // CONSTRUCTOR
        // =====================================

        public FrmIngredientes(Usuario u)
        {
            _usuario = u;

            Dock = DockStyle.Fill;

            BackColor = Color.Beige;

            ConfigurarDGV();

            // =====================================
            // PANEL BUSQUEDA
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
                        ctrl.ObtenerIngredientes()
                            .FindAll(i =>
                                i.Nombre
                                    .ToLower()
                                    .Contains(texto));
                };

            topPanel.Controls.Add(
                txtBuscar);

            topPanel.Controls.Add(
                btnBuscar);

            // =====================================
            // SPLIT
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
                PanelFormulario());

            Controls.Add(panel);

            Controls.Add(topPanel);

            // =====================================
            // EVENTOS
            // =====================================

            dgv.SelectionChanged +=
                DGV_SelectionChanged;

            btnNuevo.Click +=
                (s, e) => Limpiar();

            btnGuardar.Click +=
                (s, e) => Guardar();

            btnEliminar.Click +=
                (s, e) => Eliminar();

            btnFoto.Click +=
                (s, e) => SeleccionarFoto();

            // =====================================
            // PERMISOS
            // =====================================

            AplicarPermisos();

            // =====================================
            // CARGAR DATOS
            // =====================================

            CargarDGV();
        }

        // =====================================
        // PERMISOS POR ROL
        // =====================================

        private void AplicarPermisos()
        {
            string rol =
                _usuario.Rol.ToLower();

            // =====================================
            // CONSULTOR
            // =====================================

            if (rol == "consultor")
            {
                txtNombre.Enabled = false;

                txtUnidad.Enabled = false;

                txtCalorias.Enabled = false;

                btnFoto.Enabled = false;

                btnNuevo.Enabled = false;

                btnGuardar.Enabled = false;

                btnEliminar.Enabled = false;
            }

            // =====================================
            // OPERADOR
            // =====================================

            else if (rol == "operador")
            {
                btnEliminar.Enabled = false;
            }

            // =====================================
            // ADMIN
            // =====================================

            else if (rol == "admin")
            {
                // acceso total
            }
        }

        // =====================================
        // CONFIGURAR TABLA
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
        // PANEL FORMULARIO
        // =====================================

        private Panel PanelFormulario()
        {
            var p =
                new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    BackColor = Color.Beige
                };

            int y = 40;

            void Fila(
                string etiq,
                Control ctrl)
            {
                p.Controls.Add(
                    new Label
                    {
                        Text = etiq,

                        Location =
                            new Point(
                                5,
                                y + 3),

                        AutoSize = true,

                        Font =
                            new Font(
                                "Segoe UI",
                                10,
                                FontStyle.Bold)
                    });

                ctrl.Location =
                    new Point(110, y);

                p.Controls.Add(ctrl);

                y += ctrl.Height + 12;
            }

            Fila("Nombre:", txtNombre);

            Fila("Unidad:", txtUnidad);

            Fila("Calorías:", txtCalorias);

            pic.Location =
                new Point(110, y);

            btnFoto.Location =
                new Point(240, y + 40);

            btnFoto.BackColor =
                Color.SandyBrown;

            btnFoto.ForeColor =
                Color.White;

            btnFoto.FlatStyle =
                FlatStyle.Flat;

            p.Controls.AddRange(
                new Control[]
                {
                    new Label
                    {
                        Text = "Imagen:",

                        Location =
                            new Point(
                                5,
                                y + 3),

                        AutoSize = true,

                        Font =
                            new Font(
                                "Segoe UI",
                                10,
                                FontStyle.Bold)
                    },

                    pic,

                    btnFoto
                });

            y += 150;

            // =====================================
            // BOTONES
            // =====================================

            btnNuevo.Location =
                new Point(10, y);

            btnGuardar.Location =
                new Point(100, y);

            btnEliminar.Location =
                new Point(190, y);

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
                ctrl.ObtenerIngredientes();
        }

        // =====================================
        // SELECCION TABLA
        // =====================================

        private void DGV_SelectionChanged(
            object sender,
            EventArgs e)
        {
            if (dgv.CurrentRow == null)
                return;

            var i =
                (Ingrediente)dgv
                    .CurrentRow
                    .DataBoundItem;

            idSeleccion =
                i.IdIngrediente;

            txtNombre.Text =
                i.Nombre;

            txtUnidad.Text =
                i.UnidadMedida;

            txtCalorias.Text =
                i.CaloriasPorU
                    .ToString();

            imagenBytes =
                i.Imagen;

            if (i.Imagen != null)
            {
                pic.Image =
                    Image.FromStream(
                        new MemoryStream(
                            i.Imagen));
            }
            else
            {
                pic.Image = null;
            }
        }

        // =====================================
        // SELECCIONAR FOTO
        // =====================================

        private void SeleccionarFoto()
        {
            if (_usuario.Rol.ToLower() == "consultor")
            {
                MessageBox.Show(
                    "No tienes permisos para cambiar imágenes.",
                    "Permisos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            using var ofd =
                new OpenFileDialog
                {
                    Filter =
                        "Imágenes|*.jpg;*.jpeg;*.png;*.bmp"
                };

            if (ofd.ShowDialog()
                == DialogResult.OK)
            {
                imagenBytes =
                    File.ReadAllBytes(
                        ofd.FileName);

                pic.Image =
                    Image.FromFile(
                        ofd.FileName);
            }
        }

        // =====================================
        // GUARDAR
        // =====================================

        private void Guardar()
        {
            if (_usuario.Rol.ToLower() == "consultor")
            {
                MessageBox.Show(
                    "No tienes permisos para guardar ingredientes.",
                    "Permisos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            decimal cal = 0;

            decimal.TryParse(
                txtCalorias.Text,
                out cal);

            var i =
                new Ingrediente
                {
                    IdIngrediente =
                        idSeleccion,

                    Nombre =
                        txtNombre.Text.Trim(),

                    UnidadMedida =
                        txtUnidad.Text.Trim(),

                    CaloriasPorU = cal,

                    Imagen =
                        imagenBytes
                };

            ctrl.GuardarIngrediente(i);

            CargarDGV();

            Limpiar();

            MessageBox.Show(
                "Ingrediente guardado correctamente.",
                "Éxito",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // =====================================
        // ELIMINAR
        // =====================================

        private void Eliminar()
        {
            string rol =
                _usuario.Rol.ToLower();

            if (rol != "admin")
            {
                MessageBox.Show(
                    "Solo el administrador puede eliminar ingredientes.",
                    "Permisos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (idSeleccion == 0)
                return;

            if (MessageBox.Show(
                "¿Eliminar ingrediente?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                ctrl.EliminarIngrediente(
                    idSeleccion);

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

            imagenBytes = null;

            txtNombre.Clear();

            txtUnidad.Clear();

            txtCalorias.Clear();

            pic.Image = null;
        }
    }
}